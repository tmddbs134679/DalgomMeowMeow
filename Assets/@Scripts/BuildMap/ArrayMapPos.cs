using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

    public void EditorSaveMapTileData()
    {
        SaveMapTileData();
    }

    public void EditorLoadMapTileData()
    {
        LoadMapTileDataAsyncParallel();
    }

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

    public void SaveMapTileData()
    {
        MapTileSaveData saveData = new MapTileSaveData
        {
            width = this.width,
            height = this.height,
            rows = this.rows
        };

        string path = Path.Combine(Application.persistentDataPath, "MapTileData.json");
        string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
        File.WriteAllText(path, json);
        Debug.Log($"맵 저장 완료! 저장 경로: {path}");

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public async Task LoadMapTileDataAsyncParallel()
    {
        UI_LoadingMap ui = Managers.UI.ShowPopupUI<UI_LoadingMap>();
        ui.gameObject.SetActive(true);

        string path = Path.Combine(Application.persistentDataPath, "MapTileData.json");
        Debug.Log($"맵 타일 데이터 로드 시도 경로: {path}");

        if (!File.Exists(path))
        {
            Debug.LogWarning("저장된 맵 타일 데이터가 없습니다. 기본 타일 데이터를 복사합니다.");
            TextAsset baseData = Resources.Load<TextAsset>("Map/BaseMapTileData"); // Resources/Map/MapTileData.json

            if (baseData != null)
            {
                File.WriteAllText(path, baseData.text);
                Debug.Log($"기본 맵 타일 데이터를 저장했습니다: {path}");
            }
            else
            {
                Debug.LogError("기본 맵 타일 데이터(Resources)에서 로드 실패!");
                ui.gameObject.SetActive(false);
                return;
            }
        }

        try
        {
            string json = File.ReadAllText(path);
            MapTileSaveData saveData = JsonConvert.DeserializeObject<MapTileSaveData>(json);
            ApplyLoadedData(saveData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"맵 타일 데이터 로드 중 예외 발생: {ex}");
        }
        finally
        {
            ui.gameObject.SetActive(false);
        }
    }

    private void ApplyLoadedData(MapTileSaveData saveData)
    {
        this.width = saveData.width;
        this.height = saveData.height;
        this.rows = saveData.rows;
    }

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
            Debug.LogError("ArrayMapPos 이벤트 해제됨");
        }
    }
}

[Serializable]
public class TileRow
{
    public List<TileData> columns;
}

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
