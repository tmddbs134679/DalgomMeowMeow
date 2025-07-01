using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPopup : UI_Popup
{
    QuestType _currentTab = QuestType.Daily;
    enum GameObjects
    {
        Content
    }

    enum Buttons
    {
        BackgroundButton,
        DailyButton,
        AchievementButton
    }

    public override bool Init()
    {
        if (!base.Init()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.DailyButton).gameObject.BindEvent(DailyButton);
        GetButton((int)Buttons.AchievementButton).gameObject.BindEvent(AchievementButton);
        
        QuestManager.Instance.OnQuestUpdated += RefreshUI;
        
        CreateQuestSlots();

        return true;
    }

    private void RefreshUI()
    {
        if (_currentTab == QuestType.Daily)
            CreateQuestSlots();
        else if (_currentTab == QuestType.Achievement)
            CreateAchievementSlots();
    }

    private void DailyButton()
    {
        _currentTab = QuestType.Daily;
        CreateQuestSlots();
    }

    private void AchievementButton()
    {
        _currentTab = QuestType.Achievement;
        CreateAchievementSlots();
    }

    private void OnClickBackgroundButton()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void CreateQuestSlots()
    {
        Transform parent = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);
        
        var allDaily = QuestManager.Instance.DailyQuests;

        // 1. 먼저 완료된 퀘스트부터
        foreach (var quest in allDaily)
        {
            if (quest.State == QuestProgressState.Completed)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
            }
        }

        // 2. 그다음 진행 중 퀘스트
        foreach (var quest in allDaily)
        {
            if (quest.State == QuestProgressState.InProgress)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
            }
        }
    }

    void CreateAchievementSlots()
    {
        Transform parent = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);

        foreach (var quest in QuestManager.Instance.AchievementQuests)
        {
            if (quest.State == QuestProgressState.InProgress)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
            }
        }
    }
}