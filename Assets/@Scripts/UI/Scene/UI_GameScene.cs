using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        DailyButton,
        ShopButton,
        BuildButton,
        EditPosButton,
        BattleButton
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


        Managers.Game.OnResourcesChagned += Refresh;
        Refresh();

        return true;
    }

    private void OnClickBattleButton()
    {
        Managers.Scene.LoadScene(EScene.Test_Battle);
    }

    private void Awake()
    {
        Init();
    }

    ////음식받는거 Test
    //private void Start()
    //{
    //    StartCoroutine(StartCookingLoop());
    //}

    ////Test용
    //IEnumerator StartCookingLoop()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(5f);
    //        ResetCookItem();
    //    }
    //}
    public void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChagned -= Refresh;
    }

    public void ResetCookItem()
    {
        Food food = new Food("F0001");
        Managers.Food.Enqueue(food);

        UI_FoodItem item = Managers.UI.MakeSubItem<UI_FoodItem>(GetObject((int)GameObjects.StorageObject).transform);
        item.SetInfo(food);
    }

    public void RemoveSlotAnimated(UI_FoodItem slot)
    {
        var layout = GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>();
        layout.enabled = false;

        Vector3 removedPos = slot.transform.localPosition;

        Destroy(slot.gameObject);

        StartCoroutine(AnimateForwardShift(removedPos));
    }


    //Slot 밀리는 애니메이션
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
                targetPos.x -= GetSlotWidth() + UI_GROUP_SPACING; // 제거된 슬롯 너비 + Spacing 크기
                child.DOLocalMoveX(targetPos.x, 0.3f).SetEase(Ease.OutQuad);
            }
        }

        yield return new WaitForSeconds(0.3f);

   
        GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>().enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    //Slot 너비 구하는 함수 
    float GetSlotWidth()
    {
        GameObject obj = GetObject((int)GameObjects.StorageObject);
        if (obj.transform.childCount == 0) return 0f;

        return (obj.transform.GetChild(0) as RectTransform).rect.width;
    }


     void Refresh()
    {
        GetText((int)Texts.PlayerGoldText).text = Managers.Game.Gold.ToString();
    }
}
