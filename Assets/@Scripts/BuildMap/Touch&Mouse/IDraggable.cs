using UnityEngine;
public interface IDraggable
{
    void OnDragStart(Vector3 hitPos);
    void OnDrag(Vector3 hitPos);
    void OnDragEnd();
}