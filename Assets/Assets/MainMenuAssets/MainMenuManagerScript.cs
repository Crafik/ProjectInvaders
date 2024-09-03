using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManagerScript : MonoBehaviour
{
    [Header ("===== References =====")]
    [SerializeField] private ParticleSystem m_particles;
    [SerializeField] private GameObject moon;

    [Header ("-= Buttons =-")]
    [SerializeField] private CustomButton newGameButton;
    [SerializeField] private CustomButton optionsButton;
    [SerializeField] private CustomButton leaderboardButton;
    [SerializeField] private CustomButton musicButton;
    [SerializeField] private CustomButton exitButton;


    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject confirmPanelPrefab;
    // here be prefabs for options/leaderboard menus

    private Controls m_controls;
    private bool isInFocus;
    private int currentSelectedButton;
    private CustomButton[] buttonsArray;

    void Awake(){
        m_controls = new Controls();
        isInFocus = true;
    }

    void OnEnable(){
        m_controls.Enable();

        m_controls.General.Navigation.performed += NavigationPressed;

        ConfirmPanelScript.confirmEvent += confirmAction;
    }

    void OnDisable(){
        m_controls.Disable();

        m_controls.General.Navigation.performed -= NavigationPressed;

        ConfirmPanelScript.confirmEvent -= confirmAction;
    }

    void Start(){
        buttonsArray = new CustomButton[]{ newGameButton, optionsButton, leaderboardButton, musicButton, exitButton };
        currentSelectedButton = 0;
        buttonsArray[currentSelectedButton].SelectButton();
    }

    void NavigationPressed(InputAction.CallbackContext ctx){
        if (isInFocus){
            Vector2 val = ctx.ReadValue<Vector2>();
            int previousSelectedButton = currentSelectedButton;
            if (val.y == 0){
                return;
            }
            if (val.y > 0){
                if (currentSelectedButton > 0){
                    currentSelectedButton -= 1;
                }
            }
            else{
                if (currentSelectedButton < 4){
                    currentSelectedButton += 1;
                }
            }
            buttonsArray[previousSelectedButton].DeselectButton();
            buttonsArray[currentSelectedButton].SelectButton();
        }
    }

    private bool confirmActionFlag;
    private bool confirmActionResponse;
    void confirmAction(bool response){
        confirmActionFlag = true;
        confirmActionResponse = response;
    }

    void Update(){
        moon.transform.RotateAround(moon.transform.position, moon.transform.up, 15f * Time.deltaTime);
    }
}
