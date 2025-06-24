using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Mathematics;


#if UNITY_EDITOR
using UnityEditor;
#endif
//rows 맵
//Width,Height 맵의 크기
//rows[0].columns.Count =>첫 행의 리스트 크기를 가져와 열의 전체크기를 반환
//rows?.Count ?? 0 => rows?.Count가  null일시 0반환
[CreateAssetMenu(menuName = "Map/TileMapData")]
public class ArrayMapPos : ScriptableObject
{
    public int width;
    public int height;
    public List<TileRow> rows;

    public int Width => rows?.Count > 0 ? rows[0].columns.Count : 0;
    public int Height => rows?.Count ?? 0;

    public TileData GetTile(int x, int y) => rows[y].columns[x];

    public void SetTile(TileData tileData, int x, int y)
    {
        rows[y].columns[x].isGround = tileData.isGround;
        rows[y].columns[x].isBuild = tileData.isBuild;
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
    public bool isGround;
    public bool isBuild;
}
