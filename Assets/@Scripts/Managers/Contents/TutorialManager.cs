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
    public UI_TutorialScene UI;
    private int currentStep = 0;
    public bool IsRunning { get; private set; }

    public static TutorialManager Instance;

    void Awake() => Instance = this;

    public void StartTutorial()
    {
        IsRunning = true;
        currentStep = 0;
        ActivateStep(Steps[currentStep]);
    }

    public void CompleteStep()
    {
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
        UI.Show(step.Title, step.Description);
    }

    private void EndTutorial()
    {
        IsRunning = false;
        //UI.Hide();
    }

    public bool IsStepActive(string title) =>
        IsRunning && Steps[currentStep].Title == title;

}
