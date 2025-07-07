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
        private BuildData _CurBuildData;
    private int buyMoney;

    private bool _isGold;
    private bool _isBuild;
    public bool isSelect = false;

    public bool isLongPressAcceptBuild=false;
    public UI_BuildAction uI_BuildAction;

    public bool isAI=false;

    public Action OnBuildingCancel;
        public Action OnBuildingAccept;

    public GameObject tempOBJ2; //롱프레스쪽 임시저장 오브젝트

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
        isAI = true;
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
    /// DraggableObject에서 LongPress가 호출 될 시 현상태의 오브젝트 가져오기
    /// _tempOBJ=
    /// </summary>
    public void SetRefOBJ(GameObject tempOBJ)
    {
        _saveBuildingSO = tempOBJ.GetComponent<BuildingBase>()?.BuildingData;
        _tempOBJ =Instantiate(tempOBJ.GetComponent<BuildingBase>()?.BuildingData.previewOBJ,
                new Vector3(tempOBJ.transform.position.x,tempOBJ.transform.position.y + _heightOffset,tempOBJ.transform.position.z),
                Quaternion.identity);
                    tempDraggleOBJ = _tempOBJ.GetComponent<DraggableObject>();
            tempDraggleOBJ.isDrag = true;
            tempDraggleOBJ.isLongPress = false;
        tempOBJ2 = tempOBJ;
        tempOBJ2.SetActive(false);
        _CurBuildData = new BuildData
        {
            posX = _tempOBJ.transform.position.x,
            posZ = _tempOBJ.transform.position.z,
            testBaseBuilding = _saveBuildingSO
        };
            
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
        isAI = false;
         isSelect = false;
        _isGold = CheckBuildGold();
        CanPlaceBuilding();
_isGold = true;
        if (_isGold && _isBuild)
        {

            Managers.Game.Gold -= buyMoney;

            _buildData = new BuildData
            {
                posX = _tempOBJ.transform.position.x,
                posZ = _tempOBJ.transform.position.z,
                testBaseBuilding = _saveBuildingSO
            };

            arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트

            _tempOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일

            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;
        }
    }
    
     public void AcceptLongPressBuild()
    {
        isAI = false;
         isSelect = false;
        CanPlaceBuilding();
        if (_isBuild)
        {
            _buildData = new BuildData
            {
                posX = _tempOBJ.transform.position.x,
                posZ = _tempOBJ.transform.position.z,
                testBaseBuilding = _saveBuildingSO
            };

            arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트
            arrayBuildPos.RemoveBuildData(_CurBuildData);//기존에 있던 오브젝트 제거
            buildMap.Remove(new Vector2(_CurBuildData.posX,_CurBuildData.posZ));
            _tempOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
            ClearTile();//기존에 있던 오브젝트의 타일 제거
            _tempOBJ.GetComponent<DraggableObject>().isLongPress = true;
            Destroy(_tempOBJ);
            Destroy(tempOBJ2);
            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;
                    buildMap.ColliderAllOn();
        }
    }

    /// <summary>
    /// 건물 설치 취소
    /// </summary>
    public void CancelBuild()
    {
        isAI = false;
        isSelect = false;
        if (isLongPressAcceptBuild)
        {
            tempOBJ2.SetActive(true);
            tempOBJ2.GetComponent<DraggableObject>().isLongPress = true;
            tempOBJ2.GetComponent<DraggableObject>().isDrag = true;
        }
        if (_tempOBJ != null)
            {
                _tempOBJ.GetComponent<DraggableObject>().isDrag = false;
                Destroy(_tempOBJ);
            }
        buildMap.ColliderAllOn();
    }

    public void RemoveBuild()
    {
                        _saveBuildingSO = tempDraggleOBJ.GetComponent<BuildingBase>()?.BuildingData;
                _CurBuildData = new BuildData
        {
            posX = _tempOBJ.transform.position.x,
            posZ = _tempOBJ.transform.position.z,
            testBaseBuilding = _saveBuildingSO
        };
                    arrayBuildPos.RemoveBuildData(_CurBuildData);//기존에 있던 오브젝트 제거
        //tempDraggleOBJ
        buildMap.Remove(new Vector2(_CurBuildData.posX, _CurBuildData.posZ));
        ClearTile();//기존에 있던 오브젝트의 타일 제거
          
                      gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
    }

    /// <summary>
    /// 해당 오브젝트 아래 타일 초기화
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
