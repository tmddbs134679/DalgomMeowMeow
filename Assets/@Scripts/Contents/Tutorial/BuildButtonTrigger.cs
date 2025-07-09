using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button buildButton;

    private void Start()
    {
        buildButton = GameObject.Find("BuildButton")?.GetComponent<Button>();
        if (buildButton == null)
        {
            Debug.LogError("BuildButton not found!");
            return;
        }
        buildButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance.IsStepActive("건설 버튼 누르기"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
