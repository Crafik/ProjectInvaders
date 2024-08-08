using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        // Drawing level boundaries
        Gizmos.DrawLine(new Vector3(-9.5f, 0f, -3.5f), new Vector3(-9.5f, 0f, 8f)); // Left bound
        Gizmos.DrawLine(new Vector3(9.5f, 0f, -3.5f), new Vector3(9.5f, 0f, 8f)); // Right bound
        Gizmos.DrawLine(new Vector3(-9.5f, 0f, 8f), new Vector3(9.5f, 0f, 8f)); // Top bound
        Gizmos.DrawLine(new Vector3(-9.5f, 0f, -3.5f), new Vector3(9.5f, 0f, -3.5f)); // Bottom bound
        
        Gizmos.color = Color.red;
        // Drawing despawn boundaries
        Gizmos.DrawLine(new Vector3(-17f, 0f, 15f), new Vector3(-17f, 0f, -7f)); // Left bound
        Gizmos.DrawLine(new Vector3(17f, 0f, 15f), new Vector3(17f, 0f, -7f)); // Right bound
        Gizmos.DrawLine(new Vector3(17f, 0f, 15f), new Vector3(-17f, 0f, 15f)); // Top bound
        Gizmos.DrawLine(new Vector3(17f, 0f, -7f), new Vector3(-17f, 0f, -7f)); // Bottom bound
    }

    private float timer;
    private int listIterator;
    private List<ISpawnable> spawners;

    void Awake(){
        spawners = new List<ISpawnable>();
        foreach (Transform child in transform){
            spawners.Add(child.gameObject.GetComponent<ISpawnable>());
        }
        spawners.Sort(SpawnableComparison);
        timer = 0f;
        listIterator = 0;
    }

    private int SpawnableComparison(ISpawnable x, ISpawnable y){
        if (x.spawnTime == y.spawnTime){
            return 0;
        }
        else{
            return x.spawnTime > y.spawnTime ? 1 : -1;
        }
    }

    void Start(){
        InterfaceSingleton.Instance.SetDestinationPos(true);
        InterfaceSingleton.Instance.UpdateProgress(0f);
    }

    void Update(){
        if (listIterator < spawners.Count){
            if (timer > spawners[listIterator].spawnTime){
                while (listIterator < spawners.Count && timer > spawners[listIterator].spawnTime){
                    spawners[listIterator].SpawnEntity();
                    listIterator += 1;
                }
            }
        }
        
        timer += Time.deltaTime;
        InterfaceSingleton.Instance.UpdateProgress(timer / 15f);
    }
}
