using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_ExchangeShopPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ExchangeGoldObject,
        ExchangeDiaObject,
        ExchangeTicketObject,
    }

    enum Buttons
    {

    }

    enum Texts
    {
        TitleText
    }
    #endregion

    UI_PurchasePopup _purchasePopup;

    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
        PopupFadeInAnimation(GetText((int)Texts.TitleText).gameObject);
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

        _purchasePopup = Managers.UI.ShowPopupUI<UI_PurchasePopup>();
        _purchasePopup.gameObject.SetActive(false);

        GetObject((int)GameObjects.ExchangeGoldObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Gold));
        GetObject((int)GameObjects.ExchangeTicketObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Ticket));
        GetObject((int)GameObjects.ExchangeDiaObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Dia));


        return true;
    }

    private void OnClickExchange(Define.EExchangeType type)
    {
        _purchasePopup.gameObject.SetActive(true);
        _purchasePopup.SetInfo(type);
    }


}
