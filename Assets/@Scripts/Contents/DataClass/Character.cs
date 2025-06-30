using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;


[System.Serializable]
public struct Vector3Data
{
    public float x, y, z;

    public Vector3Data(Vector3 v)
    {
        x = v.x; y = v.y; z = v.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

public class Character
{
    public Data.CreatureData CreatureData;

    public string Id { get; set; }              //고유 식별
    public string DataId { get; set; } = "";    //정적 데이터 키
    public EAIState CurrentState { get; set; } = EAIState.Idle;
    public Vector3Data Pos { get; set; } = new Vector3Data();
    public float CurrentStamina { get; set; } = 1;

    public List<string> EquippedItemIds { get; set; } = new();
    public void Init(string dataId, Vector3 position)
    {
        Id = Guid.NewGuid().ToString();
        DataId = dataId;
        Pos = new Vector3Data(position);
        CurrentState = EAIState.Idle;
        CurrentStamina = 1f;
        EquippedItemIds = new();
    }

    public void SetInfo(CreatureData creatureData)
    {
        CreatureData = creatureData;
    }


}
