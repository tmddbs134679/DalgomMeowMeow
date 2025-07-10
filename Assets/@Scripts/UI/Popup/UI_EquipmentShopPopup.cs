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

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));


        GetButton((int)Buttons.OneTimeGachaButton).gameObject.BindEvent(OnClickOneTimeGachaButon);
        GetButton((int)Buttons.OneTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.BindEvent(OnClickFiveTimeGachaButon);
        GetButton((int)Buttons.FiveTimeGachaButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        return true;
    }


    private void OnClickOneTimeGachaButon()
    {
        //일단 골드로

        if(Managers.Game.Gold > 1000)
        {
            Managers.Game.Gold -= 10000;

            DoGacha();

        }
    }

    private void DoGacha(int count = 1)
    {
        List<Equipment> equipment = new List<Equipment>();
        equipment = Managers.Game.DoEquipmentGacha(count);
    }

    private void OnClickFiveTimeGachaButon()
    {
       
    }

 
}
