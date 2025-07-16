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
        PurchaseButton,
        MinusButton,
        PlusButton
    }
    
    enum Texts
    {
        ItemText,
        PriceText,
        SliderValueText
    }

    enum Images
    {
        ItemImage
    }

    enum Sliders
    {
        Slider
    }
    #endregion

    Define.EExchangeType _type;
    int _price;
    int _sellPrice;
    int _count;
    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        GetSlider((int)Sliders.Slider).value = 1;
        GetText((int)Texts.SliderValueText).text = "1";

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
        BindSlider(typeof(Sliders));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.PurchaseButton).gameObject.BindEvent(OnClickPurchaseButton);
        GetButton((int)Buttons.MinusButton).gameObject.BindEvent(OnClickMiusButton);
        GetButton((int)Buttons.PlusButton).gameObject.BindEvent(OnClickPlusButton);

        GetSlider((int)Sliders.Slider).onValueChanged.AddListener(UpdateSlider);


        return true;
    }

    private void OnClickPlusButton()
    {
        GetSlider((int)Sliders.Slider).value++;
    }

    private void OnClickMiusButton()
    {
        GetSlider((int)Sliders.Slider).value--;
    }

    private void OnClickPurchaseButton()
    {

        switch (_type)
        {
            case Define.EExchangeType.Gold:
                Managers.Game.Gold += 1000 * _count;
                Managers.Game.Dia -= _sellPrice;
                break;
            case Define.EExchangeType.Ticket:
                Managers.Game.Ticket += 1 * _count;
                Managers.Game.Dia -= _sellPrice;
                break;
            case Define.EExchangeType.Dia:
                Managers.Game.Dia += 70 * _count;
                Managers.Game.Gold -= _sellPrice;
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
                GetText((int)Texts.PriceText).text = Define.DIA_TO_GOLD_PRICE.ToString();

                GetSlider((int)Sliders.Slider).maxValue = Mathf.FloorToInt(Managers.Game.Dia / Define.DIA_TO_GOLD_PRICE);
                _price = Define.DIA_TO_GOLD_PRICE;
                break;
            case Define.EExchangeType.Ticket:
                GetText((int)Texts.ItemText).text = "1 Ticket";
                GetImage((int)Images.ItemImage).sprite = Managers.Resource.Load<Sprite>("Ticket.sprite");
                GetText((int)Texts.PriceText).text = Define.DIA_TO_TICKET_PRICE.ToString();

                GetSlider((int)Sliders.Slider).maxValue = Mathf.FloorToInt(Managers.Game.Dia / Define.DIA_TO_TICKET_PRICE);
                _price = Define.DIA_TO_TICKET_PRICE;
                break;
            case Define.EExchangeType.Dia:
                GetText((int)Texts.ItemText).text = "70 Dia";
                GetImage((int)Images.ItemImage).sprite = Managers.Resource.Load<Sprite>("Dia.sprite");
                GetText((int)Texts.PriceText).text = Define.GOLD_TO_DIA_PRICE.ToString();

                GetSlider((int)Sliders.Slider).maxValue = Mathf.FloorToInt(Managers.Game.Gold / Define.GOLD_TO_DIA_PRICE);
                _price = Define.GOLD_TO_DIA_PRICE;
                break;
        }
        

    }

    void UpdateSlider(float count)
    {
        GetText((int)Texts.SliderValueText).text = count.ToString();
        GetText((int)Texts.PriceText).text = (_price * count).ToString();
        _sellPrice = _price * (int)count;
        _count = (int)count;

    }
   
}
