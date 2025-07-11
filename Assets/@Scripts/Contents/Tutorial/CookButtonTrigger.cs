using UnityEngine;
using UnityEngine.UI;

public class CookButtonTrigger : MonoBehaviour
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
            TutorialManager.Instance.IsStepActive("요리 건설 버튼 누르기"))
        {
            TutorialManager.Instance.CompleteStep();
        }
    }
}