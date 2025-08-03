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
[CreateAssetMenu(menuName = "Map/TileMapData")]
public class ArrayMapPos : ScriptableObject
{
    public int width;
    public int height;
    public List<TileRow> rows;

    public int Width => rows?.Count > 0 ? rows[0].columns.Count : 0;
    public int Height => rows?.Count ?? 0;

    public TileData GetTile(int x, int y) => rows[y].columns[x];
    public void SetTile(bool isbuild, int x, int y)
    {
        rows[y].columns[x].isNotBuild = isbuild;
    }

#if UNITY_EDITOR
    public void InitializeMap()
    {
        rows = new List<TileRow>();
        for (int y = 0; y < height; y++)
        {
            TileRow row = new TileRow();
            row.columns = new List<TileData>();
            for (int x = 0; x < width; x++)
            {
                row.columns.Add(new TileData());
            }
            rows.Add(row);
        }

        EditorUtility.SetDirty(this);
    }
#endif

#if UNITY_EDITOR
    public void EditorSaveMapTileData()
    {
        SaveMapTileData();
    }
#endif
  public void SaveMapTileData()
{
#if UNITY_EDITOR
    MapTileSaveData saveData = new MapTileSaveData
    {
        width = this.width,
        height = this.height,
        rows = this.rows
    };

    string path = $"{Application.dataPath}/Resources/MapTileData.json";
    string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
    File.WriteAllText(path, json);
    Debug.Log("맵 저장 완료!");
#endif
}


public void LoadMapTileData()
{
    TextAsset mapTileData = Resources.Load<TextAsset>("MapTileData"); // "Resources/MapTileData.json"이 실제 경로
    if (mapTileData == null)
    {
        Debug.LogError("MapTileData 리소스가 없습니다!");
        return;
    }

    string json = mapTileData.text;
    MapTileSaveData saveData = JsonConvert.DeserializeObject<MapTileSaveData>(json);

    this.width = saveData.width;
    this.height = saveData.height;
    this.rows = saveData.rows;

#if UNITY_EDITOR
    EditorUtility.SetDirty(this);
#endif
}


#if UNITY_EDITOR
public void LoadProtoTypeMapTileData()
{
    TextAsset mapTileData = Resources.Load<TextAsset>("Map/BaseMapTileData"); // Resources/Map/BaseMapTileData.json
    if (mapTileData == null)
    {
        Debug.LogError("BaseMapTileData 리소스가 없습니다!");
        return;
    }

    string json = mapTileData.text;
    MapTileSaveData saveData = JsonConvert.DeserializeObject<MapTileSaveData>(json);

    this.width = saveData.width;
    this.height = saveData.height;
    this.rows = saveData.rows;

    Debug.Log("베이스 맵 타일 데이터 로드 완료!");
    EditorUtility.SetDirty(this);
}
#endif




    public void BindEvent()
    {
        if (BuildingPlacer.Instance != null)
        {
            BuildingPlacer.Instance.OnAutoSave += SaveMapTileData;

        }
    }

    public void UnBindEvent()
    {
        if (BuildingPlacer.Instance != null)
        {
            BuildingPlacer.Instance.OnAutoSave -= SaveMapTileData;
                                    Debug.LogError("arraymappos해제됨");
        }
    }
}

//TileData->TileRow
[Serializable]
public class TileRow
{
    public List<TileData> columns;
}
//isGround,isbuild->TileData
[Serializable]
public class TileData
{
    public bool isNotGround;
    public bool isNotBuild;
}


[Serializable]
public class MapTileSaveData
{
    public int width;
    public int height;
    public List<TileRow> rows;
}