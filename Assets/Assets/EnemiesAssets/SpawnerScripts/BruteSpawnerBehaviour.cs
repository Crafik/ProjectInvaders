using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteSpawnerBehaviour : MonoBehaviour, ISpawnable
{
    [Header ("--= Prefab =--")]
    [SerializeField] private GameObject prefab;

    [Space (5)]
    [Header ("--= Variables =--")]
    [SerializeField] private float timeToSpawn;

    [Space (5)]
    [Header ("--= Children =--")]
    [SerializeField] private GameObject destinationPoint;

    public float spawnTime { get { return timeToSpawn; } }

    public void SpawnEntity(){
        var bruteinst = Instantiate(prefab, transform.position, Quaternion.identity);
        bruteinst.GetComponent<EnemyBruteBehaviour>().Init(destinationPoint.transform.position);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        
        Gizmos.DrawSphere(transform.position, 0.35f);
        
        Gizmos.DrawLine(new Vector3(-8f, 0f, 6f), new Vector3(-8f, 0f, 1f)); // Left bound
        Gizmos.DrawLine(new Vector3(8f, 0f, 6f), new Vector3(8f, 0f, 1f)); // Right bound
        Gizmos.DrawLine(new Vector3(-8f, 0f, 6f), new Vector3(8f, 0f, 6f)); // Top bound
        Gizmos.DrawLine(new Vector3(-8f, 0f, 1f), new Vector3(8f, 0f, 1f)); // Bottom bound

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(destinationPoint.transform.position, 0.35f);
    }

    public void ChildDrawGizmos(){
        OnDrawGizmosSelected();
    }
}
