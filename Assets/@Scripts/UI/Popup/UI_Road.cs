using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Road : UI_Popup
{
    enum GameObjects
    {
    }

    enum Buttons
    {
        RoadBuildButton,
        RoadRemoveButton,
        CancelButton,
    }
    enum Texts
    {
    }
    enum Images {}



    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.RoadBuildButton).gameObject.BindEvent(OnClickRoadBuildButton);
        GetButton((int)Buttons.RoadRemoveButton).gameObject.BindEvent(OnClickRoadRemoveButton);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelBuildButton);
                        BuildingPlacer.Instance.OnBuildingCancel += OnClickCancelBuildButton;
        return true;
    }

    public void OnDestroy()
    {
        BuildingPlacer.Instance.OnBuildingCancel -= OnClickCancelBuildButton;
    }
    private void OnClickRoadBuildButton()
    {
        BuildingPlacer.Instance.SelectBuildingType(Define.EBuildingType.Road);
        Managers.UI.ClosePopupUI(this);

    }

    private void OnClickRoadRemoveButton()
    {
        BuildingPlacer.Instance.RemoveRoad();
                Managers.UI.ClosePopupUI(this);

    }

    private void OnClickCancelBuildButton()
    {
                Debug.Log("폯파!");
        BuildingPlacer.Instance.buildMap.ColliderAllOn();
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }


}
