using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialStep
{
    public string Title;
    public string Description;
    public GameObject HighlightTarget;
    public UnityEvent OnStart;
    public UnityEvent OnComplete;
}
public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> Steps; 
    public UI_Tutorial UI;
    public Highlighter highlighter;
    
    private int currentStep = 0;
    public bool IsRunning { get; private set; }

    public static TutorialManager Instance;

    void Awake() => Instance = this;

    private void Start()
    {
        UI = Managers.UI.ShowPopupUI<UI_Tutorial>();
        highlighter = UI.GetComponentInChildren<Highlighter>(true);
        StartTutorial();
    }

    public void StartTutorial()
    {
        IsRunning = true;
        currentStep = 0;
        ActivateStep(Steps[currentStep]);
    }

    public void CompleteStep()
    {
        Debug.Log($"{currentStep}Step Complete");
        Managers.Debug.Log($"{currentStep}Step Complete",Define.EDebugType.Building);
        Steps[currentStep].OnComplete?.Invoke();

        currentStep++;
        if (currentStep < Steps.Count)
        {
            ActivateStep(Steps[currentStep]);
        }
        else
        {
            EndTutorial();
        }
    }

    private void ActivateStep(TutorialStep step)
    {

        step.OnStart?.Invoke();
            
        // ⬇️ 설명 텍스트 보여주기
        UI.Init(); 
        UI.Show(step.Title, step.Description); 
        
        if (step.Title == "건설 버튼 누르기" && step.HighlightTarget == null)
        {
            GameObject buildBtn = GameObject.Find("BuildButton");
            if (buildBtn != null)
                step.HighlightTarget = buildBtn;
        }

        if (step.HighlightTarget != null && highlighter != null)
        {
            highlighter.Follow(step.HighlightTarget.GetComponent<RectTransform>());
        }
        else
        {
            highlighter.Hide();
        }
    }

    private void EndTutorial()
    {
        IsRunning = false;
        //UI.Hide();
    }

    public bool IsStepActive(string title) =>
        IsRunning && Steps[currentStep].Title == title;

}
