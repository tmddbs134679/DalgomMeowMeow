using UnityEngine;
using UnityEngine.UI;

public class RoadButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button roadButton;

    private void Awake()
    {
        if (roadButton == null)
            roadButton = GetComponent<Button>();

        if (roadButton == null)
        {
            Debug.LogError("RoadButton not found!");
            return;
        }

        roadButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance != null &&
            TutorialManager.Instance.IsStepActive("도로 건설 버튼 누르기"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}