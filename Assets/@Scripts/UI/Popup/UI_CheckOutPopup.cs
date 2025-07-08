using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_CheckOutPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,

    }

    enum Buttons
    {
        ExitButton,
    }

    enum Texts
    {

    }
    #endregion


    public int _CheckOutDay;
    int _monthCount;
    int _dailyCount;

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Object Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ExitButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        #endregion


        Refresh();

        return true;
    }

    public void SetInfo(int checkOutDay)
    {

    }

    void Refresh()
    {


    }

    private void OnClickExitButton()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
