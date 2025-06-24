using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Scene
{
    #region Enum
    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }
    #endregion

    bool isPreload = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        // 테스트용
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
                Managers.Scene.LoadScene(Define.EScene.LobbyScene, transform);
        });

        return true;
    }


}
