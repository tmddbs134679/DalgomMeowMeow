using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveQuestSystem
{
    private static string SavePath => Application.persistentDataPath + "/questSave.json";

    public static void Save(QuestSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SavePath, json);
    }

    public static QuestSaveData Load()
    {
        if (!File.Exists(SavePath)) return new QuestSaveData();

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<QuestSaveData>(json);
    }
}