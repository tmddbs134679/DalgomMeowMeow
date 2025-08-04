using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class BuildMap : MonoBehaviour
{
    [SerializeField] private ArrayBuildPos _arrayBuildPos;

    public ArrayBuildPos ArrayBuildPos { get => _arrayBuildPos; set => _arrayBuildPos = value; }
    public NavMeshSurface surface;

    private Dictionary<int, GameObject> _spawnedBuilds = new Dictionary<int, GameObject>();
    private Dictionary<int, BuildData> _buildDataMap = new Dictionary<int, BuildData>();
    public Dictionary<string, int> valueCounts = new Dictionary<string, int>();

    async void Awake()
    {
        await _arrayBuildPos.LoadMapDataAsyncParallel();

        InstantiateBuildings();

        surface.BuildNavMesh();
        Managers.AI.AllRelocateToNearestNavMesh();
    }

    private void InstantiateBuildings()
    {
        _buildDataMap.Clear();
        _spawnedBuilds.Clear();
        valueCounts.Clear();

        foreach (BuildData data in _arrayBuildPos.baseBuilding)
        {
            _buildDataMap[data.UniqueId] = data;

            GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
            go.name = data.testBaseBuilding.BuildingType.ToString();

            if (go.TryGetComponent<BuildingBase>(out var buildingBase))
            {
                buildingBase.SetUniqueId(data.UniqueId);
                buildingBase.SetLevel(data.LV);
                buildingBase.SetBuildMap(this);
            }

            if (go.TryGetComponent<DraggableObject>(out var draggable))
                draggable.buildMap = this;

            if (go.TryGetComponent<ForestRegion>(out var region))
                region.Id = data.UnlockId;

            _spawnedBuilds.Add(data.UniqueId, go);
        }

        valueCounts = _spawnedBuilds.Values
            .GroupBy(v => v.name)
            .ToDictionary(g => g.Key, g => g.Count());
    }

    IEnumerator DelayedRebuildAndRelocate()
    {
        yield return null; // 1프레임 대기하여 오브젝트가 씬에 안정적으로 배치되도록 함
        surface.BuildNavMesh();
        Managers.AI.AllRelocateToNearestNavMesh();
    }
    public void LoadBuild()
    {
        
        foreach (BuildData data in _arrayBuildPos.baseBuilding)
        {
            int key = data.UniqueId;

            if (_spawnedBuilds.TryGetValue(key, out var build))
            {
                build.transform.position = new Vector3(data.posX, 1f, data.posZ);
            }
            else
            {
                // 건설 후 추가 생성
                float randomY = UnityEngine.Random.Range(0f, 360f);
                Quaternion randomRotation = Quaternion.Euler(0f, randomY, 0f);
                GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
                go.name = data.testBaseBuilding.BuildingType.ToString();
                // if (go.layer == LayerMask.NameToLayer("Road"))
                //     go.transform.rotation = randomRotation;

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
            }
        }
    }
    public void Remove(int key)
    {

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
                _spawnedBuilds.Remove(key);
    }
public void ColliderAllOn()
{
    foreach (BuildData data in _arrayBuildPos.baseBuilding)
    {
        int key = data.UniqueId;
        if (_spawnedBuilds.TryGetValue(key, out GameObject obj) && obj != null)
        {
            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = true; //  콜라이더 켜기
            }
            else
            {
                Managers.Debug.Log("ColliderAllOn null 발생", Define.EDebugType.Building);
            }
        }
    }
}

public void ColliderAllOff()
{
    foreach (BuildData data in _arrayBuildPos.baseBuilding)
    {
        int key = data.UniqueId;
        if (_spawnedBuilds.TryGetValue(key, out GameObject obj) && obj != null)
        {
            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false; //  콜라이더 끄기
            }
            else
            {
                Managers.Debug.Log("ColliderAllOff null 발생", Define.EDebugType.Building);
            }
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