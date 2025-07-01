using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickMenu : UI_Popup
{
    #region Enum
    enum GameObjects
    {
   
    }

    enum Buttons
    {
        BackgroundButton,
        CharacterInfoButton,
    }

    enum Texts
    {

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
        BindText(typeof(Texts));

        GetButton((int)Buttons.CharacterInfoButton).gameObject.BindEvent(OnClickCharacterInfoButton);
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickCharacterInfoButton()
    {
        Managers.UI.ShowPopupUI<UI_CharacterPopup>();
    }


 
}
