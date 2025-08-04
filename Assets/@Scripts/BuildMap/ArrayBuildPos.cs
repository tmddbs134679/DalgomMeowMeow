using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Map/TileBuildData")]
public class ArrayBuildPos : ScriptableObject
{
    public List<BuildData> baseBuilding;

    public void GetBuildData(BuildData buildData)
    {
        if (baseBuilding == null)
            baseBuilding = new List<BuildData>();

        if (!baseBuilding.Any(b => b.UniqueId == buildData.UniqueId))
        {
            baseBuilding.Add(buildData);
        }
        else
        {
            Debug.LogWarning($"중복된 UniqueId: {buildData.UniqueId} - 이미 추가됨");
        }
    }

#if UNITY_EDITOR
    public void EditorOnly_SaveAsset()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif

    public void RemoveBuildData(BuildData buildData)
    {
        BuildData dataToRemove = baseBuilding.Find(data => data.UniqueId == buildData.UniqueId);
        if (dataToRemove != null)
            baseBuilding.Remove(dataToRemove);
    }

    public void RemoveStageData(BuildData buildData)
    {
        BuildData dataToRemove = baseBuilding.Find(data => data.UnlockId == buildData.UnlockId);
        if (dataToRemove != null)
            baseBuilding.Remove(dataToRemove);
    }

#if UNITY_EDITOR
    public void InitializeBuild()
    {
        baseBuilding.Clear();
        EditorUtility.SetDirty(this);
    }

    public void EditorSaveMapData()
    {
        SaveMapData();
    }

    public void EditorLoadMapData()
    {
        LoadMapDataAsyncParallel();
    }

    public void LoadProtoTypeMapData()
    {
        TextAsset baseData = Resources.Load<TextAsset>("Map/BaseMapData");
        if (baseData == null)
        {
            Debug.LogError("BaseMapData 리소스가 없습니다!");
            return;
        }

        string json = baseData.text;
        MapSaveData saveData = JsonConvert.DeserializeObject<MapSaveData>(json);
        List<BuildData> buildDataList = new();

        foreach (var data in saveData.buildings)
        {
            string sopath = $"Assets/@Scripts/BuildMap/ScriptableOBJ/BuildSO/{data.buildingName}.asset";
            BaseBuildingSO so = AssetDatabase.LoadAssetAtPath<BaseBuildingSO>(sopath);
            if (so == null)
            {
                Debug.LogError($"ScriptableObject 로드 실패: {sopath}");
                continue;
            }

            buildDataList.Add(new BuildData
            {
                posX = data.posX,
                posZ = data.posZ,
                testBaseBuilding = so,
                UnlockId = data.UnlockId,
                UniqueId = data.UniqueId,
                LV = data.LV,
            });
        }

        baseBuilding = buildDataList;
        Debug.Log("프로토타입 맵 데이터 로드 완료!");
    }
#endif

    private string GetSavePath() => Path.Combine(Application.persistentDataPath, "MapData.json");

    public void SaveMapData()
    {
        MapSaveData saveData = new();

        foreach (var data in baseBuilding)
        {
            saveData.buildings.Add(new BuildData
            {
                posX = data.posX,
                posZ = data.posZ,
                buildingName = data.testBaseBuilding.name,
                UnlockId = data.UnlockId,
                UniqueId = data.UniqueId,
                LV = data.LV,
            });
        }

        string path = GetSavePath();
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(path, json);
        Debug.Log($"맵 저장 완료: {path}");
    }

    public async Task LoadMapDataAsyncParallel()
    {
        UI_LodingMap ui = Managers.UI.ShowPopupUI<UI_LodingMap>();
        ui.gameObject.SetActive(true);

        string path = GetSavePath();
        if (!File.Exists(path))
        {
            Debug.LogWarning("저장된 맵 데이터가 없습니다.");
            ui.gameObject.SetActive(false);
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            MapSaveData saveData = JsonConvert.DeserializeObject<MapSaveData>(json);
            int totalCount = saveData.buildings.Count;
            int loadedCount = 0;

            var loadTasks = saveData.buildings.Select(async data =>
            {
                string key = data.buildingName;
                var handle = Addressables.LoadAssetAsync<BaseBuildingSO>(key);
                await handle.Task;

                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    BaseBuildingSO so = handle.Result;
                    var buildData = new BuildData
                    {
                        posX = data.posX,
                        posZ = data.posZ,
                        testBaseBuilding = so,
                        UnlockId = data.UnlockId,
                        UniqueId = data.UniqueId,
                        LV = data.LV,
                    };
                    Addressables.Release(handle);
                    return buildData;
                }
                else
                {
                    Debug.LogError($"Addressables 로드 실패: {key}");
                    Addressables.Release(handle);
                    return null;
                }
            }).ToList();

            var results = await Task.WhenAll(loadTasks);
            baseBuilding = results.Where(r => r != null).ToList();
            Managers.AI.AllRelocateToNearestNavMesh();
            Debug.Log("맵 로드 완료!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"맵 데이터 로드 중 예외 발생: {ex}");
        }
        finally
        {
            ui.gameObject.SetActive(false);
        }
    }

    public void BindEvent()
    {
        if (BuildingPlacer.Instance != null)
            BuildingPlacer.Instance.OnAutoSave += SaveMapData;
    }

    public void UnBindEvent()
    {
        if (BuildingPlacer.Instance != null)
        {
            BuildingPlacer.Instance.OnAutoSave -= SaveMapData;
            Debug.LogError("ArrayBuildPos 이벤트 해제됨");
        }
    }
}

[Serializable]
public class BuildData
{
    public float posX;
    public float posZ;
    public string buildingName;
    public int UnlockId;
    public BaseBuildingSO testBaseBuilding;
    public int LV;
    public int UniqueId;
}

[Serializable]
public class MapSaveData
{
    public List<BuildData> buildings = new List<BuildData>();
}
