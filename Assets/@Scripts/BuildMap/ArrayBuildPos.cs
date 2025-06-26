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
}

[Serializable]
public class BuildData
{
    public float posX;
    public float posY;

    public TestBaseBuilding testBaseBuilding;

}
