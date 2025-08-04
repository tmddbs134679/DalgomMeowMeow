using System.Collections;
using UnityEngine;

public class UI_BattleScene : UI_Scene
{
    [SerializeField] private BattleCharacter[] _playerCharacters = new BattleCharacter[3];
    [SerializeField] private ButtonCoolDown[] _skillButtons = new ButtonCoolDown[3];

    private UI_PausePopup _pausePopup;
    private GameObject _skillScene;
    private UI_Skill _skill;

    #region Enum
    enum Buttons
    {
        PauseButton,
        Skill_1_Btn,
        Skill_2_Btn,
        Skill_3_Btn,
    }
    #endregion

    public Sprite cat;  // 임시
    public Sprite bear; // 임시

    private void Awake()
    {
        Init();

        _skill = Managers.UI.ShowPopupUI<UI_Skill>();
        _skillScene = _skill.gameObject;
        _skillScene.SetActive(false);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);

        // 스킬 버튼 바인딩
        for (int i = 0; i < _playerCharacters.Length; i++)
        {
            int idx = i; // 클로저 방지
            GetButton((int)Buttons.Skill_1_Btn + idx).gameObject
                .BindEvent(() => OnClickSkillButton(idx));
        }

        return true;
    }

    private void Update()
    {
        for (int i = 0; i < _playerCharacters.Length; i++)
        {
            if (_playerCharacters[i].IsDead)
                _skillButtons[i].ButtonLock();
        }
    }

    private void OnClickPauseButton()
    {
        Managers.Sound.PlayButtonClick();
        Time.timeScale = 0f;
        _pausePopup = Managers.UI.ShowPopupUI<UI_PausePopup>();
        _pausePopup.gameObject.SetActive(true);
    }

    private void OnClickSkillButton(int index)
    {
        BattleCharacter character = _playerCharacters[index];
        if (character.IsDead) return;
        _skill.SetImage(Managers.Resource.Load<Sprite>($"{character.CharID}_S"));

        //추가 사운드 이펙트
        if(character.CharID.Contains("B"))
        {
            Managers.Sound.Play(Define.ESound.Effect, "BearSkill");
        }
        else
        {
            Managers.Sound.Play(Define.ESound.Effect, "CatSkill");
        }

            StartCoroutine(SkillCutScene());
        character.ActiveSkill();
        _skillButtons[index].SkillActive(character.SkillCooldown, character.IsDead);
    }

    private IEnumerator SkillCutScene()
    {
        _skillScene.SetActive(true);
        Time.timeScale = 0.25f;
        yield return new WaitForSecondsRealtime(0.375f);
        Time.timeScale = 1f;
        _skillScene.SetActive(false);
    }

    public void SetOFF()
    {
        gameObject.SetActive(false);
    }
}