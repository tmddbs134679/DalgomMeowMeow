using UnityEngine;

public class TutorialRegisterTarget : MonoBehaviour
{
    public string RegisterKey;

    private void Start()
    {
        RegisterKey = gameObject.name;
        TutorialManager.Instance?.RegisterTarget(RegisterKey, gameObject);
    }
}