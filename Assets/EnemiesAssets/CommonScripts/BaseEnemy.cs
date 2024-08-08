using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]

public class BaseEnemy : MonoBehaviour, IDamageable
{
    [Header ("===== Base Components =====")]
    [SerializeField] protected Rigidbody m_Rigidbody;
    [SerializeField] protected Collider m_Collider;

    [Space (10)]
    [Header ("===== Base Variables =====")]
    [SerializeField] protected int health;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int killPrice;

    public void GetDamage(int dmg){
        int prevHP = health;
        health -= dmg;
        int score;
        if (health > 0){
            score = prevHP - health;
        }
        else{
            if (prevHP > 0){
                score = prevHP;
            }
            else{
                score = 0;
            }
        }
        InterfaceSingleton.Instance.AddScore(score * 10);
        Debug.Log(health);
        if (prevHP > 0 && health < 1){
            GetDestroyed();
        }
    }

    protected virtual void GetDestroyed(){  // Override when needed
        Destroy(gameObject);
        InterfaceSingleton.Instance.AddScore(killPrice);
    }
}
