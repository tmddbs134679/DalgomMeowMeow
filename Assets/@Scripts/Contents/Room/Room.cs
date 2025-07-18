using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector3 GridPosition { get; private set; }
    private bool isLocked = true;

    public void Init(Vector3 gridPos)
    {
        GridPosition = gridPos;
        SetLocked(true);
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
    }

    private void OnMouseDown()
    {
        if (isLocked)
        {
            Debug.Log($"Room {GridPosition} is locked.");
        }
        else
        {
            Debug.Log($"Room {GridPosition} entered!");
            //Managers.Room.UnlockFrom(this, 0);
        }
    }
}
