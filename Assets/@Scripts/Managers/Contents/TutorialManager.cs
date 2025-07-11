using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class TutorialStep
{
    public string Title;
    public string Description;
    public GameObject HighlightTarget;
    public string HighlightTargetKey;
    public UnityEvent OnStart;
    public UnityEvent OnComplete;
}
public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> Steps; 
    public UI_Tutorial UI;
    public Highlighter highlighter;
    public Image dimOverlay; // 어두운 배경용
    
    private int currentStep = 0;
    public bool IsRunning { get; private set; }
    private Dictionary<string, GameObject> _registeredTargets = new();

    public static TutorialManager Instance;

    void Awake() => Instance = this;

    private void Start()
    {
        UI = Managers.UI.ShowPopupUI<UI_Tutorial>();
        highlighter = UI.GetComponentInChildren<Highlighter>(true);
        dimOverlay = UI.transform.Find("DimOverlay")?.GetComponent<Image>(); // 찾는 방식 주의
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
        StartCoroutine(CoActivateStep(step));
    }

    private IEnumerator CoActivateStep(TutorialStep step)
    {
        step.OnStart?.Invoke();

        UI.Init();
        UI.Show(step.Title, step.Description);

        GameObject highlightTarget = step.HighlightTarget;

        // 등록될 때까지 대기
        if (highlightTarget == null && !string.IsNullOrEmpty(step.HighlightTargetKey))
        {
            yield return new WaitUntil(() => _registeredTargets.ContainsKey(step.HighlightTargetKey));
            highlightTarget = _registeredTargets[step.HighlightTargetKey];
        }

        if (highlightTarget != null)
        {
            highlighter.Follow(highlightTarget.GetComponent<RectTransform>());
            highlighter.gameObject.SetActive(true);
            dimOverlay?.gameObject.SetActive(true);
            dimOverlay?.transform.SetAsLastSibling();
            highlighter.transform.SetAsLastSibling();
        }
        else
        {
            highlighter.Hide();
            dimOverlay?.gameObject.SetActive(false);
        }
    }

    private void EndTutorial()
    {
        highlighter.Hide();
        dimOverlay?.gameObject.SetActive(false);
        IsRunning = false;
    }

    public bool IsStepActive(string title) =>
        IsRunning && Steps[currentStep].Title == title;
    
    public void RegisterTarget(string key, GameObject go)
    {
        if (string.IsNullOrEmpty(key) || go == null) return;

        if (_registeredTargets.ContainsKey(key))
        {
            if (_registeredTargets[key] == null)
            {
                _registeredTargets[key] = go; // 파괴된 오브젝트 대체
            }
            else
            {
                Debug.LogWarning($"[Tutorial] 이미 등록된 key: {key} / object: {_registeredTargets[key].name}");
            }
        }
        else
        {
            _registeredTargets[key] = go;
        }
    }

}
