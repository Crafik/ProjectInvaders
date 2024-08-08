using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Little tough guy <br />
/// floats from top of the screen, stops, shoots 3-4-5 bullets in fan spread
/// </summary>
public class EnemyBruteBehaviour : BaseEnemy
{
    delegate void ActionDelegate();
    ActionDelegate actionDelegate;

    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject mediumBullet;
    [SerializeField] private GameObject explosionPrefab;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float shotCooldown;

    private Vector3 startPos;
    private Vector3 destination;
    private float lerpMultiplier;

    /// <summary>
    /// EnemyBrute initialization method
    /// </summary>
    /// <param name="posX">
    /// Horizontal coordinate of target position on grid<br />
    /// Between -8 : 8 inclusive
    /// </param>
    /// <param name="posZ">
    /// Vertical coordinate of target position on grid<br />
    /// Between 1 : 6 inclusive
    /// </param>
    public void Init(Vector3 pos){
        destination = pos;
        startPos = m_Rigidbody.position;
        lerpMultiplier = moveSpeed / Vector3.Distance(startPos, destination); // this voodoo magic works
        m_Rigidbody.rotation = Quaternion.LookRotation(destination - m_Rigidbody.position);
        actionDelegate = MoveInAction;
    }

    protected override void GetDestroyed(){
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        InterfaceSingleton.Instance.AddScore(killPrice);
    }

    private void Shoot(Vector3 targetPos, int count){
        // seems good
        float angleStep = 90f / (count - 1);

        Quaternion startRotation = Quaternion.LookRotation(targetPos - m_Rigidbody.position) * Quaternion.Inverse(Quaternion.AngleAxis(45f, Vector3.up));

        for (int i = 0; i < count; ++i){
            Instantiate(mediumBullet, transform.position, startRotation * Quaternion.AngleAxis(angleStep * i, Vector3.up));
        }
    }

    float lerpCounter = 0f;
    void MoveInAction(){
        if (lerpCounter < 1f){
            m_Rigidbody.MovePosition(Vector3.Lerp(startPos, destination, lerpCounter));
            lerpCounter += Time.fixedDeltaTime * lerpMultiplier; // should work ????????
        }
        else{
            actionDelegate = ShootingAction;
            shootCounter = shotCooldown;
        }
    }

    float shootCounter;
    int bulletcount = 3;
    void ShootingAction(){
        m_Rigidbody.rotation = Quaternion.LookRotation(GameManagerSingleton.Instance.player.transform.position - m_Rigidbody.position);
        if (shootCounter < 0f){
            Shoot(GameManagerSingleton.Instance.player.transform.position, bulletcount);
            bulletcount += 1;
            shootCounter = shotCooldown;
        }
        else{
            shootCounter -= Time.fixedDeltaTime;
        }
        if (bulletcount > 6){
            leavingPos = new Vector3(m_Rigidbody.position.x > 0f ? -20f : 20, 0f, -1f);
            m_Rigidbody.rotation = Quaternion.LookRotation(leavingPos - m_Rigidbody.position);
            actionDelegate = LeavingAction;
        }
    }

    Vector3 leavingPos;
    void LeavingAction(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * transform.forward);
    }

    void FixedUpdate(){
        if (GameManagerSingleton.Instance.isGameActive){
            actionDelegate();

            if (m_Rigidbody.position.x > 17f || m_Rigidbody.position.x < -17f ||
                m_Rigidbody.position.z > 15f || m_Rigidbody.position.z < -7f){
                Destroy(gameObject);
            }
        }
    }
}
