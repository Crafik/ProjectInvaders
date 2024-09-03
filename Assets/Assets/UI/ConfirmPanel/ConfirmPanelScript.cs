using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ConfirmPanelScript : MonoBehaviour
{
    public static event Action<bool> confirmEvent;

    [Header ("===== Children =====")]
    [SerializeField] private CustomButton noButton;
    [SerializeField] private CustomButton yesButton;
    [SerializeField] private TextMeshProUGUI label;

    private bool noButtonSelected;

    private Controls m_controls;

    private bool isInFocus;

    void Awake(){
        m_controls = new Controls();

        isInFocus = false;
    }

    public void Init(string v_text){
        label.text = v_text;
    }

    void OnEnable(){
        m_controls.Enable();

        m_controls.General.Pause.performed += EscapePressed;
        m_controls.General.Navigation.performed += NavigationPressed;
        m_controls.General.Confirm.performed += ConfirmPressed;
    }

    void OnDisable(){
        m_controls.Disable();

        m_controls.General.Pause.performed -= EscapePressed;
        m_controls.General.Navigation.performed -= NavigationPressed;
        m_controls.General.Confirm.performed -= ConfirmPressed;
    }

    void NavigationPressed(InputAction.CallbackContext ctx){
        if (noButtonSelected){
            yesButton.SelectButton();
            noButton.DeselectButton();
            noButtonSelected = false;
        }
        else{
            yesButton.DeselectButton();
            noButton.SelectButton();
            noButtonSelected = true;
        }
    }

    void EscapePressed(InputAction.CallbackContext ctx){
        noButtonPressed();
    }

    void ConfirmPressed(InputAction.CallbackContext ctx){
        (noButtonSelected ? noButton : yesButton).PressButton();
    }

    public void AnimEnded(){
        noButton.SetPositions();
        yesButton.SetPositions();

        noButtonSelected = true;
        noButton.SelectButton();

        isInFocus = true;
    }

    public void noButtonPressed(){
        confirmEvent?.Invoke(false);
        Destroy(gameObject);
    }

    public void yesButtonPressed(){
        confirmEvent?.Invoke(true);
        Destroy(gameObject);
    }
}
