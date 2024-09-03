using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private bool isOnRightSide;

    private Button m_button;

    private RectTransform rTransform;
    private float startPos;
    private float selectedPos;

    private bool isTransitioning;
    private float transitionState;

    private Coroutine currentCoroutine;

    void Start(){
        m_button = GetComponent<Button>();

        rTransform = GetComponent<RectTransform>();
        SetPositions();

        isTransitioning = false;
        transitionState = 0f;
    }

    public void SetPositions(){ // A little "fix" for some buttons
        startPos = rTransform.position.x;
        selectedPos = startPos + (isOnRightSide ? -30 : 30);
        Debug.Log(startPos + " -=- " + selectedPos);
    }

    public void SelectButton(){
        if (isTransitioning){
            StopCoroutine(currentCoroutine);
        }
        isTransitioning = true;
        currentCoroutine = StartCoroutine(SelectCoroutine());
    }

    private IEnumerator SelectCoroutine(){
        while (transitionState < 1f){
            rTransform.position = Vector3.Lerp(
                new Vector3(startPos, rTransform.position.y, rTransform.position.z),
                new Vector3(selectedPos, rTransform.position.y, rTransform.position.z),
                transitionState
            );
            transitionState += Time.deltaTime * 10f;
            yield return null;
        }
        transitionState = 1f;
        isTransitioning = false;
    }

    public void DeselectButton(){
        if (isTransitioning){
            StopCoroutine(currentCoroutine);
        }
        isTransitioning = true;
        currentCoroutine = StartCoroutine(DeselectCoroutine());
    }

    private IEnumerator DeselectCoroutine(){
        while (transitionState > 0f){
            rTransform.position = Vector3.Lerp(
                new Vector3(startPos, rTransform.position.y, rTransform.position.z),
                new Vector3(selectedPos, rTransform.position.y, rTransform.position.z),
                transitionState
            );
            transitionState -= Time.deltaTime * 10f;
            yield return null;
        }
        transitionState = 0f;
        isTransitioning = false;
    }

    public void PressButton(){
        m_button.onClick.Invoke();
    }
}