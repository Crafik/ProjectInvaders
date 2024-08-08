using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMediumBulletBehaviour : EnemyProjectileBaseClass
{
    [Space (10)]
    [Header ("===== Components =====")]
    [SerializeField] private GameObject bulletCore;

    public void Init(Quaternion dir){
        m_Rigidbody.rotation = dir;
    }

    void Update(){
        bulletCore.transform.Rotate(bulletCore.transform.rotation.eulerAngles + 10f * Vector3.up);
    }

    void OnTriggerEnter(Collider collision){
        if (collision.CompareTag("Player")){
            collision.GetComponent<PlayerController>().PlayerDeath();
        }
        Destroy(gameObject);
    }

    void FixedUpdate(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * transform.forward);

        if (m_Rigidbody.position.x > 15f || m_Rigidbody.position.x < -15f ||
            m_Rigidbody.position.z > 12f || m_Rigidbody.position.z < -5f){
            Destroy(gameObject);
        }
    }
}
