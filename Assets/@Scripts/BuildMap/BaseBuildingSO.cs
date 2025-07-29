using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


//SO를 일단은TestBaseBuilding로 다 해뒀지만 나중에 다 분류 시켜야함 상속받아서
/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName = "Map/BuildOBJ")]
public class BaseBuildingSO : ScriptableObject
{
    [SerializeField] private GameObject _buildOBJ;
    [SerializeField] private GameObject _previweOBJ;
    [SerializeField] private int _sizeWidth;
    [SerializeField] private int _sizeHeight;

    [SerializeField] private int _id;
    [SerializeField] private string _buildingName;
    [SerializeField] private Define.EBuildingType _buildingType;
    [SerializeField] private float _interval;
    [SerializeField] private int _unlockCost;
    [SerializeField] private int _buyMoney;
    [SerializeField] private string _description;

    public GameObject buildOBJ { get => _buildOBJ; set => _buildOBJ = value; }
    public GameObject previewOBJ { get => _previweOBJ; set => _previweOBJ = value; }
    public int sizeWidth { get => _sizeWidth; set => _sizeWidth = value; }
    public int sizeHeight { get => _sizeHeight; set => _sizeHeight = value; }

    public int Id { get => _id; set => _id = value; }
    public string BuildingName { get => _buildingName; set => _buildingName = value; }
    public Define.EBuildingType BuildingType { get => _buildingType; set => _buildingType = value; }
    public float Interval { get => _interval; set => _interval = value; }
    public int UnlockCost { get => _unlockCost; set => _unlockCost = value; }
    public int BuyMoney { get => _buyMoney; set => _buyMoney = value; }
    
        public string  Description{get => _description; set => _description = value; }
}
