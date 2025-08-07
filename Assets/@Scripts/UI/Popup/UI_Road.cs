using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        PlayerGoldTxt,
        DiaValueText
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
        GetText((int)Texts.PlayerGoldTxt).text = Managers.Game.Gold.ToString();
        Managers.Game.OnResourcesChagned += Refresh;
        Refresh();
        return true;
    }
 
    private void Refresh()
    {
        GetText((int)Texts.PlayerGoldTxt).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
    }

    public void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChagned -= Refresh;

        }
        BuildingPlacer.Instance.OnBuildingCancel -= OnClickCancelBuildButton;
    }

    private void OnClickRoadBuildButton()
    {
        BuildingPlacer.Instance.SelectBuildingType(Define.EBuildingType.Road);
        if (!BuildingPlacer.Instance.isGold) //돈이 부족할경우 게임씬으로 복귀
        {
            (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }

    private void OnClickRoadRemoveButton()
    {
        if (!GetButton((int)Buttons.RoadRemoveButton).interactable) return; // 튜토리얼 비활성화
        BuildingPlacer.Instance.SelectBuildingType(Define.EBuildingType.None);
        this.gameObject.SetActive(false);
    }

    private void OnClickCancelBuildButton()
    {
        if (!GetButton((int)Buttons.CancelButton).interactable) return;// 튜토리얼 비활성화
        BuildingPlacer.Instance.buildMap.ColliderAllOn();
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }


}
