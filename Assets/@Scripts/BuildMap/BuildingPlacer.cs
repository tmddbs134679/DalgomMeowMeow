using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingPlacer : MonoBehaviour
{

    public GameObject[] buildOBJ;
    TestBaseBuilding testBaseBuilding;



    //건물종류선택
    public void SelectBuildingType(int type)
    {
              testBaseBuilding= buildOBJ[type].GetComponent<TestBaseBuilding>();


      
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);
     //   Instantiate(buildOBJ[type], worldCenter, Quaternion.identity);
    }

    //건물설치재료판별
   public void CheckBuildMaterials()
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
