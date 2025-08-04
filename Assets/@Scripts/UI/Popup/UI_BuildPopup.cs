using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
     //   RoadButton,
        ShopButton,
        CancelButton,
        PotButton,
        TravelButton,
        UnlockAreaButton,


    }

    enum Texts
    {
        CookGoldText,
      //  FarmGoldText,
        PlayGroundGoldText,
        RestGoldText,
        FishingGoldText,
        StorageGoldText,
        SlotMachineGoldText,
       // RoadGoldText,
        ShopGoldText,
        PotGoldText,
        TravelGoldText,


        CookCountText,
      //  FarmCountText,
        PlayGroundCountText,
        RestCountText,
        FishingCountText,
        StorageCountText,
        SlotMachineCountText,
      //  RoadCountText,
        ShopCountText,
        PotCountText,
        TravelCountText,


        PlayerGoldTxt,
        DiaValueText,


        SlotMachineMaxText,
        ShopMaxText,
        PotMaxText,
        TravelMaxText,


    }

    enum Images
    {
        SlotLockImage,
        ShopLockImage,
        PotLockImage,
        TravelLockImage,
        SlotMachineImage,
        ShopImage,
        PotImage,
        TravelImage,
        SlotMachineChapterLock,
        StorageChapterLock,
        FishingChapterLock,
    }
    #endregion

    private Character _character;
    private UI_FarmPopup _farmPopup;
    Dictionary<Texts, (Define.EBuildingType buildingType, Texts countText)> goldTextToBuildingMap;
    private Dictionary<Define.EBuildingType, int> _buttonToMap;
    ScrollRect _scrollRect;
    bool _isDrag = false;


    private void Awake()
    {
        Init();
    }
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        SetDictionary();
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _scrollRect = GetObject((int)GameObjects.BuildScrollObject).GetComponent<ScrollRect>();

        ButtonSetting();

        GetButton((int)Buttons.CookButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Cooking));
        GetButton((int)Buttons.FarmButton).gameObject.BindEvent(ShowUIFarmPopup);
        GetButton((int)Buttons.PlayGroundButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Playing));
        GetButton((int)Buttons.RestButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Resting));
        GetButton((int)Buttons.FishingButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Fishing));
        GetButton((int)Buttons.StorageButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Storage));
        GetButton((int)Buttons.SlotMachineButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.SlotMachine));
       // GetButton((int)Buttons.RoadButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Road));
        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Shop));
        // GetButton((int)Buttons.UnlockAreaButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.UnLockStage));
        GetButton((int)Buttons.PotButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Pot));
        GetButton((int)Buttons.TravelButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.Travel));
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(CancelBuildUI);
        Managers.Game.OnResourcesChagned += Refresh;
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        Refresh();
        Setting();
        UpdateButtonStates(); //챕터

        return true;
    }

    void OnEnable()
    {
        HideOrShowUI();
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

    /// <summary>
    /// Enum Texts와 BuildingType 서로 매칭 연결
    /// </summary>
    private void SetDictionary()
    {
        goldTextToBuildingMap = new()
        {
    { Texts.CookGoldText, (Define.EBuildingType.Cooking, Texts.CookCountText) },
   // { Texts.FarmGoldText, (Define.EBuildingType.CabbageFarm, Texts.FarmCountText) },
    { Texts.PlayGroundGoldText, (Define.EBuildingType.Playing, Texts.PlayGroundCountText) },
    { Texts.RestGoldText, (Define.EBuildingType.Resting, Texts.RestCountText) },
    { Texts.FishingGoldText, (Define.EBuildingType.Fishing, Texts.FishingCountText) },
    { Texts.StorageGoldText, (Define.EBuildingType.Storage, Texts.StorageCountText) },
    { Texts.SlotMachineGoldText, (Define.EBuildingType.SlotMachine, Texts.SlotMachineCountText) },
   // { Texts.RoadGoldText, (Define.EBuildingType.Road, Texts.RoadCountText) },
    { Texts.ShopGoldText, (Define.EBuildingType.Shop, Texts.ShopCountText) },
    { Texts.PotGoldText, (Define.EBuildingType.Pot, Texts.PotCountText) },
    { Texts.TravelGoldText, (Define.EBuildingType.Travel, Texts.TravelCountText) },
        };


        _buttonToMap = new()
{
    { Define.EBuildingType.Cooking,(int)Buttons.CookButton },
    { Define.EBuildingType.Playing,(int)Buttons.PlayGroundButton },
    { Define.EBuildingType.Resting,(int)Buttons.RestButton },
    { Define.EBuildingType.Fishing,(int)Buttons.FishingButton },
    { Define.EBuildingType.Storage,(int)Buttons.StorageButton },
    { Define.EBuildingType.SlotMachine,(int)Buttons.SlotMachineButton },
 //   { Define.EBuildingType.Road,(int)Buttons.RoadButton },
    { Define.EBuildingType.Shop,(int)Buttons.ShopButton },
    { Define.EBuildingType.UnLockStage,(int)Buttons.UnlockAreaButton },
    { Define.EBuildingType.Pot,(int)Buttons.PotButton },
    { Define.EBuildingType.Travel,(int)Buttons.TravelButton },
};
    }




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
        // BuildingPlacer.Instance.OnBuildingCancel -= CancelBuildUI;
        _farmPopup = Managers.UI.ShowPopupUI<UI_FarmPopup>();
        _farmPopup.GetPopupObject(GetObject((int)GameObjects.BuildScrollObject));
        _farmPopup.GetPopup(this);
    }

    //설치 건물 선택
    private void SelectBuildingType(Define.EBuildingType type)
    {
        if (_buttonToMap.TryGetValue(type, out int buttons))
        {
            var button = GetButton(buttons);
            if (button != null && !button.interactable)
            {
                Managers.UI.ShowToast("아직 열리지 않았습니다");
                return;
            }
        }
        OnOffFarmPopup();
        LimitBuildCount(type);
        if (!BuildingPlacer.Instance.islimitBuildCount) return;
        Setting();//데이터 갱신


        GetObject((int)GameObjects.BuildScrollObject).SetActive(false);
        BuildingPlacer.Instance.SelectBuildingType(type);
        if (BuildingPlacer.Instance.isGold) //돈이 부족할경우 게임씬으로 복귀
        {

            BuildingPlacer.Instance.uI_BuildAction.transform.position = this.transform.position;
      //      if (type != Define.EBuildingType.Road)//도로는 다른곳에서 실행하므로 여기서는 패스
         BuildingPlacer.Instance.uI_BuildAction.SetActive(true);
        }
        else
        {
            (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
        }
    }
    //건물 갯수 제한 코드 구간
    private void LimitBuildCount(Define.EBuildingType type)
    {
        BuildingPlacer.Instance.islimitBuildCount = true;
        if (type == Define.EBuildingType.SlotMachine)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.SlotMachine.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }
        if (type == Define.EBuildingType.Shop)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Shop.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }

        if (type == Define.EBuildingType.Pot)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Pot.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }

        if (type == Define.EBuildingType.Travel)
        {
            if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Travel.ToString(), out int count) && count >= 1)
                BuildingPlacer.Instance.islimitBuildCount = false;
            return;
        }
    }

    private void HideOrShowUI()
    {
        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.SlotMachine.ToString(), out int Slotcount) && Slotcount >= 1)
        {
            GetText((int)Texts.SlotMachineGoldText).gameObject.SetActive(false);
            GetText((int)Texts.SlotMachineCountText).gameObject.SetActive(false);
            GetText((int)Texts.SlotMachineMaxText).gameObject.SetActive(true);
            GetImage((int)Images.SlotLockImage).gameObject.SetActive(true);
            GetImage((int)Images.SlotMachineImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.SlotMachineGoldText).gameObject.SetActive(true);
            GetText((int)Texts.SlotMachineCountText).gameObject.SetActive(true);
            GetText((int)Texts.SlotMachineMaxText).gameObject.SetActive(false);
            GetImage((int)Images.SlotLockImage).gameObject.SetActive(false);
            GetImage((int)Images.SlotMachineImage).gameObject.SetActive(true);
        }

        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Shop.ToString(), out int Shopcount) && Shopcount >= 1)
        {
            GetText((int)Texts.ShopGoldText).gameObject.SetActive(false);
            GetText((int)Texts.ShopCountText).gameObject.SetActive(false);
            GetText((int)Texts.ShopMaxText).gameObject.SetActive(true);
            GetImage((int)Images.ShopLockImage).gameObject.SetActive(true);
            GetImage((int)Images.ShopImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.ShopGoldText).gameObject.SetActive(true);
            GetText((int)Texts.ShopCountText).gameObject.SetActive(true);
            GetText((int)Texts.ShopMaxText).gameObject.SetActive(false);
            GetImage((int)Images.ShopLockImage).gameObject.SetActive(false);
            GetImage((int)Images.ShopImage).gameObject.SetActive(true);
        }

        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Pot.ToString(), out int Potcount) && Potcount >= 1)
        {
            GetText((int)Texts.PotGoldText).gameObject.SetActive(false);
            GetText((int)Texts.PotCountText).gameObject.SetActive(false);
            GetText((int)Texts.PotMaxText).gameObject.SetActive(true);
            GetImage((int)Images.PotLockImage).gameObject.SetActive(true);
            GetImage((int)Images.PotImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.PotGoldText).gameObject.SetActive(true);
            GetText((int)Texts.PotCountText).gameObject.SetActive(true);
            GetText((int)Texts.PotMaxText).gameObject.SetActive(false);
            GetImage((int)Images.PotLockImage).gameObject.SetActive(false);
            GetImage((int)Images.PotImage).gameObject.SetActive(true);
        }

        if (BuildingPlacer.Instance.buildMap.valueCounts.TryGetValue(Define.EBuildingType.Travel.ToString(), out int Travelcount) && Travelcount >= 1)
        {
            GetText((int)Texts.TravelGoldText).gameObject.SetActive(false);
            GetText((int)Texts.TravelCountText).gameObject.SetActive(false);
            GetText((int)Texts.TravelMaxText).gameObject.SetActive(true);
            GetImage((int)Images.TravelLockImage).gameObject.SetActive(true);
            GetImage((int)Images.TravelImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.TravelGoldText).gameObject.SetActive(true);
            GetText((int)Texts.TravelCountText).gameObject.SetActive(true);
            GetText((int)Texts.TravelMaxText).gameObject.SetActive(false);
            GetImage((int)Images.TravelLockImage).gameObject.SetActive(false);
            GetImage((int)Images.TravelImage).gameObject.SetActive(true);
        }

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

                // if (buildingType == Define.EBuildingType.Road) // 도로일 땐 else 처리,임시땜빵,나중에는 모든게 엑셀 데이터를 받아와서 계산해야함
                // {
                //     GetText((int)goldTextEnum).text = buyMoney.ToString();
                //     GetText((int)countText).text = output.ToString();
                //     continue;
                // }

                if (buildingType == Define.EBuildingType.Resting)
                {
                    if (output > 0)
                    {
                        GetText((int)goldTextEnum).text = ((int)(buyMoney * Mathf.Pow(3f, output))).ToString();
                        GetText((int)countText).text = output.ToString();
                    }
                    else
                    {
                        GetText((int)goldTextEnum).text = buyMoney.ToString();
                        GetText((int)countText).text = "0";
                    }
                    continue;
                }

                GetText((int)goldTextEnum).text = ((int)(buyMoney * Mathf.Pow(10f, output))).ToString();
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
        BuildingPlacer.Instance.buildMap.ColliderAllOn();
        OnOffFarmPopup();
        Managers.UI.ClosePopupUI(this);
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);
        TutorialManager.Instance.SetAllUIInteractable(true);//튜토리얼 스킵시 버튼활성화
    }


    //골드갱신 함수,이벤트연결됨
    private void Refresh()
    {
        GetText((int)Texts.PlayerGoldTxt).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
    }
    #endregion

    //챕터
    private Dictionary<Define.EBuildingType, string> _contentIdMap = new()
    {
        { Define.EBuildingType.Fishing,        "Building_FishingSpot" },
        { Define.EBuildingType.Storage,        "Building_AnimalStorage" },
        { Define.EBuildingType.SlotMachine,    "Building_SlotMachine" },
    };

    public void UpdateButtonStates()
    {
        foreach (var pair in _contentIdMap)
        {
            Define.EBuildingType type = pair.Key;
            string contentId = pair.Value;

            bool isUnlocked = QuestManager.Instance.IsUnlocked(contentId);
            var button = GetButton((int)(Buttons)Enum.Parse(typeof(Buttons), $"{type}Button")); // "Cooking" → "CookButton"
            var lockImage = GetImage((int)(Images)Enum.Parse(typeof(Images), $"{type}ChapterLock"));

            if (button != null)
                button.interactable = isUnlocked;
            if (lockImage != null)
                lockImage.gameObject.SetActive(!isUnlocked);
        }
    }

    private void OnOffFarmPopup()
    {
        // 이미 열려 있다면 닫기
        if (_farmPopup != null)
        {
            Managers.UI.ClosePopupUI(_farmPopup); // 닫기
            _farmPopup = null; // 참조 제거
        }
    }


    private void ButtonSetting()
    {
        GetButton((int)Buttons.CookButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.CookButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.CookButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.FarmButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.FarmButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.FarmButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.PlayGroundButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.PlayGroundButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.PlayGroundButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.RestButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.RestButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.RestButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.FishingButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.FishingButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.FishingButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.StorageButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.StorageButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.StorageButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.SlotMachineButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.SlotMachineButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.SlotMachineButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

 //       GetButton((int)Buttons.RoadButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
   //     GetButton((int)Buttons.RoadButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
    //    GetButton((int)Buttons.RoadButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.ShopButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.ShopButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.ShopButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.UnlockAreaButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.UnlockAreaButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.UnlockAreaButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.PotButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.PotButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.PotButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.TravelButton).gameObject.gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.TravelButton).gameObject.gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.TravelButton).gameObject.gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);
    }

    public void OnDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnDrag(pointerEventData);
    }

    public void OnBeginDrag(BaseEventData baseEventData)
    {
        _isDrag = true;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnBeginDrag(pointerEventData);
    }

    public void OnEndDrag(BaseEventData baseEventData)
    {
        _isDrag = false;
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        _scrollRect.OnEndDrag(pointerEventData);

    }
}
