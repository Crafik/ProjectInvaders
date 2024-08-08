using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyPlaneMode{
    /// <summary> Straigth flight, (shotsDesc) </summary>
    Straight,
    /// <summary> Strafe move, (shotsDesc) <br /> 
    /// Rotate to desired direction </summary>
    Strafe,
    /// <summary> Moves across screen and turns 180 degrees. <br />
    /// Turns upwards when coming from left and vice versa <br />
    /// (shotsDesc) </summary>
    Turn
}

public class EnemyPlaneBehaviour : BaseEnemy
{
    // Plane type enemy:
    // It appears on screen to fly through and bombard player with bullets.
    delegate void MoveDelegate();
    private MoveDelegate moveDelegate;

    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject smallBulletPrefab;
    [SerializeField] private GameObject explosionPrefab;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float shotCooldown;

    private Quaternion targetAngle; // for turns
    private Vector3 moveDirection;  // for strafes

    public void Init(EnemyPlaneMode mode){
        // here be something
        switch (mode){
            case EnemyPlaneMode.Straight:
                moveDelegate = MoveStraight;
                break;
            case EnemyPlaneMode.Strafe:
                moveDelegate = MoveStrafe;
                moveDirection = transform.forward;
                m_Rigidbody.rotation = Quaternion.identity;
                m_Rigidbody.MoveRotation(Quaternion.Euler(new Vector3(0f, 0f, m_Rigidbody.position.x < 0 ? -35f : 35f)));
                break;
            case EnemyPlaneMode.Turn:
                moveDelegate = MoveAndTurn;
                moveSpeed *= 2f;
                m_Rigidbody.rotation = Quaternion.Euler(new Vector3(0f, m_Rigidbody.position.x < 0 ? 90f : -90f, 0f));
                targetAngle = m_Rigidbody.rotation * Quaternion.Euler(new Vector3(0f, 180f, 0f));
                break;
            default:
                moveDelegate = MoveStraight;
                break;
        }
    }

    void MoveStraight(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * transform.forward);
    }

    void MoveStrafe(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * moveDirection);
    }

    private float v_time = 2f;
    void MoveAndTurn(){
        MoveStraight();
        if (v_time < 0f){
            m_Rigidbody.MoveRotation(Quaternion.RotateTowards(m_Rigidbody.rotation, targetAngle, 270f * Time.fixedDeltaTime));// good enough, predictable enough
        }
        else{
            v_time -= Time.fixedDeltaTime;
        }
    }

    private float shotCooldownCounter = -1f;
    void ShootAtPlayer(){
        var boolet = Instantiate(smallBulletPrefab, transform.position, Quaternion.identity);
        boolet.GetComponent<EnemySmallBulletBehaviour>().Init(GameManagerSingleton.Instance.player.transform.position);
    }

    protected override void GetDestroyed(){
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        InterfaceSingleton.Instance.AddScore(killPrice);
    }

    void FixedUpdate(){
        moveDelegate();
        if (GameManagerSingleton.Instance.isPlayerAlive){
            if (Mathf.Abs(transform.position.x - GameManagerSingleton.Instance.player.transform.position.x) < 3f){
                if (shotCooldownCounter <= 0f){
                    ShootAtPlayer();
                    shotCooldownCounter = shotCooldown;
                }
            }
        }
        if (shotCooldownCounter > 0f){
            shotCooldownCounter -= Time.fixedDeltaTime;
        }
        if (m_Rigidbody.position.x > 17f || m_Rigidbody.position.x < -17f ||
            m_Rigidbody.position.z > 15f || m_Rigidbody.position.z < -7f){
            Destroy(gameObject);
        }
    }
}
