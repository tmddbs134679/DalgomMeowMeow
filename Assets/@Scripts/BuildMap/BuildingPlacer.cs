using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingPlacer : MonoBehaviour
{

    public TestBaseBuilding[] buildingSO;
    public LayerMask groundLayer;
     [SerializeField] private float heightOffset = 0.5f; 

    //건물종류선택
    public void SelectBuildingType(int type)
    {

        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
        {
            Instantiate(buildingSO[type].BuildOBJ,new Vector3(groundHit.point.x,groundHit.point.y+heightOffset,groundHit.point.z), Quaternion.identity);
        }
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
