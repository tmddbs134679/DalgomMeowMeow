using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    
    private Dictionary<string, Quest> _quests = new();
    private Dictionary<(QuestConditionType, TargetType), List<Quest>> _questIndex = new();

    public List<QuestDataSO> questDataList;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        foreach (var data in questDataList)
        {
            var quest = new Quest(data);
            _quests[data.QuestId] = quest;

            var key = (data.Condition, data.TargetType);

            if (!_questIndex.ContainsKey(key))
                _questIndex[key] = new List<Quest>();

            _questIndex[key].Add(quest);
        }
    }

    public void OnEvent(QuestConditionType condition, TargetType target)
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
    }

    public void GiveReward(string questId)
    {
        if (_quests.TryGetValue(questId, out var quest))
        {
            quest.Reward();
        }
    }
}