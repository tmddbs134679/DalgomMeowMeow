using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
[CreateAssetMenu(menuName = "Map/BuildOBJ")]
public class BuildingCostDataSO : ScriptableObject
{
    //고양이 슬롯머신 랜드마크 제한
    //각 건물의 갯수를 제한해야하나?
    //건물 지을때 마다 갯수 파악해서 1.2배율로 가격 올리기
    //갯수파악은 buildMap.ShowBuildInfo();을 해서 하면 되는데..
    //이거의 존재 이유는 뭐지?
    //뭘 저장하려고?
    [SerializeField] private GameObject _buildOBJ;

    [SerializeField] private int _sizeWidth;
    [SerializeField] private int _sizeHeight;


    public int sizeWidth { get => _sizeWidth; set => _sizeWidth = value; }
    public int sizeHeight { get => _sizeHeight; set => _sizeHeight = value; }

}
