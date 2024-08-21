using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPickUpBehaviour : MonoBehaviour, IPickable
{
    [Header ("===== Componenets =====")]
    [SerializeField] private Rigidbody m_Rigidbody;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float moveSpeed;

    private readonly PlayerPickUpType m_type = PlayerPickUpType.bomb;
    public PlayerPickUpType type { get { return m_type; }}

    public int GetPicked()
    {
        Destroy(gameObject);
        return 1;
    }

    void FixedUpdate()
    {
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * Quaternion.AngleAxis(15f, Vector3.forward));
        m_Rigidbody.MovePosition(m_Rigidbody.position - Time.fixedDeltaTime * moveSpeed * Vector3.forward);
    }
}
