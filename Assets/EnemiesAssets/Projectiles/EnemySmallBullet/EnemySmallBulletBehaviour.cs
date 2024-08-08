using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySmallBulletBehaviour : EnemyProjectileBaseClass
{
    [Header ("===== Components =====")]
    [SerializeField] private GameObject bulletMesh;

    Vector3 targetDir;
    public void Init(Vector3 targetPos){
        targetDir = (targetPos - m_Rigidbody.position).normalized;
    }

    void OnTriggerEnter(Collider collision){
        if (collision.CompareTag("Player")){
            collision.GetComponent<PlayerController>().PlayerDeath();
        }
        Destroy(gameObject);
    }

    void Update(){
        bulletMesh.transform.Rotate(bulletMesh.transform.rotation.eulerAngles + 10f * Vector3.up);
    }

    void FixedUpdate(){
        m_Rigidbody.MovePosition(m_Rigidbody.position + moveSpeed * Time.fixedDeltaTime * targetDir);

        if (m_Rigidbody.position.x > 15f || m_Rigidbody.position.x < -15f ||
            m_Rigidbody.position.z > 12f || m_Rigidbody.position.z < -5f){
            Destroy(gameObject);
        }
    }

}
