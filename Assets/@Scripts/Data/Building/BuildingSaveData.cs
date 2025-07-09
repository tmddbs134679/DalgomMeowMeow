using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildingSaveData : MonoBehaviour
{
    public string UniqueId;
    public string BuildingId;
    public int Level;
}

[System.Serializable]
public class GameSaveData
{
    public List<BuildingSaveData> Buildings = new();
}
