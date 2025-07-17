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
    
    private void CheckUnlockConditions(string completedQuestId)
    {
        foreach (var kvp in Managers.Data.UnlockContentDic)
        {
            foreach (var condition in kvp.Value.Conditions)
            {
                if (condition.Type == Data.UnlockConditionType.Quest &&
                    condition.QuestId == completedQuestId)
                {
                    TryUnlockContent(kvp.Key);
                    break;
                }
            }
        }
    }
    
    public void TryUnlockByGold()
    {
        foreach (var kvp in Managers.Data.UnlockContentDic)
        {
            string contentId = kvp.Key;
            var data = kvp.Value;

            // 골드 조건이 없으면 continue
            var goldCondition = data.Conditions.Find(cond => cond.Type == Data.UnlockConditionType.Gold);
            if (goldCondition == null)
                continue;

            // 골드가 충족되지 않았으면 continue
            if (Managers.Game.Gold < goldCondition.RequiredGold)
                continue;

            // 나머지 조건도 충족되는지 확인
            bool allMet = true;
            foreach (var condition in data.Conditions)
            {
                switch (condition.Type)
                {
                    case Data.UnlockConditionType.Quest:
                        if (!IsQuestCompleted(condition.QuestId))
                            allMet = false;
                        break;
                    case Data.UnlockConditionType.Gold:
                        if (Managers.Game.Gold < condition.RequiredGold)
                            allMet = false;
                        break;
                }

                if (!allMet) break;
            }

            if (allMet)
            {
                Unlock(contentId); // 해금 실행
            }
        }
    }
    
    public void TryUnlockContent(string contentId)
    {
        if (!Managers.Data.UnlockContentDic.TryGetValue(contentId, out var data)) return;

        bool allMet = true;
        foreach (var condition in data.Conditions)
        {
            switch (condition.Type)
            {
                case Data.UnlockConditionType.Quest:
                    if (!QuestManager.Instance.IsQuestCompleted(condition.QuestId))
                        allMet = false;
                    break;
                case Data.UnlockConditionType.Gold:
                    if (Managers.Game.Gold < condition.RequiredGold)
                        allMet = false;
                    break;
            }

            if (!allMet) break;
        }

        if (allMet)
        {
            Unlock(contentId);
        }
    }
    public bool IsQuestCompleted(string questId)
    {
        return _quests.TryGetValue(questId, out var quest) &&
               quest.State == QuestProgressState.Completed;
    }
    
    private void Unlock(string contentId)
    {
        Debug.Log($"[해금 완료] 콘텐츠: {contentId}");
        // TODO: 콘텐츠 타입별로 처리 (ex: 건물, 지역, 캐릭터 등)
        // ex: Managers.Building.Unlock(contentId);

        if (contentId.StartsWith("Building_"))
        {
            
        }
        // 저장할 경우 리스트에 추가
        // Managers.Game.UnlockedContent.Add(contentId);
    }
}