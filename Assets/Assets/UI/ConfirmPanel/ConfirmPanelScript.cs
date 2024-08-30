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
    [SerializeField] private Button noButton;
    [SerializeField] private Button yesButton;
    [SerializeField] private TextMeshProUGUI label;

    private bool noButtonSelected;

    private Controls m_controls;

    void Awake(){
        m_controls = new Controls();

        noButtonSelected = true;
        noButton.Select();
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
            yesButton.Select();
            noButtonSelected = false;
        }
        else{
            noButton.Select();
            noButtonSelected = true;
        }
    }

    void EscapePressed(InputAction.CallbackContext ctx){
        noButtonPressed();
    }

    void ConfirmPressed(InputAction.CallbackContext ctx){
        (noButtonSelected ? noButton : yesButton).onClick.Invoke();
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
