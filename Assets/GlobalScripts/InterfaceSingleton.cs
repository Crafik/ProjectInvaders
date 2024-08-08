using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceSingleton : MonoBehaviour
{
    public static InterfaceSingleton Instance { get; private set;}

    [Header ("===== Children =====")]
    [SerializeField] private TextMeshProUGUI currentScoreText;
    [SerializeField] private RectTransform playerIcon;
    [SerializeField] private RectTransform startPoint;
    [SerializeField] private RectTransform approachPoint;
    [SerializeField] private RectTransform finalDestinationPoint;

    private int displayedScore;
    private int currentScore;   // have a feeling that it may be better to store currentScore in manager rather than here

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
