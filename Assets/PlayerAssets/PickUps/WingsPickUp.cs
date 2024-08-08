using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class WingsPickUp : MonoBehaviour, IPickable
{
    [Header ("===== Components =====")]
    [SerializeField] private Rigidbody m_Rigidbody;

    [Space (10)]
    [Header ("===== Variables =====")]
    [SerializeField] private int powerLevel;
    [SerializeField] private float moveSpeed;

    private readonly int m_type = 0;

    public int type { get { return m_type; } }

    public int GetPicked(){
        Destroy(gameObject);
        return powerLevel;
    }

    void Start(){
        velocity = new Vector3(0f, 0f, moveSpeed);
    }

    Vector3 velocity;
    void FixedUpdate(){
        if (GameManagerSingleton.Instance.isGameActive){
            m_Rigidbody.MovePosition(m_Rigidbody.position + velocity * Time.fixedDeltaTime);
            if (velocity.z > -moveSpeed){
                velocity -= moveSpeed * Time.fixedDeltaTime * Vector3.forward;  // feels weird, probably need some tweaking, but works at the time
            }

            if (m_Rigidbody.position.z < -5.5f){
                Destroy(gameObject);
            }
        }
    }
}
