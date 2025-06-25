using UnityEngine;

public class DraggableObject : MonoBehaviour, IDraggable
{
    public GameObject BuildActiontUI;
    [SerializeField] private float gridSize = 1f;         // 한 칸 크기
    [SerializeField] private float heightOffset = 0.5f;   // 바닥 위 높이

    float offsetx;
    float offsety;
    public void OnDragStart(Vector3 hitPos)
    {
        offsetx = (gameObject.transform.localScale.x % 2 == 0) ? (gridSize / 2f) : 0f;
        offsety = (gameObject.transform.localScale.z % 2 == 0) ? (gridSize / 2f) : 0f;
        CheckTilesUnderBuilding();

    Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position);
    BuildActiontUI.transform.position = screenPos;
    }

    public void OnDrag(Vector3 groundPos)
    {

        Vector3 snappedPos = GetSnappedPosition(groundPos);
        transform.position = snappedPos;
        CheckTilesUnderBuilding();

    Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main,transform.position);
    BuildActiontUI.transform.position = screenPos;
    }

    public void OnDragEnd() { }

    private Vector3 GetSnappedPosition(Vector3 targetPos)
    {
        float x = Mathf.Round(targetPos.x / gridSize) * gridSize + offsetx;
        float z = Mathf.Round(targetPos.z / gridSize) * gridSize + offsety;
        float y = targetPos.y + heightOffset;

        return new Vector3(x, y, z);
    }

    [SerializeField] private Vector2 buildSize = new Vector2(1f, 1f); // 건축물 밑면 크기 (x, z)
    [SerializeField] private LayerMask tileLayer;

    void CheckTilesUnderBuilding()
    {
        Vector3 center = transform.position + Vector3.down * 0.5f;
        Vector3 halfExtents = new Vector3(buildSize.x / 2f, 0.1f, buildSize.y / 2f); // 높이는 살짝만

        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, tileLayer);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Tile"))
            {
                var tile = col.GetComponent<TileObjectData>();
                tile.isCurrentbuild = true;
            }
        }
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
