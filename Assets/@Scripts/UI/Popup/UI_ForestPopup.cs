using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ForestPopup : UI_Popup
{
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        BattleButton,
        Background
    }
    enum Texts { ForestTitle, }
    
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(OnClickBattleButton);
        GetButton((int)Buttons.Background).gameObject.BindEvent(OnClickBackgroundButton);

        return true;
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI();
    }

    private void OnClickBattleButton()
    {
        Managers.Scene.LoadScene(Define.EScene.Test_Battle);
    }
}
