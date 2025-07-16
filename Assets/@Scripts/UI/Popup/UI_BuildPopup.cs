using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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
        SlotMachineButton,
        RoadButton,
        ShopButton,
        CancelButton,
        UnlockAreaButton,

    }

    enum Texts
    {
        CookGoldText,
        FarmGoldText,
        PlayGroundGoldText,
        RestGoldText,
        FishingGoldText,
        StorageGoldText,
        SlotMachineGoldText,
        RoadGoldText,
        ShopGoldText,
        PlayerGoldText,

        CookCountText = 100,
        FarmCountText,
        PlayGroundCountText,
        RestCountText,
        FishingCountText,
        StorageCountText,
        SlotMachineCountText,
        RoadCountText,
        ShopCountText,

    }

    enum Images
    {

    }
    #endregion

    private Character _character;
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

        GetButton((int)Buttons.CookButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Cooking));
        GetButton((int)Buttons.FarmButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Farm));
        GetButton((int)Buttons.PlayGroundButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Playing));
        GetButton((int)Buttons.RestButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Resting));
        GetButton((int)Buttons.FishingButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Fishing));
        GetButton((int)Buttons.StorageButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Storage));
        GetButton((int)Buttons.SlotMachineButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.SlotMachine));
        GetButton((int)Buttons.RoadButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Road));
        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Shop));
        GetButton((int)Buttons.UnlockAreaButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.UnLockStage));
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuildUI);
        Managers.Game.OnResourcesChagned += Refresh;
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        Refresh();
        Setting();
        return true;
    }

    public void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChagned -= Refresh;

        }
        BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;

    }

    #region Build



    //설치 건물 선택
    private void SelectBuildingType(Define.BuildingType type)
    {

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
        }
        if (type == Define.BuildingType.Shop)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.Shop.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
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

            if (buildType == "Road") // 도로일 땐 else 처리,임시땜빵,나중에는 모든게 엑셀 데이터를 받아와서 계산해야함
            {

                GetText(i).text = BuildingPlacer.Instance.buildingSO[i].BuyMoney.ToString();
                GetText(ToIndex((Texts)(100 + i))).text = "0";
                continue;
            }
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
        return value >= 100 ? (value - 100) + 10 : value;
    }
    private void CancelBuildUI()
    {
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }


    //골드갱신 함수,이벤트연결됨
    private void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
    }
    #endregion


    private void ValueCountsCheck()
    {
        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue("CatSlotMachine", out int value)) { }
    }
}
