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
      //  CloseBackGroundButton,
    }
    enum Texts
    {
        CabbageGoldText,
        CarrotGoldText,
        PumpkinGoldText,
        PotatoGoldText,
        OnionGoldText,

        CabbageCountText,
        CarrotCountText,
        PumpkinCountText,
        PotatoCountText,
        OnionCountText,
    }
    enum Images { }

    private UI_BuildPopup _uI_BuildPopup;
    private GameObject _buildScrollObject;//이전 ui에서 받아온거

    Dictionary<Texts, (Define.EBuildingType buildingType, Texts countText)> goldTextToBuildingMap;
    public override bool Init()
    {
        if (!base.Init()) return false;

        SetDictionary();


        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.CabbageFarm));
        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.CarrotFarm));
        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.OnionFarm));
        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.PotatoFarm));
        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.PumpkinFarm));
        //   GetButton((int)Buttons.CloseBackGroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        Setting();
        return true;
    }
    public void OnDestroy()
    {
        BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;
    }

/// <summary>
/// Enum Texts와 BuildingType 서로 매칭 연결
/// </summary>
    private void SetDictionary()
    {
        goldTextToBuildingMap = new()
{
    { Texts.CabbageGoldText, (Define.EBuildingType.CabbageFarm, Texts.CabbageCountText) },
    { Texts.CarrotGoldText,  (Define.EBuildingType.CarrotFarm,  Texts.CarrotCountText) },
    { Texts.OnionGoldText,   (Define.EBuildingType.OnionFarm,   Texts.OnionCountText) },
    { Texts.PotatoGoldText,  (Define.EBuildingType.PotatoFarm,  Texts.PotatoCountText) },
    { Texts.PumpkinGoldText, (Define.EBuildingType.PumpkinFarm, Texts.PumpkinCountText) },
};
    }
    //설치 건물 선택
    private void SelectBuildingType(Define.EBuildingType type)
    {
        _buildScrollObject.SetActive(false);
        // var button = GetButton((int)type);
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
    private void LimitBuildCount(Define.EBuildingType type)
    {
        BuildingPlacer.Instance.islimitBuildCount = true;
    }

    /// <summary>
    /// buildUI창 건설비용과 건물갯수 갱신
    /// goldTextEnum=Enum.Texts(GoldText)  buildingType=Enum.BuildingType  countText=Enum.Texts(CountText)
    /// </summary>
  private void Setting()
    {
        // foreach (var a in BuildingPlacer.Instance.buildMap.valueCounts)
        // {
        //     Managers.Debug.Log($"#############{a.Key} : {a.Value}개",Define.EDebugType.Building);
        // }

        Texts[] goldTexts = ((Texts[])Enum.GetValues(typeof(Texts))) //TextsEnum 의 Goldtext부분을 배열로 저장(크기 저장)
            .Where(t => t.ToString().EndsWith("GoldText"))
            .ToArray();
        for (int i = 0; i < goldTexts.Length; i++)
        {
            var goldTextEnum = goldTexts[i];
            var buildingType = goldTextToBuildingMap[goldTextEnum].buildingType;
            var countText = goldTextToBuildingMap[goldTextEnum].countText;
            var buyMoney = BuildingPlacer.Instance.buildingSO[(int)buildingType].BuyMoney;


            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(buildingType.ToString(), out int output))
            {
                GetText((int)goldTextEnum).text = ((int)(buyMoney * Mathf.Pow(1.2f, output))).ToString();
                GetText((int)countText).text = output.ToString();
            }
            else
            {
                GetText((int)goldTextEnum).text = buyMoney.ToString();
                GetText((int)countText).text = "0";
            }
        }
    }

    private void CancelBuildUI()
    {
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
