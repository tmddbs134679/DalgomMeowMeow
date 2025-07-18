using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_ChangeCharacterSlot : UI_Base
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
        MoveSpeedIcon,
    }
    //
    enum Texts
    {
        MoveSpeed,
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
        Managers.UI.GetPopupUI<UI_ChangePopup>().OnClickSlot(_character);
        var outLine = GetImage((int)Images.CharacterIcon).GetComponent<Outline>();
        outLine.enabled = _character.InMainScene;
        
    }

    Character _character;

    public void SetInfo(Character character)
    {
        _character = character;

        GetImage((int)Images.CharacterIcon).sprite = Managers.Resource.Load<Sprite>(_character.Data.IconLabel);
        var outLine = GetImage((int)Images.CharacterIcon).GetComponent<Outline>();
        outLine.enabled = _character.InMainScene;


        GetImage((int)Images.MoveSpeedIcon).sprite = Managers.Resource.Load<Sprite>("Bread");

        GetText((int)Texts.MoveSpeed).text = _character.MoveSpeed.ToString();
    }
}
