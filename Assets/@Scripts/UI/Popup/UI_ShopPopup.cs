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
        TicketObject,
        DiaObject,
        GoldObject
    }

    enum Buttons
    {
        CharacterPopupButton,
        EquipmentPopupButton,
        ExchangePopupButton,
        ExitButton,
    }

    enum Texts
    {
        TicketText,
        DiaText,
        GoldText
    }
    #endregion

    UI_EquipmentShopPopup _equipmentShopPopup;
    UI_CharacterShopPopup _characterShopPopup;
    UI_ExchangeShopPopup _exchangeShopPopup;

    private void OnEnable()
    {
        Managers.Game.OnResourcesChagned -= Refresh;
        Managers.Game.OnResourcesChagned += Refresh;

        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        _characterShopPopup.gameObject.SetActive(true);
        _equipmentShopPopup.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Managers.Game.OnResourcesChagned -= Refresh;
    }

    private void OnDestroy()
    {
        Managers.Game.OnResourcesChagned -= Refresh;
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
        _exchangeShopPopup = Managers.UI.ShowPopupUI<UI_ExchangeShopPopup>();

        _characterShopPopup.gameObject.transform.SetParent(GetObject((int)GameObjects.ShopPopupGroupObject).transform, false);
        _equipmentShopPopup.gameObject.transform.SetParent(GetObject((int)GameObjects.ShopPopupGroupObject).transform, false);
        _exchangeShopPopup.gameObject.transform.SetParent(GetObject((int)GameObjects.ShopPopupGroupObject).transform, false);

        GetObject((int)GameObjects.DiaObject).BindEvent(OnExchangeShopButton);
        GetObject((int)GameObjects.TicketObject).BindEvent(OnExchangeShopButton);
        

        _equipmentShopPopup.gameObject.SetActive(false);
        _exchangeShopPopup.gameObject.SetActive(false);
        GetObject((int)GameObjects.DiaObject).SetActive(false);
        GetObject((int)GameObjects.GoldObject).SetActive(false);

        GetButton((int)Buttons.CharacterPopupButton).gameObject.BindEvent(OnClickCharacterGachaButton);
        GetButton((int)Buttons.CharacterPopupButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipmentPopupButton).gameObject.BindEvent(OnClickEquipmentPopupButton);
        GetButton((int)Buttons.EquipmentPopupButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ExchangePopupButton).gameObject.BindEvent(OnExchangeShopButton);
        GetButton((int)Buttons.ExchangePopupButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();


        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        Refresh();

        return true;
    }

    private void OnExchangeShopButton()
    {
        GetObject((int)GameObjects.DiaObject).SetActive(true);
        GetObject((int)GameObjects.GoldObject).SetActive(true);


        _characterShopPopup.gameObject.SetActive(false);
        _exchangeShopPopup.gameObject.SetActive(true);
        _equipmentShopPopup.gameObject.SetActive(false);
    }


    void OnClickEquipmentPopupButton()
    {
        GetObject((int)GameObjects.DiaObject).SetActive(false);
        GetObject((int)GameObjects.GoldObject).SetActive(false);

        _characterShopPopup.gameObject.SetActive(false);
        _exchangeShopPopup.gameObject.SetActive(false);
        _equipmentShopPopup.gameObject.SetActive(true);

    }

    void OnClickCharacterGachaButton()
    {

        GetObject((int)GameObjects.DiaObject).SetActive(false);
        GetObject((int)GameObjects.GoldObject).SetActive(false);

        _characterShopPopup.gameObject.SetActive(true);
        _exchangeShopPopup.gameObject.SetActive(false);
        _equipmentShopPopup.gameObject.SetActive(false);
    }

    private void OnClickExitButton()
    {
       gameObject.SetActive(false);
    }

    private void Refresh()
    {
        GetText((int)Texts.TicketText).text = Managers.Game.Ticket.ToString();
        GetText((int)Texts.DiaText).text = Managers.Game.Dia.ToString();
        GetText((int)Texts.GoldText).text = Managers.Game.Gold.ToString();
    }
}
