using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
        StorageObject
    }

    enum Buttons
    {
        MailButton,
        NoticeButton,
        SettingButton,
        QuestButton,
        ArchivementButton,
        QuickButton,
        ShopButton,
        BuildButton,
        EditPosButton,
        BattleButton,
    }

    enum Texts
    {
        PlayerGoldText,
        CreatureCountText,
    }
    #endregion



    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>().spacing = UI_GROUP_SPACING;
        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(OnClickBattleButton);
        GetButton((int)Buttons.BattleButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuickButton).gameObject.BindEvent(OnClickQuickButton);
        GetButton((int)Buttons.BuildButton).gameObject.BindEvent(OnClickBuildButton);
        GetButton((int)Buttons.BuildButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.QuestButton).gameObject.BindEvent(OnClickQuestButton);

        Managers.Game.OnResourcesChagned += Refresh;
        Managers.Game.OnCharacterChanged += Refresh;
        Managers.Food.OnFoodAdded += AddFoodSlot;
        Managers.Food.OnFoodSold += RemoveFoodSlot;

        Refresh();

        return true;
    }

    private void OnClickBuildButton()
    {
        Managers.UI.ShowPopupUI<UI_BuildPopup>();
        gameObject.SetActive(false);
    }

    private void OnClickQuickButton()
    {
        Managers.UI.ShowPopupUI<UI_QuickMenu>();
    }

    private void Awake()
    {
        Init();
    }
    public void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChagned -= Refresh;
    }

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
        //Destroy(slot.gameObject);

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
                targetPos.x -= GetSlotWidth() + UI_GROUP_SPACING; // ���ŵ� ���� �ʺ� + Spacing ũ��
                child.DOLocalMoveX(targetPos.x, 0.3f).SetEase(Ease.OutQuad);
            }
        }

        yield return new WaitForSeconds(0.3f);


        GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    //Slot �ʺ� ���ϴ� �Լ� 
    float GetSlotWidth()
    {
        GameObject obj = GetObject((int)GameObjects.StorageObject);
        if (obj.transform.childCount == 0) return 0f;

        return (obj.transform.GetChild(0) as RectTransform).rect.width;
    }


    void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
        GetText((int)Texts.CreatureCountText).text = Managers.Game.Characters.Count.ToString();
    }

    void AddFoodSlot(Food food)
    {
        UI_FoodItem item = Managers.UI.MakeSubItem<UI_FoodItem>(GetObject((int)GameObjects.StorageObject).transform);
        item.SetInfo(food);
    }

    void RemoveFoodSlot(Food food)
    {
        // ������� �ڽ� �� �ش� ������ ���� ������ ã�Ƽ� �ִϸ��̼� ����
        foreach (Transform child in GetObject((int)GameObjects.StorageObject).transform)
        {
            var item = child.GetComponent<UI_FoodItem>();
            if (item != null && item._food == food)
            {
                RemoveSlotAnimated(item);
                break;
            }
        }
    }


    #region Battle
    private void OnClickBattleButton()
    {
        Managers.Scene.LoadScene(EScene.Test_Battle);
    }

    #endregion

    #region Quest
    private void OnClickQuestButton()
    {
        Managers.UI.ShowPopupUI<UI_QuestPopup>();
    }
    #endregion

}
