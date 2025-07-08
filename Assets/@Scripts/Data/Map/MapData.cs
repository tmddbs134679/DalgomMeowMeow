using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapData : MonoBehaviour
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "Map.json");

    public static void Save(ForestSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static ForestSaveData Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<ForestSaveData>(json);
        }

        return new ForestSaveData();
    }
}
