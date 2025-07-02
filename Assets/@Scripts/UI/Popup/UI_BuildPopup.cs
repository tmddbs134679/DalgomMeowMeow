using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuildPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        BuildScrollObject,
        ResourceInfo
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
        CancelButton,

    }

    enum Texts
    {
        PlayerGoldText,
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
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuildUI);

        Managers.Game.OnResourcesChagned += Refresh;

        Refresh();

        return true;
    }

    public void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChagned -= Refresh;
    }

    #region Build

    private void SelectBuildingType(int type)
    {
        GetObject(((int)GameObjects.BuildScrollObject)).SetActive(false);
        BuildingPlacer.Instance.SelectBuildingType(type);
        Managers.UI.MakeSubItem<UI_BuildAction>(this.transform);
    }

    private void CancelBuildUI()
    {
        Managers.UI.CloseAllPopupUI();
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }


    private void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
    }
    #endregion

}
