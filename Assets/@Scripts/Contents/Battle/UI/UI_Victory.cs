using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class UI_Victory : UI_Popup   //어드레서블에 프리펩 넣기
{
    #region Enum
    enum Buttons
    {
        TitleButton,
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

        return true;
    }

    public void OnClickTitle()
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Scene.LoadScene(Define.EScene.GameScene);
    }
}
