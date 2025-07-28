using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_EquipmentShopPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
       
    }

    enum Buttons
    {
        ProbabilityButton,
        OneTimeGachaButton,
        FiveTimeGachaButton
    }

    enum Texts
    {
        EquipmentText
    }
    #endregion


    UI_GachaTablePopup _gachaTablePopup;

    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
        PopupFadeInAnimation(GetText((int)Texts.EquipmentText).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.ProbabilityButton).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.OneTimeGachaButton).gameObject);
        PopupFadeInAnimation(GetButton((int)Buttons.FiveTimeGachaButton).gameObject);
    }
    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _gachaTablePopup = Managers.UI.ShowPopupUI<UI_GachaTablePopup>();


        _gachaTablePopup.gameObject.SetActive(false);

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ProbabilityButton).gameObject.BindEvent(OnClickGachaTableButon);
        GetButton((int)Buttons.ProbabilityButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();



        GetButton((int)Buttons.OneTimeGachaButton).gameObject.BindEvent(OnClickOneTimeGachaButon);
        GetButton((int)Buttons.OneTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.BindEvent(OnClickFiveTimeGachaButon);
        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        _gachaTablePopup.SetInfo(Define.EGachaType.Equipment);

        return true;
    }

    private void OnClickGachaTableButon()
    {
        Managers.Sound.PlayButtonClick();
        _gachaTablePopup.gameObject.SetActive(true);
    }

    private void OnClickOneTimeGachaButon()
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Ticket >= 1)
        {
            Managers.Game.RemoveTicket(1);
            DoGacha();
        }
        else
        {
            Managers.UI.ShowToast("티켓이 부족합니다 !");
        }
    }

    private void OnClickFiveTimeGachaButon()
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.Ticket >= 5)
        {
            Managers.Game.RemoveTicket(5);
            DoGacha(5);
        }
        else
        {
            Managers.UI.ShowToast("티켓이 부족합니다 !");
        }
    }
    private void DoGacha(int count = 1)
    {
        List<Equipment> equipment = new List<Equipment>();
        equipment = Managers.Game.DoEquipmentGacha(count);
    }


 
}
