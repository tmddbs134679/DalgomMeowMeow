using UnityEngine;

public class TutorialRegisterTarget : MonoBehaviour
{
    public string RegisterKey;

    private void OnEnable()
    {
        RegisterKey = gameObject.name;
        TutorialManager.Instance?.RegisterTarget(RegisterKey, gameObject);
    }
}