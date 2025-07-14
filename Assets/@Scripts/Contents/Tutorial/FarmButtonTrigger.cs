using UnityEngine;
using UnityEngine.UI;

public class FarmButtonTrigger : MonoBehaviour
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
            TutorialManager.Instance.IsStepActive("농장 버튼"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}