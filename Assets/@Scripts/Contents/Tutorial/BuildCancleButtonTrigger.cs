using UnityEngine;
using UnityEngine.UI;

public class BuildCancleButtonTrigger : MonoBehaviour
{
    [SerializeField] private Button roadButton;

    private void Awake()
    {
        if (roadButton == null)
            roadButton = GetComponent<Button>();

        if (roadButton == null)
        {
            Debug.LogError("cancle not found!");
            return;
        }

        roadButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (TutorialManager.Instance != null &&
            TutorialManager.Instance.IsStepActive("빌드캔슬"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}
