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
public EBuildType EType { get; set; }
public int sizeWidth { get; set; }
public int sizeHeight { get; set; }
}
