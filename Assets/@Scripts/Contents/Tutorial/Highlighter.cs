using UnityEngine;

public class Highlighter : MonoBehaviour
{
    public GameObject arrowEffect; // 강조 이펙트 (화살표, 테두리 등)

    public static Highlighter Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 강조 대상 오브젝트 위에 화살표 표시
    /// </summary>
    public void Show(GameObject target)
    {
        if (arrowEffect == null || target == null) return;

        arrowEffect.SetActive(true);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
        arrowEffect.transform.position = screenPos + new Vector3(0, 50f, 0); // UI offset
    }

    /// <summary>
    /// 강조 이펙트 끄기
    /// </summary>
    public void Hide()
    {
        if (arrowEffect != null)
            arrowEffect.SetActive(false);
    }
}