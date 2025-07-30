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

    
}
