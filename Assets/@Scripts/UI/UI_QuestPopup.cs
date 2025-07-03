using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuestPopup : UI_Popup
{
    QuestType _currentTab = QuestType.Daily;
    private List<UI_QuestSlot> _questSlots = new();

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

        QuestManager.Instance.OnQuestUpdated += RefreshProgressOnly;


        CreateQuestSlots();

        return true;
    }

    private void OnEnable()
    {
        QuestManager.Instance.OnQuestUpdated += RefreshProgressOnly;
    }

    private void OnDisable()
    {
        QuestManager.Instance.OnQuestUpdated -= RefreshProgressOnly;
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
        // 1. 기존 슬롯 제거
        foreach (var slot in _questSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }

        _questSlots.Clear(); // 기존 슬롯 목록 초기화

        var allDaily = QuestManager.Instance.DailyQuests;

        // 1. 먼저 완료된 퀘스트부터
        foreach (var quest in allDaily)
        {
            if (quest.State == QuestProgressState.Completed)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
                _questSlots.Add(slot);
            }
        }

        // 2. 그다음 진행 중 퀘스트
        foreach (var quest in allDaily)
        {
            if (quest.State == QuestProgressState.InProgress)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
                _questSlots.Add(slot);
            }
        }

        // 3. 보상 받은 퀘스트
        foreach (var quest in allDaily)
        {
            if (quest.State == QuestProgressState.Rewarded)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
                _questSlots.Add(slot);
            }
        }
    }

    public void CreateAchievementSlots()
    {
        Transform parent = GetObject((int)GameObjects.Content).transform;
        foreach (Transform child in parent)
            Destroy(child.gameObject);
        // 1. 기존 슬롯 제거
        foreach (var slot in _questSlots)
        {
            if (slot != null)
                Destroy(slot.gameObject);
        }

        _questSlots.Clear(); // 기존 슬롯 목록 초기화

        foreach (var quest in QuestManager.Instance.AchievementQuests)
        {
            if (quest.State == QuestProgressState.InProgress || quest.State == QuestProgressState.Completed)
            {
                UI_QuestSlot slot = Managers.UI.MakeSubItem<UI_QuestSlot>(parent);
                slot.SetQuest(quest);
                _questSlots.Add(slot);
            }
        }
    }

    private void RefreshProgressOnly()
    {
        _questSlots.RemoveAll(slot => slot == null || slot.gameObject == null);

        foreach (var slot in _questSlots)
        {
            if (slot != null && slot.gameObject != null)
                slot.UpdateProgressUI();
        }
    }
}