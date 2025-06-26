using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 저장되어있는 데이터맵 가져와서 그리드맵 생성및 타일에 정보전달
/// </summary>
public class BuildMap : MonoBehaviour
{
    public ArrayBuildPos arrayBuildPos;

    void Start()
    {
        foreach (BuildData a in arrayBuildPos.baseBuilding)
        {
            Instantiate(a.testBaseBuilding.buildOBJ, new Vector3(a.posX, 1f, a.posY), Quaternion.identity, transform);
        }
    }

    public void LoadBuild()
    { 
        
     }
}