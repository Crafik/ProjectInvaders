using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMissileBehaviour : PlayerBulletBehaviour
{
    [SerializeField] private float initialSpeed;
    [SerializeField] private float accelerationRate;

    [Space (10)]
    [SerializeField] private ParticleSystem m_particles;

    [Space (10)]
    [SerializeField] private GameObject explosionPrefab;

    private float currentSpeed;

    void Awake(){
        currentSpeed = initialSpeed;
    }

    void DestroyMissile(){
        // works
        m_particles.transform.parent = null;
        m_particles.Stop();
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider collision){
        // here be explosion
        if (collision.CompareTag("Enemy")){
            collision.GetComponent<IDamageable>().GetDamage(damage);
        }
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        DestroyMissile();
    }

    void FixedUpdate(){
        m_body.MovePosition(m_body.position + currentSpeed * Time.fixedDeltaTime * new Vector3(0f, 0f, 1f));
        m_body.MoveRotation(m_body.rotation * Quaternion.Euler(new Vector3(0f, 0f, 18f)));
        if (m_body.position.z > 12f){
            DestroyMissile();
        }
        if (currentSpeed < moveSpeed){
            currentSpeed += accelerationRate * Time.fixedDeltaTime;
        }
    }
}
