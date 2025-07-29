using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public List<QuestRecordWrapper> questRecords = new();
    public List<string> unlockedContentIds = new();
}

[System.Serializable]
public class QuestRecord
{
    public int Progress;
    public QuestProgressState State;
}
[System.Serializable]
public class QuestRecordWrapper
{
    public string QuestId;
    public int Progress;
    public QuestProgressState State;
}
