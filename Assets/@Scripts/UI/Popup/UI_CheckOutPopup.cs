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
        CheckOutBoardObject,
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
        _CheckOutDay = checkOutDay;
        Refresh();
    }

    void Refresh()
    {
        if (_init == false)
            return;

        if (_CheckOutDay == 0)
            return;

        _monthCount = _CheckOutDay % 30;
        _dailyCount = _monthCount % 10;

        //if (_dailyCount == 0)
        //{
        //    _dailyCount = 10;
        //}
        GetObject((int)GameObjects.CheckOutBoardObject).DestroyChilds();

        Transform parent = GetObject((int)GameObjects.CheckOutBoardObject).transform;
        for (int count = 1; count <= 10; count++)
        {
            UI_CheckOutItem item = Managers.UI.MakeSubItem<UI_CheckOutItem>(parent);
            item.transform.SetAsLastSibling();

            if (_dailyCount >= count)
                item.SetInfo(count, true);
            else
                item.SetInfo(count, false);
        }

    }

    private void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }
}
