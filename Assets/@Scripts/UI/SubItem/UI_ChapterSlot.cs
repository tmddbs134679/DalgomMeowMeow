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
    public void SetCondition(UnlockCondition condition)
    {
        string title = "";
        string progress = "";
        Color CompleteCheckColor = Color.green;

        switch (condition.Type)
        {
            case UnlockConditionType.Gold:
                float currentGold = Managers.Game.Gold;
                title = $"골드 조건 {condition.RequiredGold}";
                progress = $"{currentGold} / {condition.RequiredGold}";
                break;

            case UnlockConditionType.Quest:
                bool isCompleted = QuestManager.Instance.IsQuestCompleted(condition.QuestId);
                if (!string.IsNullOrEmpty(condition.QuestId) &&
                    Managers.Data.QuestDataDic.TryGetValue(condition.QuestId, out var questData))
                    title = $"{questData.Title}";
                CompleteCheckColor = isCompleted ? Color.green : Color.red;
                break;
        }

        GetText((int)Texts.QuestTitleText).text = title;
        GetImage((int)Images.CompleteCheckImage).color = CompleteCheckColor;
    }
}
