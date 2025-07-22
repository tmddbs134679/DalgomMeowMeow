using System.Collections.Generic;

[System.Serializable]
public class QuestSaveData
{
    public string QuestId;
    public int Progress;
    public QuestProgressState State;
    public List<string> unlockedContentIds = new();
}