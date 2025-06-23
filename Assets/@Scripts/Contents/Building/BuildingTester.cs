using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTester : MonoBehaviour
{
    public FarmBuilding farm;
    public CookingBuilding cooking;
    public RestBuilding rest;

    void Start()
    {
        farm.Init();
        farm.Unlock();
        BuildingManager.Instance.Register(farm);

        cooking.Init();
        cooking.Unlock();
        BuildingManager.Instance.Register(cooking);
        
        rest.Init();
        rest.Unlock();
        BuildingManager.Instance.Register(rest);
    }
}
