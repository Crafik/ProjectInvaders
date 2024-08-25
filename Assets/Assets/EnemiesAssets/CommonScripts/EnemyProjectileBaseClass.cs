using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class EnemyProjectileBaseClass : MonoBehaviour
{
    [Header ("===== Base Components =====")]
    [SerializeField] protected Rigidbody m_Rigidbody;
    [SerializeField] protected Collider m_Collider;

    [Space (10)]
    [Header ("===== Base Variables =====")]
    [SerializeField] protected float moveSpeed;
}
