using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SaveMoveBuild : UI_Popup
{
    #region Enum
    enum GameObjects
    {

    }

    enum Buttons
    {
CancelButton
    }

    enum Texts
    {
    
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

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuildUI);

        BuildingPlacer.Instance.uI_BuildAction= Managers.UI.ShowPopupUI<UI_BuildAction>();
        return true;
    }

    #region Build
    private void CancelBuildUI()
    {
        Managers.UI.CloseAllPopupUI();
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }

    #endregion

}
