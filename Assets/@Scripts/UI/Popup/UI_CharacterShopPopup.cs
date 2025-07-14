using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_CharacterShopPopup : UI_Popup
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
        CharacterText,
    }
    #endregion



    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);
        PopupFadeInAnimation(GetText((int)Texts.CharacterText).gameObject);
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

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        GetButton((int)Buttons.OneTimeGachaButton).gameObject.BindEvent(OnClickOneTimeGacha);

        GetButton((int)Buttons.OneTimeGachaButton).gameObject.BindEvent(OnClickOneTimeGachaButton);
        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.BindEvent(OnClickFiveTimeGachaButton);

        GetButton((int)Buttons.OneTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        return true;
    }
    private void OnClickOneTimeGachaButton()
    {
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
    private void OnClickFiveTimeGachaButton()
    {
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
       // List<Character> character = new List<Character>();
        Managers.Game.DoCharacterGacha(count);
    }



    private void OnClickOneTimeGacha()
    {
        Managers.UI.ShowToast("골드가 부족합니다");
    }
}
