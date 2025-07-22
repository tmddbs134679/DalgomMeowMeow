using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChapterSlot : UI_Base
{
    enum Texts { QuestTitleText, }
    enum Images { CompleteCheckImage }
    private string _contentId;
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        return true;
    }
    public void SetCondition(UnlockCondition condition)
    {
        string title = "";
        string progress = "";
        Color CompleteCheckColor = Color.green;

        // ✅ 해금 여부 확인 (현재 콘텐츠가 이미 해금된 상태인지)
        bool isContentUnlocked = false;

        // 슬롯이 어떤 콘텐츠(챕터)에 해당하는지 상위에서 알려줘야 함
        if (!string.IsNullOrEmpty(_contentId))
            isContentUnlocked = QuestManager.Instance.IsUnlocked(_contentId);

        switch (condition.Type)
        {
            case UnlockConditionType.Gold:
                float currentGold = Managers.Game.Gold;
                title = $"골드 조건 {condition.RequiredGold}";
                progress = $"{currentGold} / {condition.RequiredGold}";

                if (!isContentUnlocked)
                    CompleteCheckColor = currentGold >= condition.RequiredGold ? Color.green : Color.red;
                break;

            case UnlockConditionType.Quest:
                bool isCompleted = QuestManager.Instance.IsQuestCompleted(condition.QuestId);

                if (!string.IsNullOrEmpty(condition.QuestId) &&
                    Managers.Data.QuestDataDic.TryGetValue(condition.QuestId, out var questData))
                    title = $"{questData.Title}";

                if (!isContentUnlocked)
                    CompleteCheckColor = isCompleted ? Color.green : Color.red;
                break;
        }

        GetText((int)Texts.QuestTitleText).text = title;
        GetImage((int)Images.CompleteCheckImage).color = CompleteCheckColor;
    }
    public void SetContentId(string contentId)
    {
        _contentId = contentId;
    }
}
