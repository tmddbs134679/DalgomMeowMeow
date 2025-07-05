using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_PausePopup : UI_Popup
{
    #region Enum

    enum Buttons
    {
        ResumeButton,
        RetryButton,
        TitleButton
    }
    #endregion

    private void Awake()
    {
        Init();
    }

    private void OnEnable()
    {

    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        /*
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.RetryButton).gameObject.BindEvent(OnClickRetryButton);
        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitleButton);
        */


        return true;
    }


}
