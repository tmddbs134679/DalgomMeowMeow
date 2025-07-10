using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_ShopPopup : UI_Popup
{
   
    #region Enum
    enum GameObjects
    {
        ContentObject,
        ShopPopupGroupObject,
    }

    enum Buttons
    {
        CharacterPopupButton,
        EquipmentPopupButton,
        ExitButton,
    }

    enum Texts
    {

    }
    #endregion

    UI_EquipmentShopPopup _equipmentShopPopup;
    UI_CharacterShopPopup _characterShopPopup;

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));

        _characterShopPopup.gameObject.SetActive(true);
        _equipmentShopPopup.gameObject.SetActive(false);
    }
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

        _equipmentShopPopup = Managers.UI.ShowPopupUI<UI_EquipmentShopPopup>();
        _characterShopPopup = Managers.UI.ShowPopupUI<UI_CharacterShopPopup>();


        _characterShopPopup.gameObject.transform.SetParent(GetObject((int)GameObjects.ShopPopupGroupObject).transform, false);
        _equipmentShopPopup.gameObject.transform.SetParent(GetObject((int)GameObjects.ShopPopupGroupObject).transform, false);


        _equipmentShopPopup.gameObject.SetActive(false);

        GetButton((int)Buttons.CharacterPopupButton).gameObject.BindEvent(OnClickCharacterGachaButton);
        GetButton((int)Buttons.CharacterPopupButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipmentPopupButton).gameObject.BindEvent(OnClickEquipmentPopupButton);
        GetButton((int)Buttons.EquipmentPopupButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();


        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }

    void OnClickEquipmentPopupButton()
    {
        _characterShopPopup.gameObject.SetActive(false);
        _equipmentShopPopup.gameObject.SetActive(true);
    }

    void OnClickCharacterGachaButton()
    {
        _characterShopPopup.gameObject.SetActive(true);
        _equipmentShopPopup.gameObject.SetActive(false);
    }

    private void OnClickExitButton()
    {
       gameObject.SetActive(false);
    }
}
