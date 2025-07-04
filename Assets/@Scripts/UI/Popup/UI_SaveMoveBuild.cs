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
        return true;
    }
    void Start()
    {
        BuildingPlacer.Instance.uI_BuildAction = Managers.UI.MakeSubItem<UI_BuildAction>(this.transform);
        Debug.Log(BuildingPlacer.Instance.uI_BuildAction+"세이브무브빌드");
    }

    #region Build
    private void CancelBuildUI()
    {
        BuildingPlacer.Instance.isSelect = false;
        Managers.UI.CloseAllPopupUI();
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }

    #endregion

}
