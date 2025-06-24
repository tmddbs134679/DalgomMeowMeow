using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public List<BuildingBase> _buildings = new();
    
    public event Action<BuildingType> OnAnimalArrived;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void Register(BuildingBase building)
    {
        if (building == null) return;
        if (!_buildings.Contains(building)) 
            _buildings.Add(building);
    }

    public void Unregister(BuildingBase building)
    {
        _buildings.Remove(building);
    }
    
    void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (var building in _buildings.ToArray())
        {
            building.Tick(deltaTime);
        }
    }
}
