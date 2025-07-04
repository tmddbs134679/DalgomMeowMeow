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
    public Data.CreatureData Data;

    public string Id { get; set; }              //고유 식별
    public string DataId { get; set; }  //정적 데이터 키
    public int Level { get; set; } = 1; //레벨
    public float MaxExp { get; set; }
    public float CurrentExp { get; set; } 
    public EAIState CurrentState { get; set; } = EAIState.Idle;
    public Vector3Data Pos { get; set; } = new Vector3Data();
    public float CurrentStamina { get; set; } 
    public float Hp { get; set; }
    public float MoveSpeed { get; set; } = 3f; // 이동 속도
    public float WalkSpeed { get; set; }

    public List<string> EquippedItemIds { get; set; } = new();

    public Dictionary<EEquipmentType, Equipment> EquippedItems = new();
    public void Init(string id, Vector3 position)
    {
        Id = id;
        Pos = new Vector3Data(position);
        //Level = Data?.Level ?? 1;
        Hp = Data?.MaxHp ?? 100f; // 최대 체력
        MaxExp = Data?.MaxExp ?? 15f; // 최대 경험치    
        //CurrentExp = Data?.curretExp ?? 0; // 현재 경험치
        CurrentState = EAIState.Idle;
        CurrentStamina = Data?.MaxStamina ?? 100f;
        MoveSpeed =  Data?.MoveSpeed ?? 1f;
       // WalkSpeed = Data?.WalkSpeed ?? 1.5f;
        EquippedItemIds = new();
    }

    public void SetInfo(CreatureData data)
    {
        if(data == null)
        {
            Debug.LogError("Character data 없음");
            return;
        }


        Data = data;
        MoveSpeed = Data.MoveSpeed;
        DataId = data.DataId;
    }


}
