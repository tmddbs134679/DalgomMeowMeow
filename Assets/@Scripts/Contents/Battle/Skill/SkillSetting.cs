using UnityEngine;
using UnityEngine.UI;

public class SkillSetting : MonoBehaviour
{
    public Image skill_1; // 스킬 이미지
    public Image skill_1_CoolDown; // 스킬 쿨타임 이미지
    public BattleCharacter character_1;

    public Image skill_2; // 스킬 이미지
    public Image skill_2_CoolDown; // 스킬 쿨타임 이미지
    public BattleCharacter character_2;

    public Image skill_3; // 스킬 이미지
    public Image skill_3_CoolDown; // 스킬 쿨타임 이미지
    public BattleCharacter character_3;

    private void Start()
    {
        if (character_1 != null)
            SetSkillIcon(character_1.SkillID, skill_1, skill_1_CoolDown);
        if (character_2 != null)
            SetSkillIcon(character_2.SkillID, skill_2, skill_2_CoolDown);
        if (character_3 != null)
            SetSkillIcon(character_3.SkillID, skill_3, skill_3_CoolDown);

    }

    public void SetSkillIcon(string id, Image img, Image cooldownImg)
    {
        img.sprite = Managers.Resource.Load<Sprite>(id);
        cooldownImg.sprite = Managers.Resource.Load<Sprite>(id);
        cooldownImg.color = new Color(0, 0, 0, 0.5f); // 쿨타임 이미지 투명도 설정
        cooldownImg.gameObject.SetActive(false); // 쿨타임 이미지 비활성화
    }
}
