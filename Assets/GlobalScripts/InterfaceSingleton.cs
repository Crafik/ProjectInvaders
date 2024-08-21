using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceSingleton : MonoBehaviour
{
    public static InterfaceSingleton Instance { get; private set;}

    [Header ("===== Children =====")]
    [Header (" -= Left Column =- ")]
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform approachPoint;
    [SerializeField] private RectTransform finalDestinationPoint;

    [Space (3)]
    [Header (" -= Right Column =- ")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [Header ("Lives counter")]
    [SerializeField] private GameObject livesCounter;
    [SerializeField] private TextMeshProUGUI livesOverflowCounter;
    [Header ("Bombs counter")]
    [SerializeField] private GameObject bombsCounter;
    [Header ("Power counter")]
    [SerializeField] private TextMeshProUGUI powerLevelCounter;
    [SerializeField] private TextMeshProUGUI maxPowerPlug;

    [Space (3)]
    [SerializeField] private Image bombFadeIn;

    private int displayedScore;
    public int currentScore { get; private set; }   // have a feeling that it may be better to store currentScore in manager rather than here

    private Coroutine countingCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(Instance);
        }
        else{
            Instance = this;
        }
    }


#region Left column business
    private Vector3 destinationPos;

    /// <summary>
    /// Sets destination point for player icon on HUD
    /// </summary>
    /// <param name="approach">Sets destination point to approach if true.<br />Sets to destination otherwise.</param>
    public void SetDestinationPos(bool approach){
        destinationPos = approach ? approachPoint.position : finalDestinationPoint.position;
    }

    public void UpdateProgress(float progress){
        playerIcon.position = Vector3.Lerp(startPoint.position, destinationPos, progress);
    }
#endregion

#region Right column business
#region Score business

    public void SetScore(int score){
        currentScore = score;

        if (countingCoroutine != null){
            StopCoroutine(countingCoroutine);
        }
        
        countingCoroutine = StartCoroutine(CountScore());
    }

    public void AddScore(int score){
        score += currentScore;
        SetScore(score);
    }

    private IEnumerator CountScore(){
        // Appropriated code
        // Looks clunky, may refactor later. Works though

        // yield return null; looks better and count faster, so keeping it at that.

        if (displayedScore < currentScore)
        {
            while(displayedScore < currentScore)
            {
                // this is atrocious
                int stepAmount = 10;
                if (currentScore - displayedScore > 5000){
                    stepAmount = 100;
                }
                if (currentScore - displayedScore > 50000){
                    stepAmount = 1000;
                }
                if (currentScore - displayedScore > 500000){
                    stepAmount = 10000;
                }
                displayedScore += stepAmount;
                if (displayedScore > currentScore)
                {
                    displayedScore = currentScore;
                }

                // this is horrible. i don't like it. But works
                // Letter "D" looks almost(?) undistinguishable from zero. Dunno if this needs a fix
                int topDigit = displayedScore / 10000000;
                currentScoreText.text = topDigit > 9 ? Convert.ToChar(topDigit - 10 + 65).ToString() : topDigit.ToString();
                currentScoreText.text += (displayedScore % 10000000).ToString("D7");

                yield return null;
            }
        }
        else
        {
            while (displayedScore > currentScore)
            {
                int stepAmount = 10;
                if (currentScore - displayedScore > 5000){
                    stepAmount = 100;
                }
                if (currentScore - displayedScore > 50000){
                    stepAmount = 1000;
                }
                if (currentScore - displayedScore > 500000){
                    stepAmount = 10000;
                }
                displayedScore += stepAmount;
                if (displayedScore < currentScore)
                {
                    displayedScore = currentScore;
                }

                int topDigit = displayedScore / 10000000;
                currentScoreText.text = topDigit > 9 ? Convert.ToChar(topDigit - 10 + 65).ToString() : topDigit.ToString();
                currentScoreText.text += (displayedScore % 10000000).ToString("D7");

                yield return null;
            }
        }
    }

#endregion

    public void SetLivesCounter(int lives){
        // ¯\_(ツ)_/¯
        for (int i = 0; i < 5; ++i){
            livesCounter.transform.GetChild(i).gameObject.SetActive(false);
        }
        livesOverflowCounter.text = "";
        if (lives < 6){
            for (int i = 0; i < lives; ++i){
                livesCounter.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else{
            for (int i = 0; i < 3; ++i){
                livesCounter.transform.GetChild(i).gameObject.SetActive(true);
            }
            livesOverflowCounter.text = "+" + (lives - 3).ToString();
        }
    }

    public void SetPowerCounter(int level){
        powerLevelCounter.text = "";
        maxPowerPlug.text = "";
        if (level < 6){
            for (int i = 0; i < level; ++i){
                powerLevelCounter.text += "i";
            }
        }
        else{
            maxPowerPlug.text = "max";
        }
    }

    public void SetBombsCounter(int count){
        if (count > -1 && count < 6){
            for (int i = 0; i < 5; ++i){
                bombsCounter.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < count; i++){
                bombsCounter.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

#endregion

    public void SetBombFadeIn(float value){
        bombFadeIn.color = new Color(1f, 1f, 1f, value);
    }

    public void ResetBombFadeIn(){
        SetBombFadeIn(0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
