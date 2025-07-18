using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class UI_FarmPopup : UI_Popup
{
    enum GameObjects
    {
        BuildScrollObject,
    }

    enum Buttons
    {
        CabbageButton,
        CarrotButton,
        PumpkinButton,
        PotatoButton,
        OnionButton,
        CloseBackGroundButton,
    }
    enum Texts
    {
        CabbageGoldText,
        CarrotGoldText,
        PumpkinGoldText,
        PotatoGoldText,
        OnionGoldText,

        CabbageCountText = 100,
        CarrotCountText,
        PumpkinCountText,
        PotatoCountText,
        OnionCountText,
    }
    enum Images { }

    private UI_BuildPopup _uI_BuildPopup;
    private GameObject _buildScrollObject;//이전 ui에서 받아온거
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.CabbageFarm));
        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.CarrotFarm));
        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.OnionFarm));
        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.PotatoFarm));
        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.PumpkinFarm));
        //   GetButton((int)Buttons.CloseBackGroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        Setting();
        return true;
    }
    public void OnDestroy()
    {
        BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;
    }

    //설치 건물 선택
    private void SelectBuildingType(Define.BuildingType type)
    {
        _buildScrollObject.SetActive(false);
        // var button = GetButton(type);
        // if (button != null && !button.interactable)
        //     return;
        LimitBuildCount(type);
        if (!BuildingPlacer.Instance.islimitBuildCount) return;
        Setting();//데이터 갱신

        GetObject((int)GameObjects.BuildScrollObject).SetActive(false);
        BuildingPlacer.Instance.SelectBuildingType(type);
        UI_BuildAction popup = Managers.UI.MakeSubItem<UI_BuildAction>(this.transform);
        // popup.islimitBuildCount = islimitBuildCount;
    }
    //건물 갯수 제한 코드 구간
    private void LimitBuildCount(Define.BuildingType type)
    {
        if (type == Define.BuildingType.SlotMachine)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.SlotMachine.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }
        if (type == Define.BuildingType.Shop)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.Shop.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }
        BuildingPlacer.Instance.islimitBuildCount = true;
    }

    //buildUI창 건설비용과 건물갯수 갱신
    private void Setting()
    {
        foreach (var a in BuildingPlacer.Instance.buildMap.valueCounts)
        {
            Debug.Log($"#############{a.Key} : {a.Value}개");
        }
        string buildType;
        int textcount = Enum.GetValues(typeof(Texts))
    .Cast<int>()
    .Count(value => value < 100);

        for (int i = 0; i < textcount - 1; i++)
        {
            buildType = ((Define.BuildingType)i).ToString();
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(buildType, out int count))
            {
                GetText(i).text = (BuildingPlacer.Instance.buildingSO[i].BuyMoney * Mathf.Pow(1.2f, count)).ToString();
                GetText(ToIndex((Texts)(100 + i))).text = count.ToString();
            }
            else
            {
                GetText(i).text = BuildingPlacer.Instance.buildingSO[i].BuyMoney.ToString();
                GetText(ToIndex((Texts)(100 + i))).text = "0";
            }
        }
    }
    //이거좀 불안정함
    private int ToIndex(Texts text)
    {
        int value = (int)text;
        return value >= 100 ? (value - 100) + 5 : value;
    }

    private void CancelBuildUI()
    {
        Debug.Log("farmpopup닫기");
        Managers.UI.ClosePopupUI(this); //스택이기 때문에 닫는 순서 중요함
        Managers.UI.ClosePopupUI(_uI_BuildPopup);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }

    public void GetPopupObject(GameObject BuildScrollObject)
    {
        _buildScrollObject = BuildScrollObject;
    }
    
        public void GetPopup(UI_BuildPopup uI_BuildPopup)
    {
        _uI_BuildPopup = uI_BuildPopup;
    }
}
