
using System.Collections.Generic;

using UnityEngine;


public class UI_ForestPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        Content,
        ContentGroup,
    }

    enum Buttons
    {
        Select1,
        Select2,
        Select3,
        BattleButton,
        Background,
    }
    enum Texts 
    {
        ForestTitleText,
        SkillText1,
        SkillText2,
        SkillText3,
        SkillName1,
        SkillName2,
        SkillName3,
    }

    enum Images
    {
        Select1,
        Select2,
        Select3,
        SkillIcon1,
        SkillIcon2,
        SkillIcon3,
        FirstEnemy,
        SecondEnemy,
        ThirdEnemy,
    }
    #endregion


    Character _character;
    

    private StageSO stagedata;
    private Character[] _selectedCharacters = new Character[3];

    //List<Character> _selectedCharacters = new List<Character>();

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentGroup));
        RefreshSelectedSlots();

    }
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(OnClickBattleButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);
        

        GetCharacterInfo();
        GetStageData();

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI();
    }

    private void OnClickBattleButton()
    {
        StageDataManager.Instance.PlayerCharacter = _selectedCharacters;
        Managers.Scene.LoadScene(Define.EScene.BattleScene);
    }

    private void GetStageData()
    {
        stagedata = StageDataManager.Instance.SetStage();
        switch (stagedata.EnemyID.Length) 
        {
            case 1:
                break;
            case 2:
                GetImage((int)Images.FirstEnemy).sprite = Managers.Resource.Load <Sprite>(stagedata.EnemyID[0]);
                GetImage((int)Images.SecondEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[1]);
                GetImage((int)Images.ThirdEnemy).gameObject.SetActive(false);
                break;
            case 3:
                GetImage((int)Images.FirstEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[0]);
                GetImage((int)Images.SecondEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[1]);
                GetImage((int)Images.ThirdEnemy).sprite = Managers.Resource.Load<Sprite>(stagedata.EnemyID[2]);
                break;
        }
    }
    #region 캐릭터 정보 가져오기 및 삭제

    public void GetCharacterInfo()
    {
        List<Character> characters = Managers.Game.Characters;
        foreach (Character ch in characters)
        {
            UI_BattleCharacterSlot slot = Managers.UI.MakeSubItem<UI_BattleCharacterSlot>(GetObject((int)GameObjects.Content).transform);
            slot.SetInfo(ch);
        }
    }

    public void SelectCharacter(Character character)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_selectedCharacters[i] == character)
                return;
        }

        for (int i = 0; i < 3; i++)
        {
            if (_selectedCharacters[i] == null)
            {
                _selectedCharacters[i] = character;
                break;
            }
        }
        RefreshSelectedSlots();
    }

    public void OnClickDelectSelectCharacter(int index)
    {
        _selectedCharacters[index] = null;
        RefreshSelectedSlots();
    }

    private void RefreshSelectedSlots()
    {
        // 1) 슬롯 초기화
        for (int i = 0; i < 3; i++)
        {
            GetImage((int)Images.Select1 + i).sprite = null;
            GetImage((int)Images.SkillIcon1 + i).sprite = null;
            GetText((int)Texts.SkillText1 + i).text = string.Empty; 
            GetText((int)Texts.SkillName1 + i).text = string.Empty;
            GetButton((int)Buttons.Select1 + i).gameObject.BindEvent(() => { });
        }

        // 2) 선택된 캐릭터 정보 업데이트
        for (int i = 0; i < 3; i++)
        {
            Character character = _selectedCharacters[i];
            var capturedCharacter = i;

            if (character == null) continue;

            GetImage((int)Images.Select1 + i).sprite = Managers.Resource.Load<Sprite>(character.Data.IconLabel);
            GetImage((int)Images.SkillIcon1 + i).sprite = Managers.Resource.Load<Sprite>(character.Data.SkillID.ToString());
            GetText((int)Texts.SkillText1 + i).text = Managers.Data.SkillDataDic[character.Data.SkillID].Description;
            GetText((int)Texts.SkillName1 + i).text = Managers.Data.SkillDataDic[character.Data.SkillID].Name;


            GetButton((int)Buttons.Select1 + i).gameObject.BindEvent(() =>
                OnClickDelectSelectCharacter(capturedCharacter)
            );

        }
    }

    #endregion 



}
