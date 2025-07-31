using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 건물 드래그앤드롭,그리드 스냅,건물 밑 타일 정보 반영
/// </summary>
public class DraggableObject : MonoBehaviour, IDraggable
{

    public UI_BuildAction _uI_BuildAction;
    public BuildMap buildMap;
    public bool isBuild;
    public bool isDrag = false;
    public bool isLongPress = true;
    [SerializeField] private float gridSize = 1f;         // 한 칸 크기
    [SerializeField] private float heightOffset = 0.5f;   // 바닥 위 높이

    private float _offsetx;
    private float _offsety;

    public GameObject TempOBJ;

    private IsBuildColor _isBuildColor;
    [SerializeField] private Vector2 buildSize = new Vector2(1f, 1f); // 건축물 밑면 크기 (x, z)
    [SerializeField] private LayerMask tileLayer;

    public readonly float testx = 2.1f;
    public readonly float testy = 2.1f;
    void Start()
    {
        _isBuildColor = GetComponent<IsBuildColor>();
        CheckTilesUnderBuilding();
    }
    #region IDraggable
    //드래그 스타트
    public void OnDragStart(Vector3 hitPos)
    {

            BuildingPlacer.Instance.isSelect = true;
        BuildingPlacer.Instance.tempDraggleOBJ = this;
        _offsetx = (buildSize.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (buildSize.y % 2 == 0) ? (gridSize / 2f) : 0f;
        CheckTilesUnderBuilding();
    }

    //드래그
    private Vector3Int _prevGridPos = Vector3Int.zero;

    public void OnDrag(Vector3 groundPos)
    {
        if (isDrag)
        {
            Vector3 snappedPos = GetSnappedPosition(groundPos);
            transform.position = snappedPos;

            // 현재 그리드 기준 위치 계산
            Vector3Int currentGridPos = new Vector3Int(
                Mathf.RoundToInt(transform.position.x / gridSize),
                0,
                Mathf.RoundToInt(transform.position.z / gridSize)
            );

            CheckTilesUnderBuilding();
            // 이전과 다를 때만 처리
            if (currentGridPos != _prevGridPos)
            {
                _prevGridPos = currentGridPos;
                if (BuildingPlacer.Instance.isSequenceBuild) BuildingPlacer.Instance.SaveandRemoveRoad();
            }

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            if (_uI_BuildAction != null)
                _uI_BuildAction.transform.position = screenPos;
        }
    }
    //드래그 드롭
    public void OnDragEnd()
    {
            BuildingPlacer.Instance.isSelect = false;
    }
    //드래그 하지않고 눌렀다 떼기
    public void OnClickRelease()
    {

            BuildingPlacer.Instance.isSelect = false;
    }
    //꾹누르기
    public void OnLongPress()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (this.GetComponent<BuildingBase>()?.CurrentState == BuildingState.Producing) return;
        if (isLongPress)
        {
            CheckTilesUnderBuilding();
            buildMap.ColliderAllOff();
            // BuildingPlacer.Instance.tempDraggleOBJ = this;
            BuildingPlacer.Instance.isLongPressAcceptBuild = true;
            isLongPress = false;
            isDrag = true;
            Managers.Debug.Log($"{this}+롱프레스 감지!", Define.EDebugType.Drag);

            if (Managers.UI.OnLongPress != null)
            {
                Managers.UI.OnLongPress.Invoke();
            }
            //Managers.UI.ShowPopupUI<UI_SaveMoveBuild>();//건물 저장 꺼내기 기능 생기면 쓰기
            BuildingPlacer.Instance.uI_BuildAction.SetActive(true);
            StartCoroutine(WaitAndSetup());
            //건물설치함수 불러오기
            BuildingPlacer.Instance.SetRefOBJ(gameObject);


            CurrentTileAndOBJ();
        }
    }
    IEnumerator WaitAndSetup()
    {
        yield return null; // 1프레임 대기
        _uI_BuildAction = BuildingPlacer.Instance.uI_BuildAction;
        if (_uI_BuildAction != null)
        {
            _uI_BuildAction.SetActive(true);
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            _uI_BuildAction.transform.position = screenPos;
        }
    }
    #endregion
    //그리드 적용 스냅
    private Vector3 GetSnappedPosition(Vector3 targetPos)
    {
        float x = Mathf.Round(targetPos.x / gridSize) * gridSize + _offsetx;
        float z = Mathf.Round(targetPos.z / gridSize) * gridSize + _offsety;
        float y = targetPos.y + heightOffset;

        return new Vector3(x, y, z);
    }

    #region 타일 판별 
    //건물밑 타일 판별후 정보전달
    public void CheckTilesUnderBuilding()
    {
        _offsetx = (buildSize.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (buildSize.y % 2 == 0) ? (gridSize / 2f) : 0f;
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / testx, 0.1f, buildSize.y / testy);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);

        int allcheck = 0;
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Tile"))
            {
                var tile = col.GetComponent<TileObjectData>();
                if (tile.isLoadBuild) allcheck++;
            }
        }
        isBuild = allcheck == hitColliders.Length ? true : false;
        if (_isBuildColor != null) _isBuildColor.SetIsBUildColor(isBuild);

    }

    //타일에 isLoadBuild값 전달후 SetTile()호출해 arrayMapPos맵데이터 갱신
    public void SetTileIsBuild()
    {
        _offsetx = (buildSize.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (buildSize.y % 2 == 0) ? (gridSize / 2f) : 0f;
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / testx, 0.1f, buildSize.y / testy);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);
        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Tile"))
            {
                var tile = col.GetComponent<TileObjectData>();
                tile.isLoadBuild = true;
                tile.SetTile();
            }
        }
    }


    //현재 오브젝트,그 밑 타일 정보 받기
    public void CurrentTileAndOBJ()
    {
        _offsetx = (buildSize.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (buildSize.y % 2 == 0) ? (gridSize / 2f) : 0f;
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / testx, 0.1f, buildSize.y / testy);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);
        BuildingPlacer.Instance.TempCollider = hitColliders.ToArray();
        TempOBJ = gameObject;
    }
    public void SnapToGrid(Vector3 targetPos)
    {
        Vector3 snappedPos = GetSnappedPosition(targetPos);
        transform.position = snappedPos;
        CheckTilesUnderBuilding();
    }
    #endregion
    #region 기즈모
    //씬에서 기즈모 보여주기용
    void OnDrawGizmosSelected()
    {
        _offsetx = (buildSize.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (buildSize.y % 2 == 0) ? (gridSize / 2f) : 0f;
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / testx, 0.1f, buildSize.y / testy);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(center, halfExtents * 2f);
    }
    #endregion
}
