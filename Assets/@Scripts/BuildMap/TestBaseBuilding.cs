using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBuildType
{
    NONE,
    COOK,
    FARM,
    FISHING,
    PLAYGROUND,
    REST,
    SHOP,
    STORAGE
        
    }

//SO를 일단은TestBaseBuilding로 다 해뒀지만 나중에 다 분류 시켜야함 상속받아서
    /// <summary>
    /// building쪽과 합쳐야함
    /// </summary>
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
