using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    private Dictionary<string, Quest> _quests = new();
    private Dictionary<(Define.EQuestConditionType, Define.ETargetType), List<Quest>> _questIndex = new();

    public int CheckCountNotity;
    public bool CheapterNotify;
    public Action OnQuestUpdated;

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
                
                // ✅ 퀘스트 목표에 도달했으면 즉시 콘텐츠 해금 조건 검사
                if (quest.State == QuestProgressState.Completed)
                {
                    CheckUnlockConditions(quest.QuestData.QuestId);
                    TryActivateNext(quest.QuestData.QuestId); // 다음 퀘스트도 활성화
                }
                
                CheckChapterUnlock(quest.QuestData.QuestId);
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
            }
        }
    }
    
    public QuestSaveData GetAllQuestSaveData()
    {
        var data = new QuestSaveData();

        foreach (var pair in _quests)
        {
            data.questRecords.Add(new QuestRecordWrapper
            {
                QuestId = pair.Key,
                Progress = pair.Value.Progress,
                State = pair.Value.State
            });
        }

        data.unlockedContentIds = UnlockedContent.ToList(); // 저장
        return data;
    }
    

    public void LoadFromSaveData(QuestSaveData data)
    {
        foreach (var record in data.questRecords)
        {
            if (_quests.TryGetValue(record.QuestId, out var quest))
            {
                quest.Progress = record.Progress;
                quest.State = record.State;
            }
        }

        UnlockedContent = new HashSet<string>(data.unlockedContentIds); 
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
    
    // 챕터 언락 조건 체크
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

            //TryUnlockContent(kvp.Key); // 모든 조건 재검사
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
                        if (!IsQuestCompleted(condition.QuestId))
                            allMet = false;
                        break;
                    case Data.UnlockConditionType.Gold:
                        bool goldEnough = Managers.Game.Gold >= condition.RequiredGold;
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
            Managers.Game.IncreaseMaxCountInScene++;
            Managers.UI.ShowToast("해금완료");
        }
    }
    public bool IsQuestCompleted(string questId)
    {
        if (!_quests.TryGetValue(questId, out var quest) || quest == null)
            return false;

        return quest.State == QuestProgressState.Completed || quest.State == QuestProgressState.Rewarded;
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
    public HashSet<string> UnlockedContent = new(); // 콘텐츠 ID 저장

    public bool IsUnlocked(string contentId)
    {
        return UnlockedContent.Contains(contentId);
    }

    public void Unlock(string contentId)
    {

        if (!UnlockedContent.Contains(contentId))
        {
            UnlockedContent.Add(contentId);

            
            // 🟢 콘텐츠 목록에 연결된 추가 해금 항목도 처리
            if (Managers.Data.UnlockContentDic.TryGetValue(contentId, out var unlockData))
            {
                foreach (var item in unlockData.Items)
                {
                    if (!string.IsNullOrEmpty(item.Id))
                        Unlock(item.Id); // 재귀 호출로 실제 콘텐츠들 해금
                }
            }
            UI_BuildPopup buildPopup = FindObjectOfType<UI_BuildPopup>();
            UI_FarmPopup FarmPopup = FindObjectOfType<UI_FarmPopup>();
            FarmPopup?.UpdateButtonStates();
            buildPopup?.UpdateButtonStates();
        }
    }
    
    // 챕터 언락 조건 체크
    public void CheckChapterUnlock(string contentId)
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
        }
        if (allMet)
        {
            // 해금조건달성 알림 
            CheapterNotify = true;
        }

    }
}