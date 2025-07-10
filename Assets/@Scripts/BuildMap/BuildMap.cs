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
    private Dictionary<Vector2, GameObject> _spawnedBuilds = new Dictionary<Vector2, GameObject>();
    public Dictionary<String, int> valueCounts = new Dictionary<string, int>();
    void Start()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
             go.name = data.testBaseBuilding.BuildingType.ToString();
            
            if (go.GetComponent<BuildingBase>() != null)
            {
            go.GetComponent<BuildingBase>().SetUniqueId(data.UniqueId);
            go.GetComponent<BuildingBase>().SetLevel(data.LV);
            }

            go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();
            if (go.TryGetComponent(out ForestRegion region))
            {
                region.Id = data.UnlockId;
            }
            _spawnedBuilds.Add(new Vector2(data.posX, data.posZ), go);
        }
        var _valueCounts = _spawnedBuilds.Values
                          .GroupBy(v => v.name)
                          .ToDictionary(g => g.Key, g => g.Count());

        surface.BuildNavMesh();
    }

    public void LoadBuild()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2 key = new Vector2(data.posX, data.posZ);

            if (_spawnedBuilds.ContainsKey(key))
            {
                // 갱신
                _spawnedBuilds[key].transform.position = new Vector3(data.posX, 1f, data.posZ);
            }
            else
            {
                // 건설 후 추가 생성
                GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
                go.name = data.testBaseBuilding.BuildingType.ToString();
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
            }
        }
    }
    public void Remove(Vector2 key)
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
        }
    }

    public void ColliderAllOn()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2 key = new Vector2(data.posX, data.posZ);

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
            Vector2 key = new Vector2(data.posX, data.posZ);
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


    public void ShowBuildInfo()
    {
        valueCounts = _spawnedBuilds.Values
                          .GroupBy(v => v.name)
                          .ToDictionary(g => g.Key, g => g.Count());

        foreach (var a in valueCounts)
        {
            Debug.Log($"#############{a.Key} : {a.Value}개");
        }


    }
}