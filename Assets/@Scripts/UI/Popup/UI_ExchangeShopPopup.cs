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
        ExchangeAdsObject,
        ExchangeGoldObject,
        ExchangeDiaObject,
        ExchangeTicketObject,
    }

    enum Buttons
    {

    }

    enum Texts
    {
        TitleText,
        AdsText
    }
    #endregion

    UI_PurchasePopup _purchasePopup;


    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
        PopupFadeInAnimation(GetText((int)Texts.TitleText).gameObject);
        PopupFadeInAnimation(GetObject((int)GameObjects.ExchangeAdsObject).gameObject);
        PopupFadeInAnimation(GetObject((int)GameObjects.ExchangeGoldObject).gameObject);
        PopupFadeInAnimation(GetObject((int)GameObjects.ExchangeDiaObject).gameObject);
        PopupFadeInAnimation(GetObject((int)GameObjects.ExchangeTicketObject).gameObject);
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

        GetObject((int)GameObjects.ExchangeAdsObject).BindEvent(OnClickAdsButton);
        GetObject((int)GameObjects.ExchangeGoldObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Gold));
        GetObject((int)GameObjects.ExchangeTicketObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Ticket));
        GetObject((int)GameObjects.ExchangeDiaObject).BindEvent(() => OnClickExchange(Define.EExchangeType.Dia));


        return true;
    }

    private void OnClickAdsButton()
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.AdvancedGachaOpenCount >= 1)
        {
            Managers.Ads.ShowRewardedAd(() =>
            {
                Managers.Game.AdvancedGachaOpenCount--;
                Managers.Game.Gold += 1000;
                GetText((int)Texts.AdsText).text = Managers.Game.AdvancedGachaOpenCount.ToString();
                Managers.UI.ShowToast("보상 획득");
            });

        }
        else
        {
            Managers.UI.ShowToast("일일 광고 횟수 초과!");
        }
    }

    private void OnClickExchange(Define.EExchangeType type)
    {
        Managers.Sound.PlayButtonClick();

        if (type == Define.EExchangeType.None)
            return;

        switch (type)
        {
            case Define.EExchangeType.Gold:
                if(Managers.Game.Dia < 100)
                {
                    Managers.UI.ShowToast("다이아가 부족합니다!");
                    return;
                }
                break;
            case Define.EExchangeType.Ticket:
                if (Managers.Game.Dia < 100)
                {
                    Managers.UI.ShowToast("다이아가 부족합니다!");
                    return;
                }
                break;
            case Define.EExchangeType.Dia:
                if (Managers.Game.Gold < 1000)
                {
                    Managers.UI.ShowToast("골드가 부족합니다!");
                    return;
                }
                break;
        }

        //재화가 있을경우 진행
        _purchasePopup.gameObject.SetActive(true);
        _purchasePopup.SetInfo(type);
    }


}
