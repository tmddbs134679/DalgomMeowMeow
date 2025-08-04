using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    enum Images 
    {         
        CabbageButtonLockImage,
        CarrotButtonLockImage,
        PumpkinButtonLockImage,
        PotatoButtonLockImage,
        OnionButtonLockImage, 
    }

    ScrollRect _scrollRect;
    bool _isDrag = false;

    private UI_BuildPopup _uI_BuildPopup;
    private GameObject _buildScrollObject;//이전 ui에서 받아온거

    Dictionary<Texts, (Define.EBuildingType buildingType, Texts countText)> goldTextToBuildingMap;
        private Dictionary<Define.EBuildingType,int> _buttonToMap;
    public override bool Init()
    {
        if (!base.Init()) return false;

        SetDictionary();


        BindObject(typeof(GameObjects));

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        _scrollRect = GetObject((int)GameObjects.BuildScrollObject).GetComponent<ScrollRect>();


        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);

        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(null, OnDrag, Define.EUIEvent.Drag);
        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(null, OnBeginDrag, Define.EUIEvent.BeginDrag);
        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(null, OnEndDrag, Define.EUIEvent.EndDrag);


        GetButton((int)Buttons.CabbageButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.CabbageFarm));
        GetButton((int)Buttons.CarrotButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.CarrotFarm));
        GetButton((int)Buttons.OnionButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.OnionFarm));
        GetButton((int)Buttons.PotatoButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.PotatoFarm));
        GetButton((int)Buttons.PumpkinButton).gameObject.BindEvent(() => SelectBuildingType(Define.EBuildingType.PumpkinFarm));
        //   GetButton((int)Buttons.CloseBackGroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        BuildingPlacer.Instance.OnBuildingCancel += CancelBuildUI;
        Setting();
        UpdateButtonStates();//챕터
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
        
                _buttonToMap = new()
                {
                     { Define.EBuildingType.CabbageFarm,   (int)Buttons.CabbageButton },
                     { Define.EBuildingType.CarrotFarm,    (int)Buttons.CarrotButton },
                     { Define.EBuildingType.OnionFarm,     (int)Buttons.OnionButton },
                     { Define.EBuildingType.PotatoFarm,    (int)Buttons.PotatoButton },
                     { Define.EBuildingType.PumpkinFarm,   (int)Buttons.PumpkinButton },
                };
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
        LimitBuildCount(type);
        if (!BuildingPlacer.Instance.islimitBuildCount) return;
        Setting();//데이터 갱신
        _buildScrollObject.SetActive(false);
        GetObject((int)GameObjects.BuildScrollObject).SetActive(false);
        BuildingPlacer.Instance.SelectBuildingType(type);
                if (BuildingPlacer.Instance.isGold) //돈이 부족할경우 게임씬으로 복귀
        {

            BuildingPlacer.Instance.uI_BuildAction.transform.position = this.transform.position;
            if (type != Define.EBuildingType.Road)//도로는 다른곳에서 실행하므로 여기서는 패스
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

    //챕터
    private Dictionary<Define.EBuildingType, Buttons> _buttonMap = new()
        {
            { Define.EBuildingType.CarrotFarm,  Buttons.CarrotButton },
            { Define.EBuildingType.PotatoFarm,  Buttons.PotatoButton },
            { Define.EBuildingType.PumpkinFarm, Buttons.PumpkinButton },
            { Define.EBuildingType.OnionFarm,   Buttons.OnionButton }
        };

    
        public void UpdateButtonStates()
        {
            foreach (var pair in _buttonMap)
            {
                var buildingType = pair.Key;
                var buttonEnum = pair.Value;

                string contentId = $"Building_{buildingType}";
                bool isUnlocked = QuestManager.Instance.IsUnlocked(contentId);

                var button = GetButton((int)buttonEnum);
                var lockImage = GetImage((int)(Images)Enum.Parse(typeof(Images), $"{buttonEnum}LockImage"));
                if (button != null)
                    button.interactable = isUnlocked;
                if (lockImage != null)
                    lockImage.enabled = !isUnlocked;
            }
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
