using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    enum Texts
    {
        PlayerGoldText,
        CreatureCountText,

    }
    #endregion

    private const int GROUP_SPACING = 40;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.StorageObject).GetComponent<HorizontalLayoutGroup>().spacing = GROUP_SPACING;    
        return true;
    }
    private void Awake()
    {
        Init();
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
                targetPos.x -= GetSlotWidth() + GROUP_SPACING; // 또는 제거된 슬롯 너비
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


}
