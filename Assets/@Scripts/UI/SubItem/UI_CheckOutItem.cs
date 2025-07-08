using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CheckOutItem : UI_Base
{

    #region Enums
    enum GameObjects
    {

    }

    enum Texts
    {

    }

    enum Images
    {

    }

    #endregion

    int _dayCount;
    bool _isCheckOut;
    private void OnEnable()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));



        #endregion

        Refresh();
        return true;
    }



    public void SetInfo(int dayCount, bool isCheckOut)
    {
        transform.localScale = Vector3.one;

        _dayCount = dayCount;
        _isCheckOut = isCheckOut;
        Refresh();
    }

    private void Refresh()
    {

    }
}
