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
    #region 변수 
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
    public UI_LongPressGauge uI_LongPressGauge;
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
    public int BuyMoney { get => _buyMoney; set => _buyMoney = value; }
    public int uniqueId;
    public int LV;

    private int _buyMoney;

    private int _sumBuyMoney;
    //Bool값
    public bool _isBuild;//건설이 가능한 곳일 때 true
    public bool isSelect;//프리뷰 드래그,맵드래그가 동시에 눌리는거 방지
    public bool isLongPressAcceptBuild = false; //롱프레스일때 true
    public bool isAI = false;//건물 선택중일때는 true,캐릭터 상호작용 금지
    public bool islimitBuildCount = true;//건설갯수제한
    public bool isSequenceBuild;//연속건설일경우 true

    public bool isSequenceRemove;//연속삭제일경우 true
    public bool isGold;
    private string buildType;
    BuildingBase buildBase;

    #endregion
    #region 클래스 선언,초기화
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
        uI_LongPressGauge = (Managers.UI.SceneUI as UI_GameScene)._uI_LongPressGauge;
        uI_LongPressGauge.SetActive(false);

    }

    void InitializeMaps()
    {
        _arrayBuildPos = buildMap.ArrayBuildPos;
        _arrayMapPos = gridMap.ArrayMapPos;
        _arrayBuildPos.BindEvent();
        _arrayMapPos.BindEvent();
    }
    #endregion
    #region 건물 선택
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
        _sumBuyMoney += BuyMoney;
        uI_BuildAction.CountGold(_sumBuyMoney);
        if (RemoveRoadSelectBuildingType(type)) return;
        if (SequenceSelectBuildingType(type)) return;
        isAI = true;
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
            StartCoroutine(SnapToGridAfterFrame(groundHit.point));
            tempDraggleOBJ.isDrag = true;
            tempDraggleOBJ.isLongPress = false;
        }
    }

    private IEnumerator SnapToGridAfterFrame(Vector3 pos)
{
    yield return null; // 1프레임 대기
    tempDraggleOBJ.SnapToGrid(pos);
}

    /// <summary>
    /// DraggableObject에서 LongPress가 호출 될 시 현상태의 오브젝트 가져오기
    /// _tempOBJ=
    /// </summary>
    public void SetRefOBJ(GameObject OriginOBJ)
    {

        isSelect = true;
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
    ///  연속 설치 건물 종류 선택 시 호출
    /// </summary>
    private bool SequenceSelectBuildingType(Define.EBuildingType type)
    {
        if (type != Define.EBuildingType.Road) return false;
        isSequenceBuild = true;
        isAI = true;
        isSelect = true;
        buildMap.ColliderAllOff();
        tempTypeNum = (int)type;
        _saveBuildingSO = buildingSO[(int)type];
        return true;
    }


    private bool RemoveRoadSelectBuildingType(Define.EBuildingType type)
    {
        if (type != Define.EBuildingType.None) return false;
        isSequenceRemove = true;
        isAI = true;
        isSelect = true;
        //  buildMap.ColliderAllOff();
        tempTypeNum = (int)type;
        _saveBuildingSO = buildingSO[(int)type];
        return true;
    }
    #endregion

    #region 도로관련 선택및 설치
    //위에 주석처리된 카메라 중심 프리뷰 생성에서 터치한곳을 중심으로 프리뷰생성
    public void OnGroundTouched(Vector3 point)
    {
        if (!isSequenceBuild || _PreviewOBJ != null)
            return; // 조건이 안 맞으면 무시
        uI_BuildAction.SetActive(true);
        uI_BuildAction.ButtonSetAtive();
        // 프리뷰 생성
        _PreviewOBJ = Instantiate(_saveBuildingSO.previewOBJ,
            new Vector3(point.x, point.y + _heightOffset, point.z),
            Quaternion.identity);

        tempDraggleOBJ = _PreviewOBJ.GetComponent<DraggableObject>();
        tempDraggleOBJ.SnapToGrid(new Vector3(point.x, point.y - _heightOffset, point.z));
        tempDraggleOBJ.isDrag = true;
        tempDraggleOBJ.isLongPress = false;
    }



    Collider[] hits;
    //카메라 중심 프리뷰 생성에서 터치한곳을 중심으로 프리뷰생성
    public void OnGroundTouchedSecond(Vector3 point)
    {
        if (!isSequenceRemove || _PreviewOBJ != null)
            return; // 조건이 안 맞으면 무시
                    //  buildMap.ColliderAllOff();
        uI_BuildAction.SetActive(true);
        uI_BuildAction.ButtonSetAtive();
        _PreviewOBJ = Instantiate(buildingSO[(int)Define.EBuildingType.RemoveRoad].previewOBJ, //수정필요1355
       new Vector3(point.x, point.y + _heightOffset, point.z),
       Quaternion.identity);

        // 프리뷰 오브젝트에서 감지기 가져오기
        PreviewColliderSensor sensor = _PreviewOBJ.GetComponent<PreviewColliderSensor>();

        if (sensor == null)
            sensor = _PreviewOBJ.AddComponent<PreviewColliderSensor>();

        sensor.currentHits.Clear();
        tempDraggleOBJ = _PreviewOBJ.GetComponent<DraggableObject>();
        tempDraggleOBJ.SnapToGrid(new Vector3(point.x, point.y - _heightOffset, point.z));
        tempDraggleOBJ.isDrag = true;
        tempDraggleOBJ.isLongPress = false;
    }
    public void RemoveRoad()
    {
        hits = _PreviewOBJ.GetComponent<PreviewColliderSensor>()?.GetCurrentHits();

        if (hits == null || hits.Length == 0)//설치할 도로 없을 시
        {
            Destroy(OriginTempOBJ);
            Destroy(_PreviewOBJ);
            OriginTempOBJ = null;
            _PreviewOBJ = null;
            return;
        }

        foreach (var hit in hits)
        {
            GameObject obj = hit.gameObject;
            obj.GetComponent<DraggableObject>().CurrentTileAndOBJ();
            if (obj.layer == LayerMask.NameToLayer("Road"))
            {
                buildBase = obj.GetComponent<BuildingBase>();
            }

            if (buildBase != null)
            {
                _CurBuildData = new BuildData
                {
                    posX = obj.transform.position.x,
                    posZ = obj.transform.position.z,
                    UniqueId = buildBase.UniqueId
                };
                _arrayBuildPos.RemoveBuildData(_CurBuildData);
                buildMap.Remove(buildBase.UniqueId);
                Destroy(obj);
                ClearTile();
            }
            buildBase = null;


        }


        Destroy(OriginTempOBJ);
        Destroy(_PreviewOBJ);

        gridMap.LoadMap();
        buildMap.LoadBuild();
        StartCoroutine(RebuildNavMeshAfterDestroy());


        buildMap.ColliderAllOn();
        isLongPressAcceptBuild = false;
        OriginTempOBJ = null;
        _PreviewOBJ = null;
                MapBuildSave();
    }

    //여러개 삭제시에는 프레임이느려져서 네브매쉬가 destroy가 제대로 안된 상태에서 재생성 할 수 있음
    IEnumerator RebuildNavMeshAfterDestroy()
    {
        yield return null; // 다음 프레임까지 기다림
        surface.BuildNavMesh(); // 이제 완전히 제거된 후 갱신
        Managers.AI.AllRelocateToNearestNavMesh();
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
                    isGold = Managers.Game.Gold >= _buyMoney;
            return isGold;
        }

        if (buildType == "Resting") // 도로일 땐 else 처리,임시땜빵,나중에는 모든게 엑셀 데이터를 받아와서 계산해야함
        {
            if (buildMap.valueCounts.TryGetValue(buildType, out int counta))
            {
                BuyMoney = (int)(buildingSO[tempTypeNum].BuyMoney * Mathf.Pow(3f, counta));
            }
                    else
        {
            BuyMoney = buildingSO[tempTypeNum].BuyMoney;
        }
                    isGold = Managers.Game.Gold >= _buyMoney;
            return isGold;
        }

        if (buildMap.valueCounts.TryGetValue(buildType, out int count))
        {
            BuyMoney = (int)(buildingSO[tempTypeNum].BuyMoney * Mathf.Pow(10f, count));
        }
        else
        {
            BuyMoney = buildingSO[tempTypeNum].BuyMoney;
        }

        isGold = Managers.Game.Gold >= _buyMoney;
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
            GameObject PreviewOBJ = Instantiate(buildingSO[tempTypeNum].previewOBJ,
                   new Vector3(_PreviewOBJ.transform.position.x, _PreviewOBJ.transform.position.y + _heightOffset, _PreviewOBJ.transform.position.z),
                   Quaternion.identity);
            PreviewOBJ.GetComponent<DraggableObject>().SnapToGrid(new Vector3(_PreviewOBJ.transform.position.x, _PreviewOBJ.transform.position.y - _heightOffset, _PreviewOBJ.transform.position.z));
            PreviewOBJ.GetComponent<Collider>().enabled = false;
            _tempPreviewObjs.Add(PreviewOBJ);

            Vector3 temp = _PreviewOBJ.transform.position;
            _roadPosArray.Add(pos, temp); // 값은 placeholder로 true 사용
            _sumBuyMoney += BuyMoney;
            uI_BuildAction.CountGold(_sumBuyMoney);
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
    #endregion
    #region 건물 설치
    /// <summary>
    /// 연속 건물 설치 확정
    /// </summary>
    public void AcceptSequenceBuild()
    {

        isAI = false;
        CanPlaceBuilding();
        OnBuildingCancel?.Invoke();//buildaction에서 분기처리가 잘 안되어서 여기서 처리
        (Managers.UI.SceneUI as UI_GameScene).gameObject.SetActive(true);

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
        #if UNITY_EDITOR
        _arrayBuildPos.EditorOnly_SaveAsset();
#endif
        Managers.Game.Gold -= _sumBuyMoney;
        _sumBuyMoney = 0;
        uI_BuildAction.CountGold(_sumBuyMoney);
        ClearTempPreviewObjects();
        Destroy(_PreviewOBJ);
        gridMap.LoadMap(); //맵갱신
        buildMap.LoadBuild(); //오브젝트 갱신
        surface.BuildNavMesh(); //네브매쉬 깔기
        isLongPressAcceptBuild = false;

        buildMap.ColliderAllOn();
        Managers.AI.AllRelocateToNearestNavMesh();
        OnBuildingAccepted?.Invoke(_saveBuildingSO);
        QuestManager.Instance.NotifyBuildingConstructed(((Define.EBuildingType)tempTypeNum).ToString());
        MapBuildSave();
    }

    /// <summary>
    /// 건물 설치 확정
    /// </summary>
    public void AcceptBuild()
    {
        isAI = false;
        CanPlaceBuilding();


        if (_isBuild)
        {

            Managers.Game.Gold -= _buyMoney;
            _sumBuyMoney = 0;
            uI_BuildAction.CountGold(_sumBuyMoney);
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
#if UNITY_EDITOR
            _arrayBuildPos.EditorOnly_SaveAsset();
#endif
            _PreviewOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
            Destroy(_PreviewOBJ);
            gridMap.LoadMap(); //맵갱신
            buildMap.LoadBuild(); //오브젝트 갱신
            surface.BuildNavMesh(); //네브매쉬 깔기
            isLongPressAcceptBuild = false;
            buildMap.ColliderAllOn();
            Managers.AI.AllRelocateToNearestNavMesh();
            OnBuildingAccepted?.Invoke(_saveBuildingSO);
            QuestManager.Instance.NotifyBuildingConstructed(buildType);
        }
        _PreviewOBJ.GetComponent<DraggableObject>().CheckTilesUnderBuilding(); //설치 이후 종료하는게 아니기 때문에 계속 타일 판별
        MapBuildSave();
    }

    public void AcceptLongPressBuild()
    {
        isAI = false;
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
#if UNITY_EDITOR
            _arrayBuildPos.EditorOnly_SaveAsset();
#endif
            _arrayBuildPos.RemoveBuildData(_CurBuildData);//기존에 있던 오브젝트 제거
            _arrayBuildPos.GetBuildData(_buildData);//설치할 오브젝트
            buildMap.Remove(_CurBuildData.UniqueId);
            _PreviewOBJ.GetComponent<DraggableObject>().SetTileIsBuild();//새롭게 설치할 오브젝트의 타일
            ClearTile();//기존에 있던 오브젝트의 타일 제거
            _PreviewOBJ.GetComponent<DraggableObject>().isLongPress = true;
            // _PreviewOBJ.GetComponent<BuildingBase>().SerialID
            Destroy(OriginTempOBJ);
            Destroy(_PreviewOBJ);
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
                MapBuildSave();
    }
    #endregion
    #region 건물 취소 및 삭제
    /// <summary>
    /// 건물 설치 취소
    /// </summary>
    public void CancelBuild()
    {

        isAI = false;
        if (isLongPressAcceptBuild && OriginTempOBJ != null)
        {
            OriginTempOBJ.SetActive(true);
            OriginTempOBJ.GetComponent<DraggableObject>().isLongPress = true;
            OriginTempOBJ.GetComponent<DraggableObject>().isDrag = false;
        }
        if (_PreviewOBJ != null)
        {
            _PreviewOBJ.GetComponent<PreviewColliderSensor>()?.CancelPreview();
            _PreviewOBJ.GetComponent<DraggableObject>().isDrag = false;
         //   OriginTempOBJ.GetComponent<DraggableObject>().isDrag = false;
            Destroy(_PreviewOBJ);
        }
        _sumBuyMoney = 0;
        uI_BuildAction.CountGold(_sumBuyMoney);
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
        buildMap.ColliderAllOn();
        isLongPressAcceptBuild = false;
        OriginTempOBJ = null;
        _PreviewOBJ = null;
        Managers.AI.AllRelocateToNearestNavMesh();
                MapBuildSave();
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
                MapBuildSave();
    }
    #endregion
    #region 저장 및 기타

    public void OnApplicationQuit()//유니티 내장 맨마지막에 불려지는 함수
    {
        _arrayBuildPos.SaveMapData();
        _arrayMapPos.SaveMapTileData();
        Managers.Game.SaveGame();
    }
//씬 종료전에 저장
    private void OnDisable()
    {
        _arrayBuildPos.SaveMapData();
        _arrayMapPos.SaveMapTileData();
    }

    private void MapBuildSave()
    {
                _arrayBuildPos.SaveMapData();
        _arrayMapPos.SaveMapTileData();
    }
    Vector2Int GridKey(float x, float z)
    {
        float gridSize = 0.5f;
        return new Vector2Int(
            Mathf.RoundToInt(x / gridSize),
            Mathf.RoundToInt(z / gridSize)
        );
    }
    #endregion
}
