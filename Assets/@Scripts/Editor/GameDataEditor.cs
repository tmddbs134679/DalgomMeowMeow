using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System;
using UnityEngine.Rendering;

// 자동삭제 및 돈추가 기능 
public class GameDataEditor : EditorWindow
{
    private string _savePath;

    [MenuItem("Tools/Game Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameDataEditor>("Game Data Editor");
    }

    private void OnEnable()
    {
        _savePath = Application.persistentDataPath + "/SaveData.json";
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Data 관리", EditorStyles.boldLabel);
        GUILayout.Label($"_savePath : 저장 경로 :{_savePath}", EditorStyles.wordWrappedLabel);

        GUILayout.Space(10);

        if (GUILayout.Button("Gold + 10000")) 
        {
            AddGold();
        }

        if(GUILayout.Button("Data 삭제"))
        {
            DeleteSaveFile();
        }
    }

    private void DeleteSaveFile()
    {
        if(File.Exists(_savePath))
        {
            File.Delete(_savePath);
            Debug.Log("파일 삭제");
        }
        else
        {
            Debug.LogWarning("파일이 없음");
        }
    }

    private void AddGold()
    {
        if(!File.Exists(_savePath))
        {
            Debug.Log("파일이 없음");
            return;
        }

        string json = File.ReadAllText(_savePath);
        GameData data = JsonConvert.DeserializeObject<GameData>(json);
        data.Gold += 10000;
        File.WriteAllText(_savePath, JsonConvert.SerializeObject(data,formatting: Formatting.Indented));

        Debug.Log($"현재 Gold : {data.Gold}");
    }
}
