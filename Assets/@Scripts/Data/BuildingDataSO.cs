using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDataSO", menuName = "Data/Building")]
public class BuildingDataSO : ScriptableObject
{
    public int Id;
    public string BuildingName;
    public BuildingType BuildingType;
    public float Interval;
    //public ItemSO produceItem;  
    public int UnlockCost;
}

