using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Map/TileMapData")]
public class ArrayMapPos : ScriptableObject
{
    public List<TileRow> rows;

    public int Width => rows?.Count > 0 ? rows[0].tiles.Count : 0;
    public int Height => rows?.Count ?? 0;

    public TileData GetTile(int x, int y) => rows[y].tiles[x];
}

[Serializable]
public class TileRow
{
    public List<TileData> tiles;
}

[Serializable]
public struct TileData
{
    public bool isGround;
    public bool isBuild;
}
