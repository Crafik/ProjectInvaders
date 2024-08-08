using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerSingleton : MonoBehaviour
{
    public static GameManagerSingleton Instance { get; private set; }

    [HideInInspector] public GameObject player;
    [HideInInspector] public bool isPlayerAlive;

    private Controls m_controls;

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(Instance);
        }
        else{
            Instance = this;
        }

        isPlayerAlive = true;

        m_controls = new Controls();
    }

    void OnEnable(){
        m_controls.Enable();

        m_controls.General.Pause.performed += PauseGame;
    }

    void OnDisable(){
        m_controls.General.Pause.performed -= PauseGame;
    }

    void PauseGame(InputAction.CallbackContext ctx){
        // There is like a whole lot to fix about this
        // timescale it is i guess
    }
}
