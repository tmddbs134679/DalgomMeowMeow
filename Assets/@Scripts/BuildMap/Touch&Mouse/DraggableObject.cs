using UnityEngine;
public class DraggableObject : MonoBehaviour, IDraggable
{
    private float fixedHeightOffset = 0.5f; // 바닥에서 띄울 높이

    public void OnDragStart(Vector3 hitPos) { }

    public void OnDrag(Vector3 groundPos)
    {
        transform.position = new Vector3(groundPos.x, groundPos.y + fixedHeightOffset, groundPos.z);
    }

    public void OnDragEnd() { }
}
