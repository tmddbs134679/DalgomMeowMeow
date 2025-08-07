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
    private GameObject _raycastBlocker;

    void Awake() => Instance = this;

    private IEnumerator Start()
    {
        if (PlayerPrefs.GetInt("Tutorial_Completed", 0) == 1)
            yield break;

        UI = Managers.UI.ShowPopupUI<UI_Tutorial>();
        
        
        var gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
       
        StartCoroutine( GameSceneUIInteractive(gameSceneUI, false));
        


        
        yield return new WaitForSeconds(0.1f); // 1프레임 대기하여 UI 초기화 보장
        
        highlighter = UI.GetComponentInChildren<Highlighter>(true);
        dimOverlay = UI.transform.Find("DimOverlay")?.GetComponent<Image>();
    
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
            SetAllUIInteractable(true); // 전체 활성화
            highlighter.Hide();
            dimOverlay?.gameObject.SetActive(false);
        }
        if (currentStep == 0 && highlightTarget == null && string.IsNullOrEmpty(step.HighlightTargetKey))
        {
            // 첫 설명-only 단계라면 2초 뒤 자동으로 다음 단계로 진행
            yield return new WaitForSeconds(2.0f);
            CompleteStep();
            yield break;
        }
    }

    private void EndTutorial()
    {
        highlighter.Hide();
        dimOverlay?.gameObject.SetActive(false);
        UI?.gameObject.SetActive(false);
        IsRunning = false;
        
        StartCoroutine(EnableAllInteractablesAfterDelay());
        
        Managers.UI.ShowToast("챕터를 완료하고 다양한 컨텐츠를 즐겨보세요");
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
               // Debug.LogWarning($"[Tutorial] 이미 등록된 key: {key} / object: {_registeredTargets[key].name}");
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
        // EndTutorial();
        foreach (var step in Steps)
            step.OnComplete?.Invoke();

        IsRunning = false;

        // 팝업 닫기
        var uiRoad = Managers.UI.GetPopupUI<UI_Road>();
        if (uiRoad != null)
            Managers.UI.ClosePopupUI(uiRoad);

        var uiBuild = Managers.UI.GetPopupUI<UI_BuildPopup>();
        if (uiBuild != null)
            Managers.UI.ClosePopupUI(uiBuild);
        
        var uiFarm = Managers.UI.GetPopupUI<UI_FarmPopup>();
        if (uiFarm != null)
            Managers.UI.ClosePopupUI(uiFarm);
        
        var uiBuildAction = Managers.UI.GetPopupUI<UI_BuildAction>();
        if (uiBuildAction != null)
            Managers.UI.ClosePopupUI(uiBuildAction);

        // GameScene UI 켜기
        var gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        if (gameSceneUI != null)
        {
            gameSceneUI.gameObject.SetActive(true);
            StartCoroutine(GameSceneUIInteractive(gameSceneUI,true));
        }

        Managers.UI.ShowToast("튜토리얼 완료");
        PlayerPrefs.SetInt("Tutorial_Completed", 1);
        PlayerPrefs.Save();
    }
    public void FocusUI(GameObject go)
    {
        if (go == null) return;

        Canvas targetCanvas = go.GetComponentInParent<Canvas>();
        if (targetCanvas != null)
        {
            targetCanvas.sortingOrder = 200; // 튜토리얼 UI보다 위로 올림
        }
    }
    public void SetAllUIInteractable(bool state)
    {
        var gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        if (gameSceneUI != null)
        {
            var buttons = gameSceneUI.GetComponentsInChildren<Button>(true);
            var toggles = gameSceneUI.GetComponentsInChildren<Toggle>(true);


            foreach (var button in buttons)
            {
                button.interactable = state;
                if (button.image != null)
                    button.image.raycastTarget = state;
            }

            foreach (var toggle in toggles)
            {
                toggle.interactable = state;
            }
        }
        var buildActionUI = Managers.UI.GetPopupUI<UI_BuildAction>();
        if (buildActionUI != null)
        {
            var buttons = buildActionUI.GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                button.interactable = true;
                if (button.image != null)
                    button.image.raycastTarget = true;
            }

        }
        var buildUI = Managers.UI.GetPopupUI<UI_BuildPopup>();
        if (buildUI != null)
        {
            var buttons = buildUI.GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                button.interactable = state;
                if (button.image != null)
                    button.image.raycastTarget = state;
            }
        }
        
        var farmUI = Managers.UI.GetPopupUI<UI_FarmPopup>();
        if (farmUI != null)
        {
            var buttons = farmUI.GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                button.interactable = state;
                if (button.image != null)
                    button.image.raycastTarget = state;
            }
        }

        var roadUI = Managers.UI.GetPopupUI<UI_Road>();
        if (roadUI != null)
        {
            var buttons = roadUI.GetComponentsInChildren<Button>(true);
            foreach (var button in buttons)
            {
                button.interactable = state;
                if (button.image != null)
                    button.image.raycastTarget = state;
            }
        }
            
            
        foreach (var button in FindObjectsOfType<Button>(true))
        {
            button.interactable = state;
            if (button.image != null)
                button.image.raycastTarget = state;
        }
        
        foreach (var toggle in FindObjectsOfType<Toggle>(true))
        {
            toggle.interactable = state;
        }
    }

    public void SetAcitiveUIInteractable()
    {
        StartCoroutine(CoSetAllUIInteractable(true));
    }
    public IEnumerator CoSetAllUIInteractable(bool state)
    {
        yield return new WaitUntil(() =>
        {
            var canvas = GameObject.Find("Canvas");
            if (canvas == null || !canvas.activeInHierarchy)
                return false;

            var buttons = canvas.GetComponentsInChildren<Button>(true);
            var toggles = canvas.GetComponentsInChildren<Toggle>(true);
            return buttons.Length > 0 || toggles.Length > 0;
        });

        yield return null; // 1프레임 대기

        var foundButtons = GameObject.FindObjectsOfType<Button>(true);
        foreach (var button in foundButtons)
        {
            button.enabled = state;
            if (button.image != null)
                button.image.raycastTarget = state;
        }

        var foundToggles = GameObject.FindObjectsOfType<Toggle>(true);
        foreach (var toggle in foundToggles)
        {
            toggle.interactable = state;
        }

    }
    void FocusOnlyThis(GameObject target)
    {
        if (target == null) return;
        SetAllUIInteractable(false); // 전체 비활성화
        
        // var gameSceneUI = Managers.UI.SceneUI as UI_GameScene;
        // if (gameSceneUI != null)
        // {
        //     var buttons = gameSceneUI.GetComponentsInChildren<Button>(true);
        //     var toggles = gameSceneUI.GetComponentsInChildren<Toggle>(true);
        //
        //
        //     foreach (var button in buttons)
        //     {
        //         button.interactable = false;
        //         if (button.image != null)
        //             button.image.raycastTarget = false;
        //     }
        // }
        

        var btn = target.GetComponent<Button>();
        if (btn != null)
        {
            btn.interactable = true;
            btn.image.raycastTarget = true;
        }

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
    public IEnumerator GameSceneUIInteractive(UI_GameScene gameSceneUI,bool state)
    {
        // wait until ready
        yield return new WaitUntil(() => gameSceneUI.gameObject.activeInHierarchy);
        yield return new WaitForSeconds(0.1f);

        // canvas group unlock
        var cg = gameSceneUI.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.interactable = state;
            cg.blocksRaycasts = state;
        }

        var buttons = gameSceneUI.GetComponentsInChildren<Button>(true);
        var toggles = gameSceneUI.GetComponentsInChildren<Toggle>(true);


        foreach (var button in buttons)
        {
            if (button.name != "RoadButton")
            {
                button.interactable = state;
                if (button.image != null)
                    button.image.raycastTarget = state;
            }
        }

        foreach (var toggle in toggles)
        {
            toggle.interactable = state;
        }

    }
}
