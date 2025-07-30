using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform target; // 캐릭터 Transform
    public Vector3 offset = new Vector3(0, 2.25f, 0); // 머리 위 위치 조정
    public Image HpBar;
    public bool IsGiant;
    public BattleCharacter BattleCharacter => target.GetComponent<BattleCharacter>();
    public BattleManager BattleManager;

    private void Awake()
    {
        BattleManager = GetComponentInParent<BattleManager>();
    }
    private void Start()
    {
        HpBar = transform.Find("HPbar").GetComponent<Image>();
    }

    private void Update()
    {
        if (BattleCharacter.Health <= 0 || BattleCharacter.IsDead)
        {
            this.gameObject.SetActive(false);
        }

        if (BattleManager.Victory)
        {
            this.gameObject.SetActive(false);
        }
        if (HpBar == null)
            return;

        HpBar.fillAmount = BattleCharacter.Health / BattleCharacter.MaxHP;
        if (target == null || Camera.main == null) return;
        transform.position = target.position + offset;
        transform.forward = Camera.main.transform.forward;
    }
}
