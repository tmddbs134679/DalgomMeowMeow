using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChapterSlot : UI_Base
{
    enum Texts { QuestTitleText, }
    enum Images { CompleteCheckImage }
    
    public override bool Init()
    {
        if (!base.Init()) return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        return true;
    }
    public void SetCondition(UnlockCondition condition,string contentId)
    {
        string title = "";
        Color CompleteCheckColor = Color.clear;

        // ✅ 이미 해금된 콘텐츠라면 무조건 초록색 처리
        bool isContentUnlocked = QuestManager.Instance.IsUnlocked(contentId);
        if (isContentUnlocked)
        {
            // 이미 해금된 콘텐츠이므로 상태를 초록색으로만 표시
            switch (condition.Type)
            {
                case UnlockConditionType.Gold:
                    title = $"누적 골드 {Managers.Game.MaxGold}/{condition.RequiredGold}";
                    break;

                case UnlockConditionType.Quest:
                    if (!string.IsNullOrEmpty(condition.QuestId) &&
                        Managers.Data.QuestDataDic.TryGetValue(condition.QuestId, out var questData))
                    {
                        title = $"{questData.Title} ";
                    }
                    else
                    {
                        title = "퀘스트 완료";
                    }
                    break;
            }

            CompleteCheckColor = Color.white;
        }
        else
        {
            // 아직 해금되지 않은 경우 → 조건 충족 여부에 따라 색상 결정
            switch (condition.Type)
            {
                case UnlockConditionType.Gold:
                    float currentGold = Managers.Game.Gold;
                    title = $"누적 골드  {Managers.Game.MaxGold}/{condition.RequiredGold}";
                    CompleteCheckColor = currentGold >= condition.RequiredGold ? Color.white : Color.clear;
                    break;

                case UnlockConditionType.Quest:
                    bool isCompleted = QuestManager.Instance.IsQuestCompleted(condition.QuestId);
                    if (!string.IsNullOrEmpty(condition.QuestId) &&
                        Managers.Data.QuestDataDic.TryGetValue(condition.QuestId, out var questData))
                    {
                        var quest = QuestManager.Instance.GetQuest(condition.QuestId);
                        title = $"{questData.Title} ({quest.Progress}/{quest.QuestData.GoalCount})";
                    }
                    else
                    {
                        title = "퀘스트 완료";
                    }

                    CompleteCheckColor = isCompleted ? Color.white : Color.clear;
                    break;
            }
        }

        GetText((int)Texts.QuestTitleText).text = title;
        GetImage((int)Images.CompleteCheckImage).color = CompleteCheckColor;
    }

}
