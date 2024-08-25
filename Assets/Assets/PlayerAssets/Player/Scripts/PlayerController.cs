using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private Controls m_controls;

    [Header ("===== Components =====")]
    [SerializeField] private Rigidbody m_body;
    [SerializeField] private Animator m_anim;

    [Space (10)]
    [Header ("===== Children =====")]
    [SerializeField] private PlayerWeaponManager weapons;
    [SerializeField] private GameObject shield;

    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject PlayerExplosion;
    [SerializeField] private GameObject WingsPickUp;
    [SerializeField] private GameObject BombExplosion;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float recoveryPeriod;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float focusMultiplier;
    private Vector2 moveVector;
    private bool isFocused;

    private bool isShooting;

    private int bombsCounter;

    private int livesCounter;
    private bool isAlive;
    private float invincibilityCounter;

    void Awake(){
        m_controls = new Controls();

        isFocused = false;
        isAlive = true;
        livesCounter = 3;
        bombsCounter = 2;
        invincibilityCounter = -1f;
        shield.SetActive(false);
    }

    void OnEnable(){
        m_controls.Enable();

        m_controls.Player.Move.performed += OnMovePerformed;
        m_controls.Player.Move.canceled += OnMovePerformed;
        m_controls.Player.Focus.performed += OnFocusPerformed;
        m_controls.Player.Focus.canceled += OnFocusPerformed;
        m_controls.Player.Shoot.performed += OnShootPerformed;
        m_controls.Player.Shoot.canceled += OnShootPerformed;
        m_controls.Player.Bomb.performed += OnBombPerformed;
    }

    void OnDisable(){
        m_controls.Disable();

        m_controls.Player.Move.performed -= OnMovePerformed;
        m_controls.Player.Move.canceled -= OnMovePerformed;
        m_controls.Player.Focus.performed -= OnFocusPerformed;
        m_controls.Player.Focus.canceled -= OnFocusPerformed;
        m_controls.Player.Shoot.performed -= OnShootPerformed;
        m_controls.Player.Shoot.canceled -= OnShootPerformed;
        m_controls.Player.Bomb.performed -= OnBombPerformed;
    }

    void Start()
    {
        // sumthin go here probably
        InterfaceSingleton.Instance.SetLivesCounter(livesCounter);
        InterfaceSingleton.Instance.SetBombsCounter(bombsCounter);
    }

    public void PowerUp(){
        weapons.SetPowerLevel(true);
    }

    public void SetPowerLevel(int level){
        weapons.SetPowerLevel(level);
    }

    void OnMovePerformed(InputAction.CallbackContext ctx){
            moveVector = ctx.ReadValue<Vector2>();
            int moveSide = moveVector.x != 0 ? (moveVector.x > 0 ? 1 : -1) : 0; // looks ugly, should not be that impactfull though
            m_anim.SetInteger("HorizontalMove", moveSide);
            moveVector.Normalize();
    }

    void OnFocusPerformed(InputAction.CallbackContext ctx){
        isFocused = ctx.performed;
        m_anim.SetBool("IsFocused", isFocused);
    }

    void OnShootPerformed(InputAction.CallbackContext ctx){
        isShooting = ctx.performed;
    }

    void OnBombPerformed(InputAction.CallbackContext ctx){
        if (bombsCounter > 0){
            bombsCounter -= 1;
            InterfaceSingleton.Instance.SetBombsCounter(bombsCounter);
            invincibilityCounter = 0.7f;
            StartCoroutine(BombCoroutine());
        }
    }

    IEnumerator BombCoroutine(){
        var bombFX = Instantiate(BombExplosion, transform);
        while (invincibilityCounter > 0f){
            if (Time.timeScale > 0.4f){
                Time.timeScale -= Time.deltaTime;
            }
            float sizeLerp = Mathf.Lerp(0f, 15f, 1f - (invincibilityCounter / 0.7f));
            bombFX.transform.localScale = Vector3.one * sizeLerp;
            InterfaceSingleton.Instance.SetBombFadeIn(1f - (invincibilityCounter / 0.7f));
            invincibilityCounter -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Collider[] colliders = Physics.OverlapBox(new Vector3(0f, 0f, 2f), new Vector3(10f, 2f, 6f));
        foreach (Collider c in colliders){
            if (c.CompareTag("Enemy")){
                c.GetComponent<IDamageable>().GetDamage(50);
            }
            if (c.CompareTag("EnemyProjectile")){
                Destroy(c.gameObject);
            }
        }
        InterfaceSingleton.Instance.ResetBombFadeIn();
        Time.timeScale = 1f;
        Destroy(bombFX);
    }

    void OnCollisionEnter(Collision collision){
        Debug.Log("Collided!");
    }

    void OnTriggerEnter(Collider collision){
        if (collision.CompareTag("Pickable")){
            int scoreToAdd = 0;
            switch (collision.GetComponent<IPickable>().type){
                case PlayerPickUpType.powerUp:
                    weapons.SetPowerLevel(weapons.powerLevel + collision.GetComponent<IPickable>().GetPicked());
                    scoreToAdd = weapons.powerLevel == weapons.maxPowerLevel ? 1200 : 200;
                    break;
                case PlayerPickUpType.bomb:
                    bombsCounter += 1;
                    collision.GetComponent<IPickable>().GetPicked();
                    InterfaceSingleton.Instance.SetBombsCounter(bombsCounter);
                    break;
                case PlayerPickUpType.extraLife:
                    livesCounter += 1;
                    collision.GetComponent<IPickable>().GetPicked(); // That's just silly. Would probably be better with another implementation. ¯\_(ツ)_/¯
                    InterfaceSingleton.Instance.SetLivesCounter(livesCounter);
                    break;
                default:
                    // here probably be safestate(+1000 points)
                    scoreToAdd = 1000;
                    break;
            }
            // here be score addition
            InterfaceSingleton.Instance.AddScore(scoreToAdd);
        }
        if (collision.CompareTag("Enemy")){
            collision.GetComponent<IDamageable>().GetDamage(10);
            PlayerDeath();
        }
    }

    public void PlayerDeath(){
        // TODO: Health drops and game over(rough estimate)
        if (isAlive && invincibilityCounter < 0f){
            livesCounter -= 1;
            InterfaceSingleton.Instance.SetLivesCounter(livesCounter);
            isAlive = false;
            GameManagerSingleton.Instance.isPlayerAlive = false;

            Vector3 pos = m_body.position;
            m_body.velocity = Vector3.zero;
            m_body.position = new Vector3(m_body.position.x, 0f, -6f);

            Instantiate(PlayerExplosion, pos, Quaternion.identity);
            weapons.Shoot(false); // this is wrong =(
            
            if (livesCounter > -1){
                if (weapons.powerLevel > 1){
                    Instantiate(WingsPickUp, pos, Quaternion.identity);
                }
                SetPowerLevel(0);
                StartCoroutine(RespawnCoroutine());
            }
            else{
                // here be game over
            }
        }
    }

    IEnumerator RespawnCoroutine(){ // looks actually good
        float timer = 1f;
        while (timer > 0){
            timer -= Time.deltaTime;
            yield return null;
        }
        timer = 0f;
        Vector3 startPos = m_body.position;
        Vector3 endPos = new Vector3(startPos.x, 0f, -3f);
        while (m_body.position.z != -3f){
            m_body.position = Vector3.Lerp(startPos, endPos, timer);
            timer += Time.deltaTime * 2f;
            yield return null;
        }
        isAlive = true;
        GameManagerSingleton.Instance.isPlayerAlive = true;
        shield.SetActive(true);
        invincibilityCounter = recoveryPeriod;
        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine(){
        float flickerTimer = 0.1f;
        while (invincibilityCounter > 0f){
            invincibilityCounter -= Time.deltaTime;
            if (invincibilityCounter < 1.5f){
                if (flickerTimer < 0f){
                    shield.SetActive(!shield.activeSelf);
                    flickerTimer = 0.1f;
                }
                flickerTimer -= Time.deltaTime;
            }
            yield return null;
        }
        shield.SetActive(false);
    }

    void Update(){
        if (isAlive){
            weapons.Shoot(isShooting);
        }
        else{
            weapons.Shoot(false);
        }
    }

    void FixedUpdate(){
        // dunno if this is good, but kinematic does not collide with static colliders, so yeah
        if (isAlive){
            m_body.velocity = new Vector3(moveVector.x, 0f, moveVector.y) * moveSpeed * (isFocused ? focusMultiplier : 1);
            m_body.position = new Vector3(Mathf.Clamp(m_body.position.x, -9.5f, 9.5f),
                                        0f,
                                        Mathf.Clamp(m_body.position.z, -3.5f, 8f));
        }
    }
}
