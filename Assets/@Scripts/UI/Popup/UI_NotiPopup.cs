using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NotiPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject
    }

    enum Buttons
    {
        BackgroundButton,

    }

    private void OnEnable()
    {
        PopupOpenAnimation(gameObject);

    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);



        return true;
    }

    private void OnClickBackgroundButton()
    {
        gameObject.SetActive(false);
    }
}
