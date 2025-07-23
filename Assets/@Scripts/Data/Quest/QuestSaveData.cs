using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public Dictionary<string, QuestRecord> questRecords = new();
    public List<string> unlockedContentIds = new();
}

[System.Serializable]
public class QuestRecord
{
    public int Progress;
    public QuestProgressState State;
}
