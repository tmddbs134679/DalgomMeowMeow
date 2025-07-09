using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CharacterPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
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
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        Refresh();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        Refresh();
        return true;
    }

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
      
    }

    private void Refresh()
    {
        GetObject((int)GameObjects.CharacterInfoScrollContentObject).DestroyChilds();

        List<Character> characters = Managers.Game.Characters;
        foreach (Character ch in characters)
        {
            UI_CharacterInfo slot = Managers.UI.MakeSubItem<UI_CharacterInfo>(GetObject((int)GameObjects.CharacterInfoScrollContentObject).transform);
            slot.SetInfo(ch);
        }
    }
    
}
