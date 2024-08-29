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
    [HideInInspector] public bool isGameActive;
    [HideInInspector] public bool isGamePaused;

    private Controls m_controls;

    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(Instance);
        }
        else{
            Instance = this;
        }

        isPlayerAlive = true;
        isGamePaused = false;
        isGameActive = true;

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
        if (isGamePaused){
            if (isGameActive){ // ??? hmmm
                InterfaceSingleton.Instance.DestroyMenu();
                Time.timeScale = 1f;
                isGamePaused = false;
            }
        }
        else{
            InterfaceSingleton.Instance.CallMenu();
            Time.timeScale = 0f;
            isGamePaused = true;
        }
    }

    public void FlipPauseState(){
        PauseGame(new InputAction.CallbackContext());
    }
}
