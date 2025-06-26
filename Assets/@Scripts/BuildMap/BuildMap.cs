using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 저장되어있는 데이터맵 가져와서 그리드맵 생성및 타일에 정보전달
/// </summary>
public class BuildMap : MonoBehaviour
{
    public ArrayBuildPos arrayBuildPos;
    public GameObject BuildActiontUI;

    public BuildingPlacer buildingplacer;
    private Dictionary<Vector2, GameObject> _spawnedBuilds = new Dictionary<Vector2, GameObject>();
    void Start()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            GameObject go = Instantiate(data.testBaseBuilding.buildOBJ, new Vector3(data.posX, 1f, data.posZ), Quaternion.identity, transform);
            go.GetComponent<DraggableObject>().BuildActiontUI = BuildActiontUI;
            go.GetComponent<DraggableObject>().buildMap = gameObject.GetComponent<BuildMap>();
            go.GetComponent<DraggableObject>().buildingplacer = buildingplacer;
            _spawnedBuilds.Add(new Vector2(data.posX, data.posZ), go);
        }
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
                go.GetComponent<DraggableObject>().BuildActiontUI = BuildActiontUI;
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
            if (_spawnedBuilds.ContainsKey(key))
            {
                _spawnedBuilds[key].GetComponent<Collider>().enabled = true;
            }
        }


    }
    public void ColliderAllOff()
    {
        foreach (BuildData data in arrayBuildPos.baseBuilding)
        {
            Vector2 key = new Vector2(data.posX, data.posZ);
            if (_spawnedBuilds.ContainsKey(key))
            {
                _spawnedBuilds[key].GetComponent<Collider>().enabled = false;
            }
        }
    }
    
}