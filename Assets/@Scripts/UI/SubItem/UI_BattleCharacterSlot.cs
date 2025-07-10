using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_BattleCharacterSlot : UI_Base
{ 
    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {

    }

    enum Images
    {
        CharacterIcon,
        HealthIcon,
        AtkIcon,
        SkillIcon,
    }

    enum Texts
    {
        Health,
        Atk,
    }

    #endregion

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        gameObject.BindEvent(OnClickObjectButton);
        return true;
    }

    private void OnClickObjectButton()
    {
        Managers.UI.GetPopupUI<UI_ForestPopup>().SelectCharacter(_character);
    }

    Character _character;

    public void SetInfo(Character character)
    {
        _character = character;
        
        GetImage((int)Images.CharacterIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        GetImage((int)Images.HealthIcon).sprite = Managers.Resource.Load<Sprite>("Cheese");
        GetImage((int)Images.AtkIcon).sprite = Managers.Resource.Load<Sprite>("Fork");
        //GetImage((int)Images.SkillIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.SkillTypeList.ToString());

        GetText((int)Texts.Health).text = _character.Hp.ToString();
        GetText((int)Texts.Atk).text = _character.Atk.ToString();
        // 스킬 아이콘은 아직 구현 안됨
    }
}
