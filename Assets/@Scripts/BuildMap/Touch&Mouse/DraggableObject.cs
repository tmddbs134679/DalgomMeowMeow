using UnityEngine;

public class DraggableObject : MonoBehaviour, IDraggable
{
    [SerializeField] private float gridSize = 1f;         // 한 칸 크기
    [SerializeField] private float heightOffset = 0.5f;   // 바닥 위 높이

    float offsetx;
    float offsety;
    public void OnDragStart(Vector3 hitPos)
    {
                offsetx = (gameObject.transform.localScale.x % 2 == 0) ? (gridSize / 2f) : 0f;
        offsety = (gameObject.transform.localScale.z % 2 == 0) ? (gridSize / 2f) : 0f;
    }

    public void OnDrag(Vector3 groundPos)
    {

        Vector3 snappedPos = GetSnappedPosition(groundPos);
        transform.position = snappedPos;
    }

    public void OnDragEnd() { }

    private Vector3 GetSnappedPosition(Vector3 targetPos)
    {     
        float x = Mathf.Round(targetPos.x / gridSize) * gridSize +offsetx;
        float z = Mathf.Round(targetPos.z / gridSize) * gridSize +offsety;
        float y = targetPos.y + heightOffset;

        return new Vector3(x, y, z);
    }
}
