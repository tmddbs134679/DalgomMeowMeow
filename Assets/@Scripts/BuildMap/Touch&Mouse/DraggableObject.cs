using UnityEngine;
using System;
using System.Linq;
/// <summary>
/// 건물 드래그앤드롭,그리드 스냅,건물 밑 타일 정보 반영
/// </summary>
public class DraggableObject : MonoBehaviour, IDraggable
{
    public GameObject BuildActiontUI;
    public BuildMap buildMap;
    public BuildingPlacer buildingplacer;
    [SerializeField] private float gridSize = 1f;         // 한 칸 크기
    [SerializeField] private float heightOffset = 0.5f;   // 바닥 위 높이

    public bool isBuild;
    public bool isDrag=false;

    public bool isLongPress = true;
    float offsetx;
    float offsety;
private Vector3 _dragOffset;


    public GameObject TempOBJ;
    public Collider[] TempCollider;

    //드래그 스타트
    public void OnDragStart(Vector3 hitPos)
    {
        offsetx = (gameObject.transform.localScale.x % 2 == 0) ? (gridSize / 2f) : 0f;
        offsety = (gameObject.transform.localScale.z % 2 == 0) ? (gridSize / 2f) : 0f;
        isBuild = CheckTilesUnderBuilding();


    }

    //드래그
    public void OnDrag(Vector3 groundPos)
    {
        if (isDrag)
        {
Debug.Log("드래그 실행됨 : " + isDrag);
            Vector3 snappedPos = GetSnappedPosition(groundPos);
            transform.position = snappedPos;
            isBuild = CheckTilesUnderBuilding();

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            if (BuildActiontUI != null)
                BuildActiontUI.transform.position = screenPos;
        }
    }

    //드래그 드롭
    public void OnDragEnd() { }

    public void OnLongPress()
    {
        if (isLongPress)
        {
            isDrag = true;
            Debug.Log("롱프레스 감지!");
            BuildActiontUI.SetActive(true);
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            BuildActiontUI.transform.position = screenPos;
            //건물설치함수 불러오기
            buildingplacer.SetTempOBJ(gameObject);
            CurrentTileAndOBJ();
        }
    }
    //그리드 적용 스냅
    private Vector3 GetSnappedPosition(Vector3 targetPos)
    {
        float x = Mathf.Round(targetPos.x / gridSize) * gridSize + offsetx;
        float z = Mathf.Round(targetPos.z / gridSize) * gridSize + offsety;
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

//해당 오브젝트 밑 타일 초기화
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

    //현재 오브젝트,그 밑 타일 정보 받기
    public void CurrentTileAndOBJ()
    {
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2.5f, 0.1f, buildSize.y / 2.5f);

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);
        TempCollider = hitColliders.ToArray();
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
