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
    public NavMeshSurface surface;

    [Header("임시 드래그 오브젝트")]
    public DraggableObject tempDraggleOBJ;

    [Header("연속설치 설정")]
    Dictionary<Vector2Int, Vector3> _roadPosArray = new Dictionary<Vector2Int, Vector3>();
    List<GameObject> _tempPreviewObjs = new List<GameObject>();

    [SerializeField] private float _heightOffset = 0.5f;
    public Collider[] TempCollider;
    public UI_BuildAction uI_BuildAction;

    public GameObject OriginTempOBJ; //롱프레스쪽 임시저장 오브젝트
    public static event Action<BaseBuildingSO> OnBuildingAccepted;
    public DragController dragController;

    private ArrayBuildPos _arrayBuildPos;
    private ArrayMapPos _arrayMapPos;
    private GameObject _PreviewOBJ;
    private BaseBuildingSO _saveBuildingSO;
    private BuildData _buildData;
    private BuildData _CurBuildData;

    //event
    public Action OnBuildingCancel;
    public Action OnBuildingAccept; //사용 안하는중
    public event Action OnAutoSave;

    //Int값
    public int tempTypeNum;
    public int BuyMoney { get => buyMoney; set => buyMoney = value; }
    public int uniqueId;
    public int LV;

    private int buyMoney;

    //Bool값
    public bool _isBuild;
    public bool isSelect = false;
    public bool isLongPressAcceptBuild = false;
    public bool isAI = false;
    public bool islimitBuildCount = true;
    public bool isSequenceBuild;

    public bool isGold;
    private string buildType;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }
    void Start()
    {
        InitializeMaps();
        uI_BuildAction = (Managers.UI.SceneUI as UI_GameScene)._uI_BuildAction;
        uI_BuildAction.SetActive(false);
    }

    void InitializeMaps()
    {
        _arrayBuildPos = buildMap.ArrayBuildPos;
        _arrayMapPos = gridMap.ArrayMapPos;
        _arrayBuildPos.BindEvent();
        _arrayMapPos.BindEvent();
    }

    /// <summary>
    /// 건물 종류 선택 시 호출
    /// </summary>
    public void SelectBuildingType(Define.EBuildingType type)
    {
        tempTypeNum = (int)type;
        if (!CheckBuildGold(type))
        {
                            Managers.UI.ShowToast("돈이 부족합니다.");
            return;
        }
        if (SequenceSelectBuildingType(type)) return;
        isAI = true;
        isSelect = false;
        buildMap.ColliderAllOff();
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
        {
            _saveBuildingSO = buildingSO[(int)type];

            _PreviewOBJ = Instantiate(buildingSO[(int)type].previewOBJ,
                new Vector3(groundHit.point.x, groundHit.point.y + _heightOffset, groundHit.point.z),
                Quaternion.identity);

            tempDraggleOBJ = _PreviewOBJ.GetComponent<DraggableObject>();
            tempDraggleOBJ.SnapToGrid(groundHit.point);
            tempDraggleOBJ.isDrag = true;
            tempDraggleOBJ.isLongPress = false;
        }
    }

    /// <summary>
    ///  연속 설치 건물 종류 선택 시 호출
    /// </summary>
    private bool SequenceSelectBuildingType(Define.EBuildingType type)
    {
        if (type != Define.EBuildingType.Road) return false;

        isSequenceBuild = true;
        isAI = true;
        isSelect = false;
        buildMap.ColliderAllOff();
        tempTypeNum = (int)type;
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, cam.nearClipPlane);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out var groundHit, 1000f, groundLayer))
        {
            _saveBuildingSO = buildingSO[(int)type];

            _PreviewOBJ = Instantiate(buildingSO[(int)type].previewOBJ,
                new Vector3(groundHit.point.x, groundHit.point.y + _heightOffset, groundHit.point.z),
                Quaternion.identity);

            tempDraggleOBJ = _PreviewOBJ.GetComponent<DraggableObject>();
            tempDraggleOBJ.SnapToGrid(new Vector3(groundHit.point.x, groundHit.point.y - _heightOffset, groundHit.point.z));
            tempDraggleOBJ.isDrag = true;
            tempDraggleOBJ.isLongPress = false;
        }


        return true;
    }

    /// <summary>
    /// DraggableObject에서 LongPress가 호출 될 시 현상태의 오브젝트 가져오기
    /// _tempOBJ=
    /// </summary>
    public void SetRefOBJ(GameObject OriginOBJ)
    {
        _saveBuildingSO = OriginOBJ.GetComponent<BuildingBase>()?.BuildingData;
        uniqueId = OriginOBJ.GetComponent<BuildingBase>().UniqueId;
        LV = OriginOBJ.GetComponent<BuildingBase>().CurrentLevel;
        _PreviewOBJ = Instantiate(OriginOBJ.GetComponent<BuildingBase>()?.BuildingData.previewOBJ,
                new Vector3(OriginOBJ.transform.position.x, OriginOBJ.transform.position.y + _heightOffset, OriginOBJ.transform.position.z),
                Quaternion.identity);
        _PreviewOBJ.GetComponent<DraggableObject>().CheckTilesUnderBuilding();
        tempDraggleOBJ = _PreviewOBJ.GetComponent<DraggableObject>();
        tempDraggleOBJ.SnapToGrid(new Vector3(OriginOBJ.transform.position.x, OriginOBJ.transform.position.y - _heightOffset, OriginOBJ.transform.position.z));
        tempDraggleOBJ.isDrag = true;
        tempDraggleOBJ.isLongPress = false;
        OriginTempOBJ = OriginOBJ;
        dragController.ChangeTarget(tempDraggleOBJ);
        OriginTempOBJ.SetActive(false);
        _CurBuildData = new BuildData
        {
            posX = _PreviewOBJ.transform.position.x,
            posZ = _PreviewOBJ.transform.position.z,
            UniqueId = uniqueId,
        };

    }



    /// <summary>
    /// 설치 재료(돈) 판별
    /// </summary>
    bool CheckBuildGold(Define.EBuildingType Type)
    {
        buildType = Type.ToString();
        
  if (buildType == "Road") // 도로일 땐 else 처리,임시땜빵,나중에는 모든게 엑셀 데이터를 받아와서 계산해야함
        {
            BuyMoney = buildingSO[tempTypeNum].BuyMoney;
        }
        else if (buildMap.valueCounts.TryGetValue(buildType, out int count))
        {
            BuyMoney = (int)(buildingSO[tempTypeNum].BuyMoney * Mathf.Pow(1.2f, count));
        }
        else
        {
            BuyMoney = buildingSO[tempTypeNum].BuyMoney;
        }
        isGold = Managers.Game.Gold > buyMoney;
        return isGold;
    }

    /// <summary>
    /// 설치 가능 여부 판별
    /// </summary>
    public void CanPlaceBuilding()
    {
        if (_PreviewOBJ != null)
            _isBuild = _PreviewOBJ.GetComponent<DraggableObject>().isBuild;
    }

    public void SaveandRemoveRoad()
    {
        CanPlaceBuilding();
        if (_isBuild)
        {
            Vector2Int pos = new Vector2Int((int)_PreviewOBJ.transform.position.x, (int)_PreviewOBJ.transform.position.z);
            if (_roadPosArray.ContainsKey(pos)) //PreviewOBJ 동일 위치 중복 생성 방지
            {
                return;
            }
            GameObject PreviewOBJ = Instantiate(buildingSO[tempTypeNum].buildOBJ,
                   new Vector3(_PreviewOBJ.transform.position.x, _PreviewOBJ.transform.position.y + _heightOffset, _PreviewOBJ.transform.position.z),
                   Quaternion.identity);
            PreviewOBJ.GetComponent<DraggableObject>().SnapToGrid(new Vector3(_PreviewOBJ.transform.position.x, _PreviewOBJ.transform.position.y - _heightOffset, _PreviewOBJ.transform.position.z));
            PreviewOBJ.GetComponent<Collider>().enabled = false;
            _tempPreviewObjs.Add(PreviewOBJ);

                Vector3 temp = _PreviewOBJ.transform.position;
                _roadPosArray.Add(pos, temp); // 값은 placeholder로 true 사용

        }
    }

    public void ClearTempPreviewObjects()
    {
        foreach (var obj in _tempPreviewObjs)
        {
            if (obj != null)
                Destroy(obj);
        }
        _tempPreviewObjs.Clear();
        _roadPosArray.Clear();
    }
    /// <summary>
    /// 연속 건물 설치 확정
    /// </summary>
    public void AcceptSequenceBuild()
    {
        
        isAI = false;
        isSelect = false;
        CanPlaceBuilding();
OnBuildingCancel?.Invoke();//buildaction에서 분기처리가 잘 안되어서 여기서 처리


            foreach (var a in _roadPosArray)
            {
                int hash = Guid.NewGuid().GetHashCode();
                _buildData = new BuildData
                {
                    posX = a.Key.x,
                    posZ = a.Key.y,
                    testBaseBuilding = _saveBuildingSO,
                    UniqueId = hash,
                    LV = 0,
                };

                _arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트
                _PreviewOBJ.transform.position = a.Value;
                _PreviewOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
            }
            ClearTempPreviewObjects();
                        Destroy(_PreviewOBJ);
            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;

            buildMap.ColliderAllOn();
            OnBuildingAccepted?.Invoke(_saveBuildingSO);
            QuestManager.Instance.NotifyBuildingConstructed(((Define.EBuildingType)tempTypeNum).ToString());
        
        OnAutoSave?.Invoke();
    }

    /// <summary>
    /// 건물 설치 확정
    /// </summary>
    public void AcceptBuild()
    {
        isAI = false;
        isSelect = false;
        CanPlaceBuilding();


        if ( _isBuild)
        {

            Managers.Game.Gold -= buyMoney;

            int hash = Guid.NewGuid().GetHashCode();
            _buildData = new BuildData
            {
                posX = _PreviewOBJ.transform.position.x,
                posZ = _PreviewOBJ.transform.position.z,
                testBaseBuilding = _saveBuildingSO,
                UniqueId = hash,
                LV = 1,
            };

            _arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트
            _PreviewOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
                        Destroy(_PreviewOBJ);
            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;
                       buildMap.ColliderAllOn();
            OnBuildingAccepted?.Invoke(_saveBuildingSO);
            QuestManager.Instance.NotifyBuildingConstructed(buildType);
        }
        _PreviewOBJ.GetComponent<DraggableObject>().CheckTilesUnderBuilding(); //설치 이후 종료하는게 아니기 때문에 계속 타일 판별

        OnAutoSave?.Invoke();
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
                posX = _PreviewOBJ.transform.position.x,
                posZ = _PreviewOBJ.transform.position.z,
                testBaseBuilding = _saveBuildingSO,
                UniqueId = uniqueId,
                LV = LV,
            };
            _arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트
            _arrayBuildPos.RemoveBuildData(_CurBuildData);//기존에 있던 오브젝트 제거
            buildMap.Remove(_CurBuildData.UniqueId);
            _PreviewOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
            ClearTile();//기존에 있던 오브젝트의 타일 제거
            _PreviewOBJ.GetComponent<DraggableObject>().isLongPress = true;
            // _PreviewOBJ.GetComponent<BuildingBase>().SerialID
            Destroy(_PreviewOBJ);
            Destroy(OriginTempOBJ);
            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;
            buildMap.ColliderAllOn();

            Managers.AI.AllRelocateToNearestNavMesh();

            // foreach (var ai in AIManager.Instance.AllCharacters)
            // {

            //     Managers.AI.ValidateNavMeshPosition(ai);
            // }
        }
        OnAutoSave?.Invoke();
    }

    /// <summary>
    /// 건물 설치 취소
    /// </summary>
    public void CancelBuild()
    {
        isAI = false;
        isSelect = false;
        if (isLongPressAcceptBuild && OriginTempOBJ != null)
        {
            OriginTempOBJ.SetActive(true);
            OriginTempOBJ.GetComponent<DraggableObject>().isLongPress = true;
            OriginTempOBJ.GetComponent<DraggableObject>().isDrag = true;
        }
        if (_PreviewOBJ != null)
        {
            _PreviewOBJ.GetComponent<DraggableObject>().isDrag = false;
            Destroy(_PreviewOBJ);
        }
        buildMap.ColliderAllOn();
        isLongPressAcceptBuild = false;
                ClearTempPreviewObjects();
    }

    public void RemoveBuild()
    {
        _arrayBuildPos.RemoveBuildData(_CurBuildData);//기존에 있던 오브젝트 제거
        buildMap.Remove(_CurBuildData.UniqueId);
        ClearTile();//기존에 있던 오브젝트의 타일 제거
        Destroy(OriginTempOBJ);
        Destroy(_PreviewOBJ);
        gridMap.LoadMap(); //맵갱신
        buildMap.LoadBuild(); //오브젝트 갱신
        surface.BuildNavMesh(); //네브매쉬 깔기
        Managers.AI.AllRelocateToNearestNavMesh();
                OnAutoSave?.Invoke();
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

    public void DeleteStage(GameObject stage)
    {

        BuildData data = new BuildData
        {
            posX = stage.transform.position.x,
            posZ = stage.transform.position.z,
            UnlockId = stage.GetComponent<ForestRegion>().Id,
        };
        _arrayBuildPos.RemoveStageData(data);//기존에 있던 오브젝트 제거
        buildMap.Remove(data.UniqueId);
        stage.GetComponent<DraggableObject>().CurrentTileAndOBJ();
        ClearTile();//기존에 있던 오브젝트의 타일 제거
        Destroy(stage);
        gridMap.LoadMap(); //맵갱신
        buildMap.LoadBuild(); //오브젝트 갱신
        surface.BuildNavMesh(); //네브매쉬 깔기
    }

    //건물 갯수 제한 코드 구간


    Vector2Int GridKey(float x, float z)
    {
        float gridSize = 0.5f;
        return new Vector2Int(
            Mathf.RoundToInt(x / gridSize),
            Mathf.RoundToInt(z / gridSize)
        );
    }
}
