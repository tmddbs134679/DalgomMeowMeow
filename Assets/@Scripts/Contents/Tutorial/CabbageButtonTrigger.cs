using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CabbageButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button cookButton;

    private void Awake()
    {
        if (cookButton == null)
            cookButton = GetComponent<Button>();

        if (cookButton == null)
        {
            Debug.LogError("cookButton not found!");
            return;
        }

        cookButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance != null &&
            TutorialManager.Instance.IsStepActive("배추농장버튼"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
