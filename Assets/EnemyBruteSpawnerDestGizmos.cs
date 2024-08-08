using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBruteSpawnerDestGizmos : MonoBehaviour
{
    [SerializeField] private BruteSpawnerBehaviour parentScript;
    void OnDrawGizmosSelected(){
        parentScript.ChildDrawGizmos();
    }
}
