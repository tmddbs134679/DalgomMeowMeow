using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    private Button _titleButton;

    private void Awake()
    {
        _titleButton = GetComponent<Button>();
        if (_titleButton != null)
        {
            _titleButton.onClick.AddListener(OnClickTitleBtn);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject.");
        }
    }
    public void OnClickTitleBtn()
    {
    }
}
