using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Lose : UI_Popup
{
    #region Enum
    enum Buttons
    {
        TitleButton,
        RetryButton,
    }
    #endregion
    private void Awake()
    {
        Init();
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        GetButton((int)Buttons.TitleButton).gameObject.BindEvent(OnClickTitle);
        GetButton((int)Buttons.RetryButton).gameObject.BindEvent(OnClickRetryButton);

        return true;
    }
    public void OnClickRetryButton()
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(Define.EScene.Test_Battle);
    }

    public void OnClickTitle()
    {
        Managers.UI.ClosePopupUI(this);
        Time.timeScale = 1f;
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
}
