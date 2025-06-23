using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBuildType
{
    NONE,
    FARM,
    COOK,
    PLAYGROUND,
    FISHING,
    REST,
    SHOP,
    STORAGE
        
    }
public class TestBaseBuilding : MonoBehaviour
{
public GameObject Build { get; set; }
public EBuildType Type { get; set; }
public int PosX { get; set; }
public int PosY { get; set; }
}
