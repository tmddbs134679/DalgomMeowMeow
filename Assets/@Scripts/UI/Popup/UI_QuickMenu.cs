using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuickMenu : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        CharacterNotifyObject,
        EquipmentNotifyObject
    }

    enum Buttons
    {
        BackgroundButton,
        CharacterInfoButton,
        CharacterEquipmentButton,
    }

    enum Texts
    {

    }
    #endregion

    UI_CharacterPopup _characterPopupUI;
    UI_EquipPopup _EquipPopupUI;
    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        CheckNotify();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));


        _characterPopupUI = Managers.UI.ShowPopupUI<UI_CharacterPopup>();
        _EquipPopupUI = Managers.UI.ShowPopupUI<UI_EquipPopup>();

        _characterPopupUI.gameObject.SetActive(false);
        _EquipPopupUI.gameObject.SetActive(false);

        GetButton((int)Buttons.CharacterInfoButton).gameObject.BindEvent(OnClickCharacterInfoButton);
        GetButton((int)Buttons.CharacterEquipmentButton).gameObject.BindEvent(OnClickCharacterEquipmentButton);
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);


        return true;
    }


    private void OnClickCharacterEquipmentButton()
    {
        _EquipPopupUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickCharacterInfoButton()
    {
        _characterPopupUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickBackgroundButton()
    {
        gameObject.SetActive(false);
    }

    private void CheckNotify()
    {
        //장비
        if (Managers.Game.OwnedEquipments.Any(e => !e.IsConfirmed))
            GetObject((int)GameObjects.EquipmentNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.EquipmentNotifyObject).SetActive(false);


    }
}
