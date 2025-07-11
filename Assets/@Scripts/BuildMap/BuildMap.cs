using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;
using System;
/// <summary>
/// 저장되어있는 데이터맵 가져와서 그리드맵 생성및 타일에 정보전달
/// 현재까지는 remove,Collider ON,Off를 하기 위한 장치
/// </summary>
public class BuildMap : MonoBehaviour
{
    public ArrayBuildPos arrayBuildPos;

    public NavMeshSurface surface;
    private Dictionary<Vector2Int, GameObject> _spawnedBuilds = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<String, int> valueCounts = new Dictionary<string, int>();
    public Dictionary<String, int> filtervalueCounts = new Dictionary<string, int>();
    private Dictionary<int, BuildData> _buildDataMap = new Dictionary<int, BuildData>();
    void Start()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            _buildDataMap[data.UniqueId] = data;
            GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
            go.name = data.testBaseBuilding.BuildingType.ToString();

            if (go.GetComponent<BuildingBase>() != null)
            {
                var buildingBase = go.GetComponent<BuildingBase>();
                buildingBase.SetUniqueId(data.UniqueId);
                buildingBase.SetLevel(data.LV);
                buildingBase.SetBuildMap(this);
            }

            go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();
            if (go.TryGetComponent(out ForestRegion region))
            {
                region.Id = data.UnlockId;
            }
            _spawnedBuilds.Add(GridKey(data.posX, data.posZ), go);
        }

        valueCounts = _spawnedBuilds.Values
                          .GroupBy(v => v.name)
                          .ToDictionary(g => g.Key, g => g.Count());

        var excludeNames = new List<string> { "Road" };

        filtervalueCounts = _spawnedBuilds.Values
        .Where(v => !excludeNames.Contains(v.name))
                          .GroupBy(v => v.name)
                          .ToDictionary(g => g.Key, g => g.Count());

        surface.BuildNavMesh();
    }

    public void LoadBuild()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2Int key = GridKey(data.posX, data.posZ);

            if (_spawnedBuilds.TryGetValue(key, out var build))
            {
                build.transform.position = new Vector3(data.posX, 1f, data.posZ);
            }
            else
            {
                // 건설 후 추가 생성
                GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
                go.name = data.testBaseBuilding.BuildingType.ToString();
                _buildDataMap[data.UniqueId] = data;
                if (go.GetComponent<BuildingBase>() != null)
                {
                    var buildingBase = go.GetComponent<BuildingBase>();
                    buildingBase.SetUniqueId(data.UniqueId);
                    buildingBase.SetLevel(data.LV);
                    buildingBase.SetBuildMap(this);
                }
                go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();
                if (go.TryGetComponent(out ForestRegion region))
                {
                    region.Id = data.UnlockId;
                }
                go.GetComponent<Collider>().enabled = false;
                _spawnedBuilds.Add(key, go);

                if (valueCounts.ContainsKey(go.name))
                    valueCounts[go.name]++;
                else
                    valueCounts[go.name] = 1;

                if (filtervalueCounts.ContainsKey(go.name))
                    filtervalueCounts[go.name]++;
                else
                    filtervalueCounts[go.name] = 1;
            }
        }
    }
    public void Remove(Vector2Int key)
    {
        _spawnedBuilds.Remove(key);
        if (_spawnedBuilds.TryGetValue(key, out GameObject go))
        {
            if (valueCounts.ContainsKey(go.name))
            {
                valueCounts[go.name]--;
                if (valueCounts[go.name] <= 0)
                    valueCounts.Remove(go.name);
            }
            else
                Managers.Debug.Log($"Remove null 발생", Define.EDebugType.Building);

            if (filtervalueCounts.ContainsKey(go.name))
            {
                filtervalueCounts[go.name]--;
                if (filtervalueCounts[go.name] <= 0)
                    filtervalueCounts.Remove(go.name);
            }
            else
                Managers.Debug.Log($"Remove null 발생", Define.EDebugType.Building);
        }
    }

    public void ColliderAllOn()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2Int key = GridKey(data.posX, data.posZ);

            if (_spawnedBuilds.TryGetValue(key, out GameObject obj) && obj != null)
            {
                Collider col = obj.GetComponent<Collider>();
                if (col != null)
                    col.enabled = true;
                else
                    Managers.Debug.Log($"ColliderAllOn null 발생", Define.EDebugType.Building);
            }

        }


    }
    public void ColliderAllOff()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2Int key = GridKey(data.posX, data.posZ);
            if (_spawnedBuilds.TryGetValue(key, out GameObject obj) && obj != null)
            {
                Collider col = obj.GetComponent<Collider>();
                if (col != null)
                    col.enabled = false;
                else
                    Managers.Debug.Log($"ColliderAllOn null 발생", Define.EDebugType.Building);
            }
        }
    }



    public void UpdateBuildLevel(int uniqueId, int newLevel)
    {
        if (_buildDataMap.TryGetValue(uniqueId, out var data))
        {
            data.LV = newLevel;
        }
        else
        {
            Debug.LogWarning($"[BuildMap] 레벨 업데이트 실패: ID {uniqueId}를 찾을 수 없습니다.");
        }
    }

    Vector2Int GridKey(float x, float z)
{
    float gridSize = 0.5f;
    return new Vector2Int(
        Mathf.RoundToInt(x / gridSize),
        Mathf.RoundToInt(z / gridSize)
    );
}
}