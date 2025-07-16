using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class UI_FarmPopup : UI_Popup
{
    enum GameObjects
    {

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
        GetButton((int)Buttons.CloseBackGroundButton).gameObject.BindEvent(OnClickBackgroundButton);        
        return true;
    }


    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

 //설치 건물 선택
    private void SelectBuildingType(Define.BuildingType type)
    {
        LimitBuildCount(type);
        if (!BuildingPlacer.Instance.islimitBuildCount) return;
        Setting();//데이터 갱신


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


}
