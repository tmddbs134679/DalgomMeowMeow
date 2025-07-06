using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

/// <summary>
/// 저장되어있는 데이터맵 가져와서 그리드맵 생성및 타일에 정보전달
/// 현재까지는 Collider ON,Off를 하기 위한 장치
/// </summary>
public class BuildMap : MonoBehaviour
{
    public ArrayBuildPos arrayBuildPos;

    public NavMeshSurface surface;
    private Dictionary<Vector2, GameObject> _spawnedBuilds = new Dictionary<Vector2, GameObject>();
    void Start()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
            go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();

            _spawnedBuilds.Add(new Vector2(data.posX, data.posZ), go);
        }
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
            go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();
                go.GetComponent<Collider>().enabled = false;
                _spawnedBuilds.Add(key, go);
            }
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

}