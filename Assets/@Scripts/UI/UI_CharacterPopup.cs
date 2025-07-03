using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        CharacterInfoScrollContentObject,
    }

    enum Buttons
    {
        ExitButton,
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

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        Refresh();
        return true;
    }

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
      
    }

    private void Refresh()
    {
        List<Character> characters = Managers.Game.Characters;
        foreach (Character ch in characters)
        {
            UI_CharacterInfo slot = Managers.UI.MakeSubItem<UI_CharacterInfo>(GetObject((int)GameObjects.CharacterInfoScrollContentObject).transform);
            slot.SetInfo(ch);
        }
    }
    
}
