using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private Dictionary<string, Quest> _quests = new();

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
            _quests[data.QuestId] = new Quest(data);
    }

    public void OnEvent(string conditionType, string targetId)
    {
        foreach (var quest in _quests.Values)
        {
            if (quest.State != QuestProgressState.InProgress) continue;
            if (quest.QuestData.Condition.ToString() != conditionType) continue;
            if (quest.QuestData.TargetType.ToString() != targetId) continue;

            quest.AddProgress();
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