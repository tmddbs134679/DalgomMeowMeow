using UnityEngine;
using System;
using System.Linq;
using System.Collections; 
/// <summary>
/// 건물 드래그앤드롭,그리드 스냅,건물 밑 타일 정보 반영
/// </summary>
public class DraggableObject : MonoBehaviour, IDraggable
{

    public UI_BuildAction _uI_BuildAction;
    public BuildMap buildMap;
    public bool isBuild;
    public bool isDrag=false;
    public bool isLongPress = true;
    [SerializeField] private float gridSize = 1f;         // 한 칸 크기
    [SerializeField] private float heightOffset = 0.5f;   // 바닥 위 높이

    private float _offsetx;
    private float _offsety;
private Vector3 _dragOffset;


    public GameObject TempOBJ;

    private IsBuildColor _isBuildColor;

    //드래그 스타트
    public void OnDragStart(Vector3 hitPos)
    {
        BuildingPlacer.Instance.tempDraggleOBJ = this;
        _offsetx = (gameObject.transform.localScale.x % 2 == 0) ? (gridSize / 2f) : 0f;
        _offsety = (gameObject.transform.localScale.z % 2 == 0) ? (gridSize / 2f) : 0f;
        isBuild = CheckTilesUnderBuilding();
        _isBuildColor = GetComponent<IsBuildColor>();
    }

    //드래그
    public void OnDrag(Vector3 groundPos)
    {
        if (isDrag)
        {
            Vector3 snappedPos = GetSnappedPosition(groundPos);
            transform.position = snappedPos;
            isBuild = CheckTilesUnderBuilding();
                       if (_isBuildColor != null) _isBuildColor.SetIsBUildColor(isBuild);

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            if (_uI_BuildAction != null)
                _uI_BuildAction.transform.position = screenPos;
        }
    }

    //드래그 드롭
    public void OnDragEnd() { }

    public void OnLongPress()
    {
        //버그수정중
        if (isLongPress)
        {
                    BuildingPlacer.Instance.tempDraggleOBJ = this;
            isLongPress = false;
            BuildingPlacer.Instance.isSelect = true;
            isDrag = true;
            Debug.Log(this + "롱프레스 감지!");
            Managers.UI.ShowPopupUI<UI_SaveMoveBuild>();
            StartCoroutine(WaitAndSetup());
            //건물설치함수 불러오기
            BuildingPlacer.Instance.SetTempOBJ(gameObject);
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
    //그리드 적용 스냅
    private Vector3 GetSnappedPosition(Vector3 targetPos)
    {
        float x = Mathf.Round(targetPos.x / gridSize) * gridSize + _offsetx;
        float z = Mathf.Round(targetPos.z / gridSize) * gridSize + _offsety;
        float y = targetPos.y + heightOffset;

        return new Vector3(x, y, z);
    }

    [SerializeField] private Vector2 buildSize = new Vector2(1f, 1f); // 건축물 밑면 크기 (x, z)
    [SerializeField] private LayerMask tileLayer;

    //건물밑 타일 판별후 정보전달
    bool CheckTilesUnderBuilding()
    {
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2.5f, 0.1f, buildSize.y / 2.5f);

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
        return allcheck == hitColliders.Length;
    }

//타일에 isLoadBuild값 전달후 SetTile()호출해 arrayMapPos맵데이터 갱신
    public void SetTileIsBuild()
    {
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2.5f, 0.1f, buildSize.y / 2.5f);

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
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2.5f, 0.1f, buildSize.y / 2.5f);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);
       BuildingPlacer.Instance.TempCollider = hitColliders.ToArray();
                     TempOBJ = gameObject;
    }

    //씬에서 기즈모 보여주기용
    void OnDrawGizmosSelected()
    {
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2f, 0.1f, buildSize.y / 2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, halfExtents * 2f);
    }

}
