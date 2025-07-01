using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
    }

    enum Buttons
    {
        CookButton,
        FarmButton,
        PlayGroundButton,
        RestButton,
        FishingButton,
        StorageButton,
        LoadButton,
        AceeptButton,
        CancelButton,
    }

    enum Texts
    {

    }

    enum Images
    {

    }
    #endregion

    Character _character;
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

        GetButton((int)Buttons.CookButton).gameObject.BindEvent(() => SelectBuildingType(0));
        GetButton((int)Buttons.FarmButton).gameObject.BindEvent(() => SelectBuildingType(1));
        GetButton((int)Buttons.PlayGroundButton).gameObject.BindEvent(() => SelectBuildingType(2));
        GetButton((int)Buttons.RestButton).gameObject.BindEvent(() => SelectBuildingType(3));
        GetButton((int)Buttons.FishingButton).gameObject.BindEvent(() => SelectBuildingType(4));
        GetButton((int)Buttons.StorageButton).gameObject.BindEvent(() => SelectBuildingType(5));
        GetButton((int)Buttons.LoadButton).gameObject.BindEvent(() => SelectBuildingType(6));
        GetButton((int)Buttons.AceeptButton).gameObject.BindEvent(AcceptBuild);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuild);

        return true;
    }

    #region Build
    private void AcceptBuild()
    {
        BuildingPlacer.Instance.AcceptBuild();
    }
    private void CancelBuild()
    {
        BuildingPlacer.Instance.CancelBuild();
    }
    private void SelectBuildingType(int type)
    {
        Debug.Log(type);

        UI_BuildAction builder = Managers.UI.ShowPopupUI<UI_BuildAction>();
        // builder
        BuildingPlacer.Instance.SelectBuildingType(type);
        Managers.UI.MakeSubItem<UI_BuildAction>(this.transform);
    }
    #endregion

}
