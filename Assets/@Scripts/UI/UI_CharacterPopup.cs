using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CharacterInfoScrollContentObject,
        CharacterInfoScrollGroup
    }

    enum Buttons
    {
        ExitButton,
        CarSortButton,
        BearSortButton
    }

    enum Texts
    {

    }
    #endregion

    ScrollRect _scrollrect;
    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        Refresh();
    }

    private void OnDestroy()
    {
        
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        _scrollrect = GetObject((int)GameObjects.CharacterInfoScrollGroup).GetComponent<ScrollRect>();

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.CarSortButton).gameObject.BindEvent(OnClickCatSortButton);
        GetButton((int)Buttons.BearSortButton).gameObject.BindEvent(OnClickBearSortButton);

        Refresh();
        return true;
    }
    private void OnClickCatSortButton()
    {
        Refresh(1);
    }

    private void OnClickBearSortButton()
    {
        Refresh(0);
    }

    private void OnClickExitButton()
    {
        //Nofity Check
        if (Managers.UI.SceneUI is UI_GameScene)
            (Managers.UI.SceneUI as UI_GameScene).CheckNotify();
        else
            (Managers.UI.SceneUI as UI_CharacterStoreScene).CheckNotify();

        gameObject.SetActive(false);
    }

    private void Refresh(int? alpha = null)
    {
        GetObject((int)GameObjects.CharacterInfoScrollContentObject).DestroyChilds();

       

        List<Character> characters = Managers.Game.Characters;

        if (alpha == 1)
            characters.Sort((a, b) => b.Data.DataId.CompareTo(a.Data.DataId));
        else if (alpha == 0)
            characters.Sort((a, b) => a.Data.DataId.CompareTo(b.Data.DataId));

        foreach (Character ch in characters)
        {
            UI_CharacterInfo slot = Managers.UI.MakeSubItem<UI_CharacterInfo>(GetObject((int)GameObjects.CharacterInfoScrollContentObject).transform);
            slot.SetInfo(ch, _scrollrect);
        }
    }
    
}
