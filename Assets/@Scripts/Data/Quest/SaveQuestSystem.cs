using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveQuestSystem
{
    private static string SavePath => Application.persistentDataPath + "/questSave.json";

    public static void Save(List<QuestSaveData> data)
    {
        string json = JsonUtility.ToJson(new QuestSaveWrapper { Quests = data });
        File.WriteAllText(SavePath, json);
    }

    public static List<QuestSaveData> Load()
    {
        if (!File.Exists(SavePath)) return new List<QuestSaveData>();

        string json = File.ReadAllText(SavePath);
        var wrapper = JsonUtility.FromJson<QuestSaveWrapper>(json);
        return wrapper.Quests ?? new List<QuestSaveData>();
    }

    [System.Serializable]
    private class QuestSaveWrapper
    {
        public List<QuestSaveData> Quests;
    }
}