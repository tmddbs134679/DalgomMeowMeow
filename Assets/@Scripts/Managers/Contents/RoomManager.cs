using System.Collections.Generic;
using UnityEngine;

public class RoomManager 
{
    public GameObject roomPrefab;
    public Dictionary<Vector3, Room> rooms = new Dictionary<Vector3, Room>();
    public int UnLockRoom = 1;

    // 기준 방향 (예: 위, 왼쪽 위, 오른쪽 위)



    public Vector3[] directions = new[]
    {
        new Vector3(0,0,0),
        new Vector3(1,1,0),
        new Vector3(0,1,-1),
    };


    //public void UnlockRoom(Vector2Int from, int directionIndex)
    //{
    //    Vector2Int newPos = from + directions[directionIndex];
    //    if (rooms.ContainsKey(newPos)) return;

    //    Room room = CreateRoom(newPos);
    //    room.SetLocked(false);
    //}

    //public void UnlockFrom(Room room, int directionIndex)
    //{
    //    UnlockRoom(room.GridPosition, directionIndex);
    //}
    

    public Room CreateRoom(Vector3 gridPos)
    {
        if(rooms.ContainsKey(gridPos))
        {
            return rooms[gridPos];
        }
        Vector3 worldPos = new Vector3(gridPos.x * 9.5f, gridPos.y * 5f, gridPos.z * 9.5f);
        GameObject go = Managers.Resource.Instantiate("Room", pooling: true);
        go.transform.localPosition = worldPos;

        Room room = go.GetComponent<Room>();
        rooms.Add(gridPos, room);
        return room;
    }
}