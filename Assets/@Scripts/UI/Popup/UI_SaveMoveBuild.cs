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
                BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        return true;
    }
        public void OnDestroy()
    {
        BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;
    }
    void Start()
    {
        BuildingPlacer.Instance.uI_BuildAction.transform.position = this.transform.position;
        BuildingPlacer.Instance.uI_BuildAction.SetActive(true);
    }

    #region Build
    private void CancelBuildUI()
    {
        BuildingPlacer.Instance.isSelect = false;
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }

    #endregion

}
