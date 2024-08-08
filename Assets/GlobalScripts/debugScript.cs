using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class debugScript : MonoBehaviour, IDamageable
{
    [Header ("===== Components =====")]
    public Rigidbody m_Rigidbody;

    [Space (10)]
    [Header ("===== STUFF =====")]
    public GameObject wingsPrefab;

    public float movespeed;

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Player")){
            collision.gameObject.GetComponent<PlayerController>().PowerUp();
        }
    }

    int HP = 100;
    // Need to save that to use on enemies
    public void GetDamage(int dmg){
        int prevHP = HP;
        HP -= dmg;
        int score;
        if (HP > 0){
            score = prevHP - HP;
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
        Debug.Log(HP);
        if (prevHP > 0 && HP < 1){
            Destroy(gameObject);
            InterfaceSingleton.Instance.AddScore(500);
        }
    }

    void OnTriggerEnter(Collider collision){
        // StarsParticlesSingleton.Instance.SetSpeed(5f);
        // Debug.Log("Slowing stars");
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerDeath();
    }
}
