using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simpliest enemy. <br />
/// Just flies down the screen, makes one dash up and to the side when reaches certain z-coordinate. <br />
/// Spawn them in groups. <br />
/// Referrence: Fan from Gradius
/// </summary>
public class EnemyMinionBehaviour : BaseEnemy
{
    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject explosionPrefab;

    [HideInInspector] public EnemyMinionBehaviour follower;
    [HideInInspector] public EnemyMinionBehaviour leader;

    protected override void GetDestroyed(){
        leader.follower = this.follower;
        follower.leader = this.leader;  // this should fix that
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
        InterfaceSingleton.Instance.AddScore(killPrice);
    }

    public void GetDashPos(float pos, bool side){
        dashPos = pos;
        shiftRight = side;
        if (follower != null){
            SendDashPos(pos, side);
        }
    }

    private void SendDashPos(float pos, bool side){
        dashPos = pos;
        shiftRight = side;
        follower.GetDashPos(dashPos, side);
    }

    private float dashPos = -9555f; // magic number
    private bool isShifting = false;
    private bool shiftRight = true;
    private float dashTimer = 0.35f;
    void FixedUpdate(){
        // looks a bit ugly
        if (dashPos < -20f){
            if (m_Rigidbody.position.z - GameManagerSingleton.Instance.player.transform.position.z < 2f){
                SendDashPos(m_Rigidbody.position.z, GameManagerSingleton.Instance.player.transform.position.x > m_Rigidbody.position.x);
            }
            m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * Vector3.back);
        }
        else{
            if (m_Rigidbody.position.z < dashPos && !isShifting){
                isShifting = true;
            }
            if (isShifting && dashTimer > 0f){
                m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * new Vector3(shiftRight ? 1f : -1f, 0f, 1.5f));
                dashTimer -= Time.fixedDeltaTime;
            }
            else{
                m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * Vector3.back);
            }
        }
        if (m_Rigidbody.position.z < -5){
            Destroy(gameObject);
        }
    }
}
