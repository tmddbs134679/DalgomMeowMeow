using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform target; // 캐릭터 Transform
    public Vector3 offset = new Vector3(0, 2.25f, 0); // 머리 위 위치 조정
    public Image HpBar;
    public BattleCharacter BattleCharacter => target.GetComponent<BattleCharacter>();

    private void Start()
    {
        HpBar = transform.Find("HPbar").GetComponent<Image>();
        Managers.Debug.Log($"HealthBarUI Start: {BattleCharacter.Health} / {BattleCharacter.MaxHP}", Define.EDebugType.UI);
    }

    private void Update()
    {
        if (HpBar == null)
            return;

        HpBar.fillAmount = BattleCharacter.Health / BattleCharacter.MaxHP;
        if (target == null || Camera.main == null) return;
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
