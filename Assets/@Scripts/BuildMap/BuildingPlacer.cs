using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingPlacer : MonoBehaviour
{

    public TestBaseBuilding[] buildingSO;

    //건물종류선택
    public void SelectBuildingType(int type)
    {
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Vector3 worldCenter = cam.ScreenToWorldPoint(screenCenter);
        Instantiate(buildingSO[type].BuildOBJ, worldCenter, Quaternion.identity);
    //생성은 했지만 레이를 쏴서 위치 재조정을 해야할듯
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
