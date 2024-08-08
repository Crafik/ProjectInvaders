using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    [Header ("===== Components =====")]
    [SerializeField] protected Rigidbody m_body;

    [Space (10)]
    [Header ("===== Variables =====")]
    public float moveSpeed;
    public int damage;

    void OnTriggerEnter(Collider collision){
        if (collision.CompareTag("Enemy")){
            collision.GetComponent<IDamageable>().GetDamage(damage);
        }
        Destroy(gameObject);
    }

    void FixedUpdate(){
        if (GameManagerSingleton.Instance.isGameActive){
            m_body.MovePosition(m_body.position + new Vector3(0f, 0f, 1f) * moveSpeed * Time.fixedDeltaTime);
            if (m_body.position.z > 12f){
                Destroy(gameObject);
            }
        }
    }
}
