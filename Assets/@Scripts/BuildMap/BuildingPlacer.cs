using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using TMPro;
/// <summary>
/// 건물 설치 기능 전용 매니저 (UI 관리 분리)
/// </summary>
public class BuildingPlacer : MonoBehaviour
{
    public static BuildingPlacer Instance;

    [Header("설치 관련 설정")]
    public BaseBuildingSO[] buildingSO;
    public LayerMask groundLayer;
    public GridMap gridMap;
    public BuildMap buildMap;
    public ArrayBuildPos arrayBuildPos;
    public NavMeshSurface surface;

    public BuildUI buildUI;

    [Header("임시 드래그 오브젝트")]
    public DraggableObject tempDraggleOBJ;

    [SerializeField] private float _heightOffset = 0.5f;
    public Collider[] TempCollider;

    private GameObject _tempOBJ;
    private BaseBuildingSO _saveBuildingSO;
    private BuildData _buildData;
    private int buyMoney;

    private bool _isGold;
    private bool _isBuild;
    public bool isSelect = false;

    public UI_BuildAction uI_BuildAction;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// 건물 종류 선택 시 호출
    /// </summary>
    public void SelectBuildingType(int type)
    {
        isSelect = true;
        buildMap.ColliderAllOff();

        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
        {
            _saveBuildingSO = buildingSO[type];
            buyMoney = buildingSO[type].BuyMoney;
            _tempOBJ = Instantiate(buildingSO[type].previewOBJ,
                new Vector3(groundHit.point.x, groundHit.point.y + _heightOffset, groundHit.point.z),
                Quaternion.identity);

            tempDraggleOBJ = _tempOBJ.GetComponent<DraggableObject>();
            tempDraggleOBJ.isDrag = true;
            tempDraggleOBJ.isLongPress = false;
        }

    }

    /// <summary>
    /// DraggableObject에서 설치할 오브젝트 설정 시 호출
    /// </summary>
    public void SetTempOBJ(GameObject tempOBJ)
    {
        _saveBuildingSO = tempOBJ.GetComponent<BuildingBase>()?.BuildingData;
        _tempOBJ = tempOBJ;
    }

    /// <summary>
    /// 설치 재료(돈) 판별
    /// </summary>
    bool CheckBuildGold()
    {
        return Managers.Game.Gold > 0;
    }

    /// <summary>
    /// 설치 가능 여부 판별
    /// </summary>
    public void CanPlaceBuilding()
    {
        if (_tempOBJ != null)
            _isBuild = _tempOBJ.GetComponent<DraggableObject>().isBuild;
    }

    /// <summary>
    /// 건물 설치 확정
    /// </summary>
    public void AcceptBuild()
    {
        _isGold = CheckBuildGold();
        CanPlaceBuilding();
        if (tempDraggleOBJ.isLongPress) _isGold = true;
        if (_isGold && _isBuild)
        {
                    if (!tempDraggleOBJ.isLongPress)
            Managers.Game.Gold -= buyMoney;

            _buildData = new BuildData
            {
                posX = _tempOBJ.transform.position.x,
                posZ = _tempOBJ.transform.position.z,
                testBaseBuilding = _saveBuildingSO
            };

            arrayBuildPos.GetBuildData(_buildData);

            _tempOBJ.GetComponent<DraggableObject>().SetTileIsBuild();

           if(tempDraggleOBJ.isLongPress)ClearTile();
            gridMap.LoadMap();
            buildMap.LoadBuild();
            surface.BuildNavMesh();
        }
    }

    /// <summary>
    /// 건물 설치 취소
    /// </summary>
    public void CancelBuild()
    {
        isSelect = false;

        if (_tempOBJ != null)
        {
            _tempOBJ.GetComponent<DraggableObject>().isDrag = false;
            Destroy(_tempOBJ);
        }
        buildMap.ColliderAllOn();
    }
    

    /// <summary>
    /// 해당 오브젝트 밑 타일 초기화
    /// </summary>
    public void ClearTile()
    {
        foreach (Collider col in TempCollider)
        {
            if (col.CompareTag("Tile"))
            {
                var tile = col.GetComponent<TileObjectData>();
                tile.isLoadBuild = false;
                tile.SetTile();
            }
        }
    }
}
