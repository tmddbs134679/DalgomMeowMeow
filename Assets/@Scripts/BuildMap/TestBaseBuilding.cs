using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBuildType
{
    NONE,
    FARM,
    COOK,
    PLAYGROUND,
    REST,
    FISHING,
    SHOP,
    STORAGE
        
    }
[CreateAssetMenu(menuName = "Map/BuildOBJ")]
public class TestBaseBuilding : ScriptableObject
{
    [SerializeField] private GameObject buildOBJ;
    [SerializeField] private EBuildType eType;
    [SerializeField] private int sizeWidth;
    [SerializeField] private int sizeHeight;

    public GameObject BuildOBJ { get => buildOBJ; set => buildOBJ = value; }
    public EBuildType EType { get => eType; set => eType = value; }
    public int SizeWidth { get => sizeWidth; set => sizeWidth = value; }
    public int SizeHeight { get => sizeHeight; set => sizeHeight = value; }
}
