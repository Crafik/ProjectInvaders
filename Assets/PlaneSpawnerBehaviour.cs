using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneSpawnerBehaviour : MonoBehaviour, ISpawnable
{
    [Header ("--= Prefab =--")]
    [SerializeField] private GameObject prefab;

    [Space (5)]
    [Header ("--= Variables =--")]
    [SerializeField] private float timeToSpawn;
    [SerializeField] private EnemyPlaneMode mode;

    public float spawnTime { get { return timeToSpawn; } }

    public void SpawnEntity(){
        var enemyPlane = Instantiate(prefab, transform.position, transform.rotation);
        enemyPlane.GetComponent<EnemyPlaneBehaviour>().Init(mode);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;

        Gizmos.DrawSphere(transform.position, 0.35f);

        if (mode == EnemyPlaneMode.Turn){
            bool onRightSide = transform.position.x > 0;
            Vector3 secondPoint = new Vector3(onRightSide ? -7.5f : 7.5f, 0f, transform.position.z);
            Gizmos.DrawLine(transform.position, secondPoint);
            Vector3 horizontalShift = Vector3.forward * (onRightSide ? -2f : 2f);
            Gizmos.DrawLine(secondPoint, secondPoint + horizontalShift);
            Gizmos.DrawLine(transform.position + horizontalShift, secondPoint + horizontalShift);
        }
        else{
            Gizmos.DrawRay(transform.position, transform.forward * 40f);
        }
    }
}
