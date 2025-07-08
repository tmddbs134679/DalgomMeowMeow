using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleScene : UI_Scene
{
    [SerializeField] private BattleCharacter _playerCharacter_1;
    [SerializeField] private BattleCharacter _playerCharacter_2;
    [SerializeField] private BattleCharacter _playerCharacter_3;

    [SerializeField] private ButtonCoolDown _skill_1;
    [SerializeField] private ButtonCoolDown _skill_2;
    [SerializeField] private ButtonCoolDown _skill_3;

    private GameObject _skillScene;
    #region Enum

    enum Buttons
    {
        PauseButton,
        Skill_1_Btn,
        Skill_2_Btn,
        Skill_3_Btn,
    }
    #endregion

    private void Awake()
    {
        Init();
        UI_Skill _skill = Managers.UI.ShowPopupUI<UI_Skill>(); // 스킬 UI 표시
        _skillScene = _skill.gameObject; // 스킬 UI 게임 오브젝트 저장
        _skillScene.SetActive(false); // 스킬 UI 비활성화
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.Skill_1_Btn).gameObject.BindEvent(OnClickSkill_1Button);
        GetButton((int)Buttons.Skill_2_Btn).gameObject.BindEvent(OnClickSkill_2Button);
        GetButton((int)Buttons.Skill_3_Btn).gameObject.BindEvent(OnClickSkill_3Button);



        return true;
    }
    UI_PausePopup _pausePopup;
    public void OnClickPauseButton()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        _pausePopup = Managers.UI.ShowPopupUI<UI_PausePopup>(); // PausePopup UI 표시
        _pausePopup.gameObject.SetActive(true);
    }

    public void OnClickSkill_1Button()
    {
        _playerCharacter_1.ActiveSkill(); // 플레이어 캐릭터 1의 첫 번째 스킬 활성화
        StartCoroutine(SkillCutScene()); // 스킬 컷씬 실행
        _skill_1.SkillActive(_playerCharacter_1.SkillCooldown); // 스킬 버튼 쿨타임 활성화
    }
    public void OnClickSkill_2Button()
    {
        _playerCharacter_2.ActiveSkill(); // 플레이어 캐릭터 2의 두 번째 스킬 활성화
        StartCoroutine(SkillCutScene()); // 스킬 컷씬 실행
        _skill_2.SkillActive(_playerCharacter_2.SkillCooldown); // 스킬 버튼 쿨타임 활성화
    }
    public void OnClickSkill_3Button()
    {
        _playerCharacter_3.ActiveSkill(); // 플레이어 캐릭터 3의 세 번째 스킬 활성화
        StartCoroutine(SkillCutScene()); // 스킬 컷씬 실행
        _skill_3.SkillActive(_playerCharacter_3.SkillCooldown); // 스킬 버튼 쿨타임 활성화
    }

    private IEnumerator SkillCutScene()
    {
        //색바꾸기 or 이미지 바꾸기 등

        _skillScene.SetActive(true); // 스킬 UI 활성화
        Time.timeScale = 0f; // 게임 일시 정지
        yield return new WaitForSecondsRealtime(1f); // 0.5초 대기
        Time.timeScale = 1f; // 게임 재개
        _skillScene.SetActive(false); // 스킬 UI 비활성화
    }
}
