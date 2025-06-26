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
    STORAGE,
    LOAD
        
    }

//SO를 일단은TestBaseBuilding로 다 해뒀지만 나중에 다 분류 시켜야함 상속받아서
    /// <summary>
    /// building쪽과 합쳐야함
    /// </summary>
[CreateAssetMenu(menuName = "Map/BuildOBJ")]
public class TestBaseBuilding : ScriptableObject
{
    [SerializeField] private GameObject _buildOBJ;
    [SerializeField] private GameObject _previweOBJ;
    [SerializeField] private EBuildType _eType;
    [SerializeField] private int _sizeWidth;
    [SerializeField] private int _sizeHeight;

public GameObject buildOBJ{ get => _buildOBJ; set => _buildOBJ = value; }
    public GameObject previewOBJ { get => _previweOBJ; set => _previweOBJ = value; }
    public EBuildType eType { get => _eType; set => _eType = value; }
    public int sizeWidth { get => _sizeWidth; set => _sizeWidth = value; }
    public int sizeHeight { get => _sizeHeight; set => _sizeHeight = value; }
}
