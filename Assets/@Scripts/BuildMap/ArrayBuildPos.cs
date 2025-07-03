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
public void RemoveBuildData(BuildData buildData)
{
        float targetX = buildData.posX;
        float targetZ = buildData.posZ;
    BuildData dataToRemove = baseBuilding.Find(data => 
        math.abs(data.posX - targetX) < 0.01f && 
        math.abs(data.posZ - targetZ) < 0.01f);

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
//파일에서 건물데이터 불러오기
    public void BuildDataToFile()
    {
        baseBuilding.Clear();
        EditorUtility.SetDirty(this);
    }
#endif
}

[Serializable]
public class BuildData
{
    public int index;
    public float posX;
    public float posZ;

    public BaseBuildingSO testBaseBuilding;

}
