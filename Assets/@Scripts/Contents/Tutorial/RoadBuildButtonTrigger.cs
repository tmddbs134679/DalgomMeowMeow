using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadBuildButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button roadBuildButton;

    private void Awake()
    {
        if (roadBuildButton == null)
            roadBuildButton = GetComponent<Button>();

        if (roadBuildButton == null)
        {
            Debug.LogError("RoadButton not found!");
            return;
        }

        roadBuildButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance != null &&
            TutorialManager.Instance.IsStepActive("도로건설버튼"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
