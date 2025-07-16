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
        
        if (PlayerPrefs.GetInt("Tutorial_Completed", 0) == 1)
            return;
        
        UI = Managers.UI.ShowPopupUI<UI_Tutorial>();
        highlighter = UI.GetComponentInChildren<Highlighter>(true);
        dimOverlay = UI.transform.Find("DimOverlay")?.GetComponent<Image>(); // 찾는 방식 주의
        StartTutorial();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTutorialProgress();
            Debug.Log("튜토리얼 진행 상태 초기화됨");
        }
    }

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
            //FocusUI(highlightTarget);
            yield return new WaitForSeconds(0.1f);
            FocusOnlyThis(highlightTarget);
            highlighter.Follow(highlightTarget.GetComponent<RectTransform>());
            highlighter.gameObject.SetActive(true);
            dimOverlay?.gameObject.SetActive(true);
            UI.gameObject.GetComponent<Canvas>().sortingOrder = 100;
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
        UI?.gameObject.SetActive(false);
        IsRunning = false;
        
        StartCoroutine(EnableAllInteractablesAfterDelay());
        
        PlayerPrefs.SetInt("Tutorial_Completed", 1);
        PlayerPrefs.Save();
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
    public void ResetTutorialProgress()
    {
        PlayerPrefs.DeleteKey("Tutorial_Completed");
        PlayerPrefs.Save();
    }
    
    public void SkipTutorial()
    {
        foreach (var step in Steps)
        {
            step.OnComplete?.Invoke();
        }

        EndTutorial();
    }
    public void FocusUI(GameObject go)
    {
        if (go == null) return;

        Canvas targetCanvas = go.GetComponentInParent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.sortingOrder = 200; // 튜토리얼 UI보다 위로 올림
            Debug.Log($"[Tutorial] Canvas '{targetCanvas.name}' sortingOrder = 200");
        }
    }
    void SetAllUIInteractable(bool state)
    {
        foreach (var button in FindObjectsOfType<Button>())
        {
            button.interactable = state;
            button.image.raycastTarget = state;
            Debug.Log($"{button} interactable: {button.interactable}" );
            Debug.Log($"{button} raycastTarget:{button.image.raycastTarget}");
        }

        foreach (var toggle in FindObjectsOfType<Toggle>())
        {
            toggle.interactable = state;
        }
    }
    void FocusOnlyThis(GameObject target)
    {
        SetAllUIInteractable(false); // 전체 비활성화

        var btn = target.GetComponent<Button>();
        Debug.Log($"{btn} interactable : {btn.interactable}" );
        Debug.Log($"{btn} raycastTarget :{btn.image.raycastTarget}");
        if (btn != null)
        {
            btn.interactable = true;
            btn.image.raycastTarget = true;
        }
        Debug.Log($"{btn} : {btn.interactable}" );
        Debug.Log($"{btn} :{btn.image.raycastTarget}");

        var toggle = target.GetComponent<Toggle>();
        if (toggle != null)
            toggle.interactable = true;
    }
    
    
    private IEnumerator EnableAllInteractablesAfterDelay()
    {
        yield return new WaitForSeconds(0.1f); // UI가 SetActive(true) 된 이후로 대기
        SetAllUIInteractable(true);
    }
    
    void SetAllUIInteractableFromCanvas(Canvas canvas, bool state)
    {
        if (canvas == null) return;

        Button[] buttons = canvas.GetComponentsInChildren<Button>(true); // 비활성 포함
        foreach (var button in buttons)
        {
            button.interactable = state;
            if (button.image != null)
                button.image.raycastTarget = state;
        }

        Toggle[] toggles = canvas.GetComponentsInChildren<Toggle>(true);
        foreach (var toggle in toggles)
        {
            toggle.interactable = state;
        }
    }

}
