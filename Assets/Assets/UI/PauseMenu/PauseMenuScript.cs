using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [Header ("===== Children =====")]
    [SerializeField] private CustomButton continueButton;
    [SerializeField] private CustomButton newGameButton;
    [SerializeField] private CustomButton exitButton;
    [SerializeField] private TextMeshProUGUI contCount;

    [Space (10)]
    [Header ("===== Prefabs =====")]
    [SerializeField] private GameObject confirmPanel;

    private Controls m_controls;

    private CustomButton[] buttonsArray;
    private int currentSelectedButton;

    private bool isInFocus;

    void Awake(){
        buttonsArray = new CustomButton[]{continueButton, newGameButton, exitButton};

        m_controls = new Controls();

        isInFocus = false;
    }

    void OnEnable(){
        m_controls.Enable();

        m_controls.General.Pause.performed += EscapePressed;
        m_controls.General.Navigation.performed += NavigationPressed;
        m_controls.General.Confirm.performed += ConfirmPressed;

        ConfirmPanelScript.confirmEvent += confirmAction;
    }

    void OnDisable(){
        m_controls.Disable();

        m_controls.General.Pause.performed -= EscapePressed;
        m_controls.General.Navigation.performed -= NavigationPressed;
        m_controls.General.Confirm.performed -= ConfirmPressed;

        ConfirmPanelScript.confirmEvent -= confirmAction;
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
                if (currentSelectedButton < 2){
                    currentSelectedButton += 1;
                }
            }
            buttonsArray[previousSelectedButton].DeselectButton();
            buttonsArray[currentSelectedButton].SelectButton();
        }
    }

    void EscapePressed(InputAction.CallbackContext ctx){
        if (isInFocus){
            GameManagerSingleton.Instance.FlipPauseState();
        }
    }

    void ConfirmPressed(InputAction.CallbackContext ctx){
        if (isInFocus){
            buttonsArray[currentSelectedButton].PressButton();
        }
    }
    
    public void AnimEnded(){
        // here be code
        foreach (CustomButton b in buttonsArray){
            b.SetPositions();
        }
        currentSelectedButton = 0;
        buttonsArray[currentSelectedButton].SelectButton();

        isInFocus = true;
    }

    private bool confirmActionFlag;
    private bool confirmActionResponse;
    void confirmAction(bool response){
        confirmActionFlag = true;
        confirmActionResponse = response;
    }

    // placeholders ahoy
    public void ContinuePressed(){
        GameManagerSingleton.Instance.FlipPauseState();
    }

    public void NewGamePressed(){
        isInFocus = false;
        confirmActionFlag = false;
        var panel = Instantiate(confirmPanel, transform);
        panel.GetComponent<ConfirmPanelScript>().Init("Start a new game?");
        StartCoroutine(NewGameCoroutine());
    }

    private IEnumerator NewGameCoroutine(){
        yield return new WaitUntil(() => confirmActionFlag);
        if (confirmActionResponse){
            Debug.Log("YesNewgame");
            GameManagerSingleton.Instance.FlipPauseState();
        }
        else{
            Debug.Log("NoNewgame");
            isInFocus = true;
        }
    }

    public void ExitPressed(){
        isInFocus = false;
        confirmActionFlag = false;
        var panel = Instantiate(confirmPanel, transform);
        panel.GetComponent<ConfirmPanelScript>().Init("Exit to main menu?");
        StartCoroutine(ExitCoroutine());
    }

    private IEnumerator ExitCoroutine(){
        yield return new WaitUntil(() => confirmActionFlag);
        if (confirmActionResponse){
            Debug.Log("YesExit");
            GameManagerSingleton.Instance.FlipPauseState();
        }
        else{
            Debug.Log("NoExit");
            isInFocus = true;
        }
    }
}