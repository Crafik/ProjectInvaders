using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallExplosionBehaviour : MonoBehaviour
{
    // something bothers me with this explosion
    [SerializeField] private int damage;

    void Start(){
        var TriggerOverlap = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider entity in TriggerOverlap){
            if (entity.TryGetComponent<IDamageable>(out var damageable)){
                damageable.GetDamage(damage);
            }
        }

        Destroy(gameObject, 0.6f);
    }
}
