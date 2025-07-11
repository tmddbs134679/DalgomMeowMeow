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
        GetButton((int)Buttons.SlotMachineButton).gameObject.BindEvent(() => SelectBuildingType(6));
        GetButton((int)Buttons.RoadButton).gameObject.BindEvent(() => SelectBuildingType(7));
        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(() => SelectBuildingType(8));
        GetButton((int)Buttons.UnlockAreaButton).gameObject.BindEvent(() => SelectBuildingType(9));
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
    private void SelectBuildingType(int type)
    {
        //건물 갯수 제한 코드 구간
        if (type == (int)Define.BuildingType.SlotMachine)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.SlotMachine.ToString(), out int count2) && count2 >= 1)
                return;
        }
        if (type == (int)Define.BuildingType.Shop)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.BuildingType.Shop.ToString(), out int count2) && count2 >= 1)
                return;
        }

        Setting();//데이터 갱신


        //   GetText(type).text = "돈";//Gold
        //  GetText(type + 100).text = "갯수";//Count;

        GetObject((int)GameObjects.BuildScrollObject).SetActive(false);
        BuildingPlacer.Instance.SelectBuildingType(type);
        Managers.UI.MakeSubItem<UI_BuildAction>(this.transform);
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
