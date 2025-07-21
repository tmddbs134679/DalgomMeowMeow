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
        var saveData = SaveQuestSystem.Load();
        QuestManager.Instance.LoadFromSaveData(saveData);
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
    

    public void UpdateQuestProgress(Define.EQuestConditionType condition, Define.ETargetType target)
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
            CheckUnlockConditions(questId);
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
            var data = kvp.Value;

            // 이 콘텐츠에 퀘스트 조건이 1개라도 포함되어 있는 경우만 검사
            bool hasRelatedQuest = data.Conditions.Exists(c =>
                c.Type == Data.UnlockConditionType.Quest && c.QuestId == completedQuestId);

            if (!hasRelatedQuest)
                continue;

            TryUnlockContent(kvp.Key); // 모든 조건 재검사
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
                        bool questCompleted = IsQuestCompleted(condition.QuestId);
                        Debug.Log($"[해금검사:{contentId}] 퀘스트 {condition.QuestId} 완료 여부: {questCompleted}");
                        if (!IsQuestCompleted(condition.QuestId))
                            allMet = false;
                        break;
                    case Data.UnlockConditionType.Gold:
                        bool goldEnough = Managers.Game.Gold >= condition.RequiredGold;
                        Debug.Log($"[해금검사:{contentId}] 골드 {condition.RequiredGold} 필요, 현재 {Managers.Game.Gold} → 충족: {goldEnough}");
                        if (Managers.Game.Gold < condition.RequiredGold)
                            allMet = false;
                        break;
                }

                if (!allMet) break;
            }

            if (allMet)
            {
                Debug.Log($"[해금 검사] 콘텐츠: {contentId}");
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
            // Debug.Log($"[해금 조건 달성] 콘텐츠: {contentId}");
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
        // TODO: 콘텐츠 타입별로 처리 (ex: 건물, 지역, 캐릭터 등)
        // ex: Managers.Building.Unlock(contentId);

        if (contentId.StartsWith("Building_"))
        {
            
        }
        // 저장할 경우 리스트에 추가
        // Managers.Game.UnlockedContent.Add(contentId);
    }
    
    public void NotifyBuildingConstructed(string type)
    {
        // string → ETargetType 변환
        if (!Enum.TryParse(type, out Define.ETargetType targetType))
        {
            Debug.LogWarning($"[퀘스트] 알 수 없는 건물 타입: {type}");
            return;
        }

        UpdateQuestProgress(Define.EQuestConditionType.Build, targetType);
    }
    
    private Define.ETargetType ConvertToTargetType(Define.EBuildingType type)
    {
        return type switch
        {
            Define.EBuildingType.OnionFarm => Define.ETargetType.Onion,
            Define.EBuildingType.PotatoFarm => Define.ETargetType.Potato,
            Define.EBuildingType.CabbageFarm => Define.ETargetType.Cabbage,
            Define.EBuildingType.CarrotFarm => Define.ETargetType.Carrot,
            Define.EBuildingType.PumpkinFarm => Define.ETargetType.Pumpkin,
            _ => Define.ETargetType.None
        };
    }
}