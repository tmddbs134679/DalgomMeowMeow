using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PurchasePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject
    }

    enum Buttons
    {
        BackgroundButton,
        PurchaseButton
    }
    
    enum Texts
    {
        ItemText,
        PriceText
    }

    enum Images
    {
        ItemImage
    }
    #endregion

    Define.EExchangeType _type;

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
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
        BindImage(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(OnClickPurchaseButton);

        return true;
    }

    private void OnClickPurchaseButton()
    {

        switch (_type)
        {
            case Define.EExchangeType.Gold:
                Managers.Game.Gold += 1000;
                Managers.Game.Dia -= 100;
                break;
            case Define.EExchangeType.Ticket:
                Managers.Game.Ticket += 1;
                Managers.Game.Dia -= 100;
                break;
            case Define.EExchangeType.Dia:
                Managers.Game.Dia += 70;
                Managers.Game.Gold -= 1000;
                break;
        }

        gameObject.SetActive(false);
    }

    private void OnClickBackgroundButton()
    {
        gameObject.SetActive(false);
    }

    public void SetInfo(Define.EExchangeType type) 
    {
        _type = type;

        switch (type)
        {
            case Define.EExchangeType.Gold:
                GetText((int)Texts.ItemText).text = "1000 Gold";
                GetImage((int)Images.ItemImage).sprite = Managers.Resource.Load<Sprite>("Gold.sprite");
                GetText((int)Texts.PriceText).text = "100";
            break;
            case Define.EExchangeType.Ticket:
                GetText((int)Texts.ItemText).text = "1 Ticket";
                GetImage((int)Images.ItemImage).sprite = Managers.Resource.Load<Sprite>("Ticket.sprite");
                GetText((int)Texts.PriceText).text = "100";
                break;
            case Define.EExchangeType.Dia:
                GetText((int)Texts.ItemText).text = "70 Dia";
                GetImage((int)Images.ItemImage).sprite = Managers.Resource.Load<Sprite>("Dia.sprite");
                GetText((int)Texts.PriceText).text = "1000";
                break;
        }
    }
}
