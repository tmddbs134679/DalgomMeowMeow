using UnityEngine;
/// <summary>
/// IDraggable 인터페이스
/// </summary>
public interface IDraggable
{
    void OnDragStart(Vector3 hitPos);
    void OnDrag(Vector3 hitPos);
    void OnDragEnd();
    void OnLongPress();

}