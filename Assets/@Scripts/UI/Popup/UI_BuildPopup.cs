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
        PotButton,
        TravelButton,
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
        PotGoldText,
        TravelGoldText,

        CookCountText = 100,
        FarmCountText,
        PlayGroundCountText,
        RestCountText,
        FishingCountText,
        StorageCountText,
        SlotMachineCountText,
        RoadCountText,
        ShopCountText,
        PotCountText,
        TravelCountText,
                PlayerGoldText=200,

    }

    enum Images
    {

    }
    #endregion

    private Character _character;
    private UI_FarmPopup _farmPopup;
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
        GetButton((int)Buttons.FarmButton).gameObject.BindEvent(ShowUIFarmPopup);
        GetButton((int)Buttons.PlayGroundButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Playing));
        GetButton((int)Buttons.RestButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Resting));
        GetButton((int)Buttons.FishingButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Fishing));
        GetButton((int)Buttons.StorageButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Storage));
        GetButton((int)Buttons.SlotMachineButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.SlotMachine));
        GetButton((int)Buttons.RoadButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Road));
        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Shop));
        GetButton((int)Buttons.UnlockAreaButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.UnLockStage));
        GetButton((int)Buttons.PotButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Pot));
        GetButton((int)Buttons.TravelButton).gameObject.BindEvent(() => SelectBuildingType(Define.BuildingType.Travel));
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuildUI);
        Managers.Game.OnResourcesChagned += Refresh;
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
      //  Refresh();
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

    private void ShowUIFarmPopup()
    {
        // 이미 열려 있다면 닫기
        if (_farmPopup != null)
        {
            Managers.UI.ClosePopupUI(_farmPopup); // 닫기
            _farmPopup = null; // 참조 제거
            return;
        }

        // 열려 있지 않으면 열기
        BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;
        _farmPopup = Managers.UI.ShowPopupUI<UI_FarmPopup>();
        _farmPopup.GetPopupObject(GetObject((int)GameObjects.BuildScrollObject));
        _farmPopup.GetPopup(this);
}

    //설치 건물 선택
    private void SelectBuildingType(Define.BuildingType type)
    {
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
            {
                BuildingPlacer.Instance.islimitBuildCount = false;
                return;
            }
        }
        if (type == Define.BuildingType.Shop)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.Shop.ToString(), out int count) && count >= 1)
            {
                BuildingPlacer.Instance.islimitBuildCount = false;
                return;
            }
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

        for (int i = 0; i < textcount-1; i++)
        {
            buildType = ((Define.BuildingType)i).ToString();

            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(buildType, out int count))
            {
            if (buildType == "Road") // 도로일 땐 else 처리,임시땜빵,나중에는 모든게 엑셀 데이터를 받아와서 계산해야함
            {

                GetText(i).text = BuildingPlacer.Instance.buildingSO[i].BuyMoney.ToString();
                GetText(ToIndex((Texts)(100 + i))).text = count.ToString();
                continue;
            }

                GetText(i).text = Mathf.RoundToInt(BuildingPlacer.Instance.buildingSO[i].BuyMoney * Mathf.Pow(1.2f, count)).ToString();
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
        return value >= 100 ? (value - 100) + 11 : value;
    }
    private void CancelBuildUI()
    {
        Debug.Log("buildpopup닫기");
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
    }


    //골드갱신 함수,이벤트연결됨
    private void Refresh()
    {
    //    GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
    }
    #endregion


    private void ValueCountsCheck()
    {
        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue("CatSlotMachine", out int value)) { }
    }
}
