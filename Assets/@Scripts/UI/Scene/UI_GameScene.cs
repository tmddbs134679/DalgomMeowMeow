using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Define;


public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        StorageObject,
        QuickNotifyObject
    }

    enum Buttons
    {
        NoticeButton,
        SettingButton,
        QuestButton,
        CheckOutButton,
        QuickButton,
        ShopButton,
        BuildButton,
        EditPosButton
    }

    enum Texts
    {
        PlayerGoldText,
        CreatureCountText,
        DiaText,
    }
    #endregion

    UI_QuickMenu _quickMenuPopupUI;
    UI_CheckOutPopup _checkOutPopupUI;
    UI_ShopPopup _shopPopupUI;
    UI_EditSettingPopup _editSettingPopupUI;
    UI_NotiPopup _uiNotiPopup;
    public UI_BuildAction _uI_BuildAction;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        _quickMenuPopupUI = Managers.UI.ShowPopupUI<UI_QuickMenu>();
        _checkOutPopupUI = Managers.UI.ShowPopupUI<UI_CheckOutPopup>();
        _shopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        _editSettingPopupUI = Managers.UI.ShowPopupUI<UI_EditSettingPopup>();
        _uI_BuildAction = Managers.UI.MakeSubItem<UI_BuildAction>();
        _uiNotiPopup = Managers.UI.ShowPopupUI<UI_NotiPopup>();

        _quickMenuPopupUI.gameObject.SetActive(false);
        _checkOutPopupUI.gameObject.SetActive(false);
        _shopPopupUI.gameObject.SetActive(false);
        _editSettingPopupUI.gameObject.SetActive(false);
        _uI_BuildAction.SetActive(false);

        _uiNotiPopup.gameObject.SetActive(false);

        GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>().spacing = UI_GROUP_SPACING;

        GetButton((int)Buttons.QuickButton).gameObject.BindEvent(OnClickQuickButton);
        GetButton((int)Buttons.QuickButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BuildButton).gameObject.BindEvent(OnClickBuildButton);
        GetButton((int)Buttons.BuildButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CheckOutButton).gameObject.BindEvent(OnClickCheckOutButton);
        GetButton((int)Buttons.CheckOutButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuestButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuestButton).gameObject.BindEvent(OnClickQuestButton);

        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickEditSettingButton);
        GetButton((int)Buttons.SettingButton).gameObject.GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.ShopButton).gameObject.BindEvent(OnClickShopButton);
        GetButton((int)Buttons.ShopButton).GetOrAddComponent<UI_ButtonAnimation>();


        GetButton((int)Buttons.NoticeButton).gameObject.BindEvent(OnClickNoticeButton);
        GetButton((int)Buttons.NoticeButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.EditPosButton).gameObject.SetActive(false);

        #region Action 추가
        Managers.Game.OnResourcesChagned += Refresh;
        Managers.Game.OnCharacterChanged += Refresh;
        Managers.Equipment.EquipInfoChanged += Refresh;

        Managers.Food.OnFoodAdded += AddFoodSlot;
        Managers.Food.OnFoodSold += RemoveFoodSlot;

        #endregion

        Refresh();

        return true;
    }



    private void Awake()
    {
        Init();
    }
    public void OnDestroy()
    {
        if (Managers.Game != null)
        {
            Managers.Game.OnResourcesChagned -= Refresh;
            Managers.Game.OnCharacterChanged -= Refresh;
            Managers.Food.OnFoodAdded -= AddFoodSlot;
            Managers.Food.OnFoodSold -= RemoveFoodSlot;
            Managers.Equipment.EquipInfoChanged -= Refresh;
        }
    }



    void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.CreatureCountText).text = Managers.Game._characters.Count.ToString();
        GetText((int)Texts.DiaText).text = Managers.Game.Dia.ToString();

        CheckNotify();
    }

    private void OnClickNoticeButton()
    {
        _uiNotiPopup.gameObject.SetActive(true);
    }

    private void OnClickEditSettingButton()
    {
        _editSettingPopupUI.gameObject.SetActive(true);
    }

    private void OnClickShopButton()
    {
        if (!QuestManager.Instance.IsUnlocked("Content_AnimalAdoption"))
        {
            Managers.UI.ShowToast("아직 열리지 않았습니다");
            return;
        }
        _shopPopupUI.gameObject.SetActive(true);
    }

    private void OnClickCheckOutButton()
    {
        if (_checkOutPopupUI == null)
        {
            Managers.Debug.LogError("_checkOutPopupUI가 없음", EDebugType.UI);
            return;
        }

        _checkOutPopupUI.SetInfo(Managers.Time.AttendanceDay);
        _checkOutPopupUI.gameObject.SetActive(true);

    }

    private void OnClickBuildButton()
    {
        Managers.UI.ShowPopupUI<UI_BuildPopup>();
        gameObject.SetActive(false);
    }

    private void OnClickQuickButton()
    {
        _quickMenuPopupUI.gameObject.SetActive(true);
    }


    #region Food

    public void ResetCookItem(Food food)
    {
        UI_FoodItem item = Managers.UI.MakeSubItem<UI_FoodItem>(GetObject((int)GameObjects.StorageObject).transform);
        item.SetInfo(food);
    }

    public void RemoveSlotAnimated(UI_FoodItem slot)
    {
        var layout = GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>();
        layout.enabled = false;

        Vector3 removedPos = slot.transform.localPosition;

        Managers.Resource.Destroy(slot.gameObject);

        StartCoroutine(AnimateForwardShift(removedPos));
    }


    //Slot �и��� �ִϸ��̼�
    IEnumerator AnimateForwardShift(Vector3 removedPosition)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in GetObject((int)GameObjects.StorageObject).transform)
            children.Add(child);

        foreach (var child in children)
        {
            if (child.localPosition.x > removedPosition.x)
            {
                Vector3 targetPos = child.localPosition;
                targetPos.x -= GetSlotWidth() + UI_GROUP_SPACING;
                child.DOLocalMoveX(targetPos.x, 0.3f).SetEase(Ease.OutQuad);
            }
        }

        yield return new WaitForSeconds(0.3f);

        // 레이아웃 강제 리빌드
        var layout = GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>();
        layout.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
    }

    //Slot �ʺ� ���ϴ� �Լ� 
    float GetSlotWidth()
    {
        GameObject obj = GetObject((int)GameObjects.StorageObject);
        if (obj.transform.childCount == 0) return 0f;

        return (obj.transform.GetChild(0) as RectTransform).rect.width;
    }

    void AddFoodSlot(Food food)
    {
        var layout = GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>();
        layout.enabled = false;

        UI_FoodItem item = Managers.UI.MakeSubItem<UI_FoodItem>(layout.transform);

        //임시로 화면 밖으로 보내기
        item.transform.localPosition = new Vector3(9999, 9999, 0);
        item.SetInfo(food);

        // LinkedList 순서 정리
        int index = Util.GetIndexInLinkedList(Managers.Food._foodList, food);
        item.transform.SetSiblingIndex(index);

        layout.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.transform as RectTransform);
    }

    void RemoveFoodSlot(Food food)
    {
        // 정확히 Food 객체를 비교해서 찾음
        foreach (Transform child in GetObject((int)GameObjects.StorageObject).transform)
        {
            var item = child.GetComponent<UI_FoodItem>();
            if (item != null && ReferenceEquals(item._food, food))
            {
                RemoveSlotAnimated(item);
                break;
            }
        }
    }

    #endregion

    #region Battle


    #endregion

    #region Quest
    private void OnClickQuestButton()
    {
        Managers.UI.ShowPopupUI<UI_QuestPopup>();
    }
    #endregion


    public void CheckNotify()
    {
        //장비, 캐릭터 업데이트된게 있으면 True
        if (Managers.Game.OwnedEquipments.Any(e => !e.IsConfirmed) || Managers.Game._characters.Any(c => !c.Value.IsConfirmed))
            GetObject((int)GameObjects.QuickNotifyObject).SetActive(true);
        else
            GetObject((int)GameObjects.QuickNotifyObject).SetActive(false);
    }



}
