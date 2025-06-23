using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingPlacer : MonoBehaviour
{
    
    TestBaseBuilding TestBaseBuilding;
//건물종류선택
    void SelectBuildingType(EBuildType eBuildType)
    {
        TestBaseBuilding.Type = eBuildType;
    }

    //건물설치재료판별
    void CheckBuildMaterials()
    {

    }

//설치 가능한지 판별
    void CanPlaceBuilding(Vector3 pos)
    {

    }

//설치할 장소에 설치
    void PlaceBuilding(Vector3 pos)
    {

    }

}
