using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button buildButton;

    private void Start()
    {
        if (buildButton == null)
            buildButton = GetComponent<Button>();
        if (buildButton == null)
        {
            Debug.LogError("BuildButton not found!");
            return;
        }
        buildButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance.IsStepActive("챕터2"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
