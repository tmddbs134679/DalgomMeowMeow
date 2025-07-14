using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    private Dictionary<string, Quest> _quests = new();
    private Dictionary<(Define.EQuestConditionType, Define.ETargetType), List<Quest>> _questIndex = new();

    public event Action OnQuestUpdated;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        
        
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => Managers.Data.QuestDataDic.Count > 0);

        InitQuest();
    }

    public void InitQuest()
    {
        foreach (var kvp in Managers.Data.QuestDataDic)
        {
            var data = kvp.Value;
            var quest = new Quest(data);
            _quests[data.QuestId] = quest;

            var key = (data.QuestConditionType, data.TargetType);
            if (!_questIndex.ContainsKey(key))
                _questIndex[key] = new List<Quest>();
            _questIndex[key].Add(quest);
        }
    }

    public void OnEvent(Define.EQuestConditionType condition, Define.ETargetType target)
    {
        var key = (condition, target);
        if (!_questIndex.TryGetValue(key, out var questList)) return;

        foreach (var quest in questList)
        {
            if (quest.State == QuestProgressState.InProgress)
            {
                quest.AddProgress();
            }
        }
        OnQuestUpdated?.Invoke(); // 이벤트 호출
    }

    public void GiveReward(string questId)
    {
        if (_quests.TryGetValue(questId, out var quest))
        {
            quest.Reward();
        }
    }
    
    public void TryActivateNext(string completedQuestId)
    {
        if (!_quests.TryGetValue(completedQuestId, out var completedQuest)) return;

        int carriedProgress = completedQuest.Progress;
        
        foreach (var quest in _quests.Values)
        {
            if (quest.State == QuestProgressState.NotStarted &&
                quest.QuestData.PreviousQuestID == completedQuestId)
            {
                quest.State = QuestProgressState.InProgress;
                quest.SetProgress(carriedProgress); //  누적 진행도 반영
                Debug.Log($"[퀘스트 시작] {quest.QuestData.Title} (진행도: {quest.Progress}/{quest.QuestData.GoalCount})");
            }
        }
    }
    
    public List<QuestSaveData> GetAllQuestSaveData()
    {
        List<QuestSaveData> saveList = new();
        foreach (var quest in _quests.Values)
        {
            saveList.Add(quest.ToSaveData());
        }
        return saveList;
    }
    
    public void LoadFromSaveData(List<QuestSaveData> saveDataList)
    {
        foreach (var saveData in saveDataList)
        {
            if (_quests.TryGetValue(saveData.QuestId, out var quest))
            {
                quest.LoadProgress(saveData);
            }
        }
    }
    
    public List<Quest> DailyQuests
    {
        get
        {
            List<Quest> list = new();
            foreach (var quest in _quests.Values)
            {
                if (quest.QuestData.QuestType == Define.EQuestType.Daily)
                    list.Add(quest);
            }
            return list;
        }
    }
    
    public List<Quest> AchievementQuests
    {
        get
        {
            List<Quest> list = new();
            foreach (var quest in _quests.Values)
            {
                if (quest.QuestData.QuestType == Define.EQuestType.Achievement)
                    list.Add(quest);
            }
            return list;
        }
    }
}