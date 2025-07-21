using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingDataSO", menuName = "Building/Building")]
public class BuildingDataSO : ScriptableObject
{
    public int Id;
    public string BuildingName;
    public Define.EBuildingType BuildingType;
    public float Interval;
    //public ItemSO produceItem;  
    public int UnlockCost;
}

