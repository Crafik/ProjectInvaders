using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [Header ("===== Children =====")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private TextMeshProUGUI contCount;

    void Awake(){
        continueButton.enabled = false;
        newGameButton.enabled = false;
        exitButton.enabled = false;
    }
    
    public void AnimEnded(){
        // here be code
        continueButton.enabled = true;
        newGameButton.enabled = true;
        exitButton.enabled = true;
    }
}
