using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using System.IO;
using Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif
//rows 맵
//Width,Height 맵의 크기
//rows[0].columns.Count =>첫 행의 리스트 크기를 가져와 열의 전체크기를 반환
//rows?.Count ?? 0 => rows?.Count가  null일시 0반환

/// <summary>
/// 맵 저장 데이터
/// </summary>
[CreateAssetMenu(menuName = "Map/TileBuildData")]
public class ArrayBuildPos : ScriptableObject
{
    public List<BuildData> baseBuilding;

    public void GetBuildData(BuildData buildData)
    {
        baseBuilding.Add(buildData);
    }
    public void RemoveBuildData(BuildData buildData)
    {
        float targetX = buildData.posX;
        float targetZ = buildData.posZ;
        BuildData dataToRemove = baseBuilding.Find(data =>
        data.UniqueId == buildData.UniqueId);

        if (dataToRemove != null)
        {
            baseBuilding.Remove(dataToRemove);
        }
    }

    public void RemoveStageData(BuildData buildData)
    {
        float targetX = buildData.posX;
        float targetZ = buildData.posZ;
        BuildData dataToRemove = baseBuilding.Find(data =>
        data.UnlockId == buildData.UnlockId);

        if (dataToRemove != null)
        {
            baseBuilding.Remove(dataToRemove);
        }
    }

#if UNITY_EDITOR
    public void InitializeBuild()
    {
        baseBuilding.Clear();
        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    //파일에서 건물데이터 저장하기
    public void EditorSaveMapData()
    {
        SaveMapData();
    }
#endif

    //save이벤트
    public void SaveMapData()
    {
        string path = $"{Application.dataPath}/@Resources/Map/MapData.json";
        MapSaveData saveData = new MapSaveData();

        foreach (var data in baseBuilding)
        {
            saveData.buildings.Add(new BuildData
            {
                posX = data.posX,
                posZ = data.posZ,
                buildingName = data.testBaseBuilding.name, // 오브젝트 자체의 이름만 저장
                UnlockId = data.UnlockId,
                UniqueId = data.UniqueId,
                LV = data.LV,
            });
        }

        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(path, json);
    }


#if UNITY_EDITOR
    //파일에서 건물데이터 불러오기
    public void EditorLoadMapData()
    {
        LoadMapData();
    }
#endif

 public void LoadMapData()
    {
        string path = $"{Application.dataPath}/@Resources/Map/MapData.json";

        if (!File.Exists(path))
        {
            Debug.LogError("건물 데이터 파일이 없습니다!");
            return;
        }

        string json = File.ReadAllText(path);
        MapSaveData saveData = JsonConvert.DeserializeObject<MapSaveData>(json);

        List<BuildData> buildDataList = new List<BuildData>();

        foreach (var data in saveData.buildings)
        {
            string sopath = $"Assets/@Scripts/BuildMap/ScriptableOBJ/BuildSO/{data.buildingName}.asset";
            if (!File.Exists(sopath))
            {
                Debug.LogError("건물 So 파일이 없습니다!");
                return;
            }
            BaseBuildingSO so = AssetDatabase.LoadAssetAtPath<BaseBuildingSO>(sopath);
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
        Debug.Log("건물 데이터 불러오기 완료!");
    }

#if UNITY_EDITOR
    //파일에서 건물데이터 불러오기
    public void LoadProtoTypeMapData()
    {
        string path = $"{Application.dataPath}/@Resources/Map/BaseMapData.json";

        if (!File.Exists(path))
        {
            Debug.LogError("건물 데이터 파일이 없습니다!");
            return;
        }

        string json = File.ReadAllText(path);
        MapSaveData saveData = JsonConvert.DeserializeObject<MapSaveData>(json);

        List<BuildData> buildDataList = new List<BuildData>();

        foreach (var data in saveData.buildings)
        {
            string sopath = $"Assets/@Scripts/BuildMap/ScriptableOBJ/BuildSO/{data.buildingName}.asset";
            if (!File.Exists(sopath))
            {
                Debug.LogError("건물 So 파일이 없습니다!");
                return;
            }
            BaseBuildingSO so = AssetDatabase.LoadAssetAtPath<BaseBuildingSO>(sopath);
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
        Debug.Log("건물 데이터 불러오기 완료!");
    }
#endif



    public void BindEvent()
    {
        if (BuildingPlacer.Instance != null)
        {
            BuildingPlacer.Instance.OnAutoSave += SaveMapData;

        }
    }

    public void UnBindEvent()
    {        if (BuildingPlacer.Instance != null)
        {
            BuildingPlacer.Instance.OnAutoSave -= SaveMapData;
                                                Debug.LogError("arraybuildpos해제됨");
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

    //레벨
    public int LV;
    //고유ID
    public int UniqueId;
}

//유니티 관련 오브젝트를 따로 저장할수는 없기 때문에 필요한 부분들만 따로 빼내서 저장하는걸 채택
[Serializable]
public class MapSaveData
{
    public List<BuildData> buildings = new List<BuildData>();
}


