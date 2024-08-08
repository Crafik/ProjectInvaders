using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnerBehaviour : MonoBehaviour, ISpawnable
{
    [Header ("--= Prefab =--")]
    [SerializeField] private GameObject prefab;

    [Space (5)]
    [Header ("--= Variables =--")]
    [SerializeField] private float timeToSpawn;
    [SerializeField] private int quantity;

    public float spawnTime { get { return timeToSpawn; } }

    public void SpawnEntity(){
        Vector3 spawnPos = transform.position;
        var firstMinion = Instantiate(prefab, spawnPos, Quaternion.identity);
        for (int i = 1; i < quantity; ++i){
            spawnPos += Vector3.forward * 1.5f;
            var secondMinion = Instantiate(prefab, spawnPos, Quaternion.identity);
            firstMinion.GetComponent<EnemyMinionBehaviour>().follower = secondMinion.GetComponent<EnemyMinionBehaviour>();
            secondMinion.GetComponent<EnemyMinionBehaviour>().leader = firstMinion.GetComponent<EnemyMinionBehaviour>();
            firstMinion = secondMinion;
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, 0.35f);

        Vector3 shiftedLeft = transform.position - new Vector3(2.1f, 0f, 0.5f);
        Vector3 shiftedRight = transform.position - new Vector3(-2.1f, 0f, 0.5f);
        
        Gizmos.DrawLine(transform.position, transform.position - Vector3.forward * 20.5f);
        Gizmos.DrawLine(transform.position, shiftedLeft);
        Gizmos.DrawLine(transform.position, shiftedRight);
        Gizmos.DrawLine(shiftedLeft, shiftedLeft - Vector3.forward * 20f);
        Gizmos.DrawLine(shiftedRight, shiftedRight - Vector3.forward * 20f);
    }
}
