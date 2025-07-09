using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBuildingLevel : MonoBehaviour
{
    public void SaveGame()
    {
        GameSaveData saveData = new();

        var allBuildings = GameObject.FindObjectsOfType<BuildingBase>();
        foreach (var building in allBuildings)
        {

        }

        string json = JsonUtility.ToJson(saveData, true);
        PlayerPrefs.SetString("GameSave", json);
    }
    
    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("GameSave")) return;

        string json = PlayerPrefs.GetString("GameSave");
        GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);

        foreach (var data in saveData.Buildings)
        {
            // BuildingId에 맞는 프리팹 로드 후 Instantiate
            GameObject prefab = Managers.Resource.Load<GameObject>($"Prefabs/Buildings/{data.BuildingId}");
      
        }
    }
}
