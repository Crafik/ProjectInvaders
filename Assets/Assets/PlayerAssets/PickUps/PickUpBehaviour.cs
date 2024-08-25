using UnityEngine;

public class PickUpBehaviour : MonoBehaviour, IPickable
{
    [Header ("===== Componenets =====")]
    [SerializeField] private Rigidbody m_Rigidbody;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerPickUpType m_type;
    [SerializeField] private int value;

    public PlayerPickUpType type { get { return m_type; }}

    public int GetPicked()
    {
        Destroy(gameObject);
        return value;
    }

    void FixedUpdate()
    {
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * Quaternion.AngleAxis(15f, Vector3.forward));
        m_Rigidbody.MovePosition(m_Rigidbody.position - Time.fixedDeltaTime * moveSpeed * Vector3.forward);
    }
}