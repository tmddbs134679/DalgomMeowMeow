using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestSlot : UI_Base
{
    enum Texts { QuestTitleText, ProgressText }
    enum Buttons { RewardButton }

    Quest _quest;

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.RewardButton).gameObject.BindEvent(OnClickRewardButton);
        return true;
    }

    public void SetQuest(Quest quest)
    {
        Init();

        _quest = quest;
        GetText((int)Texts.QuestTitleText).text = quest.QuestData.Title;
        GetText((int)Texts.ProgressText).text = $"{quest.Progress}/{quest.QuestData.GoalCount}";
        

        bool canClaim = quest.State == QuestProgressState.Completed;
        GetButton((int)Buttons.RewardButton).interactable = canClaim;
    }

    void OnClickRewardButton()
    {
        if (_quest.State == QuestProgressState.Completed)
        {
            QuestManager.Instance.GiveReward(_quest.QuestData.QuestId);
            
            if (_quest.QuestData.Type == QuestType.Daily)
            {
                // Daily 퀘스트는 클릭 시 맨 아래로 이동
                transform.SetAsLastSibling();
            }

            SetQuest(_quest); // UI 갱신
        }
    }
}

