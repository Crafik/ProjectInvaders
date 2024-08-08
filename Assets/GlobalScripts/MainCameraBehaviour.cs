using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainCameraBehaviour : MonoBehaviour
{
    [Header ("===== Components =====")]
    [SerializeField] private CinemachineVirtualCamera m_camera;

    private CinemachineTrackedDolly m_dolly;
    private GameObject m_player;

    void Awake(){
        m_dolly = m_camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
        m_dolly.m_PathPosition = (m_player.transform.position.x + 12f) / 24f;
    }
}
