using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

public class  Character
{
    public Data.CreatureData Data;

    public string UniqueId { get; set; }              //고유 식별
    public string Name { get; set; }    
    public string DataId { get; set; }  
    public float Level { get; set; }
    public float MaxExp { get; set; }
    public float CurrentExp { get; set; } 
    public EAIState CurrentState { get; set; } 
    public BuildingBase LoadBuilding { get; set; }
    public Vector3Data Pos { get; set; } = new Vector3Data();
    public Vector3Data RoomPos { get; set; } = new Vector3Data(); // 방 내 위치
    public float MaxStamina { get; set; } 
    public float CurrentStamina { get; set; } 
    public float Atk { get; set; } 
    public float Hp { get; set; }
    public float MoveSpeed { get; set; }
    public float WalkSpeed { get; set; } 
    public bool IsConfirmed { get; set; }
    public bool InMainScene { get; set; } 
    public List<string> EquippedItemIds { get; set; } = new();

    public bool IsTravelMode { get; set; }

    public Dictionary<EEquipmentType, Equipment> EquippedItems = new();
    public void Init(string dataid, Vector3 position)
    {
        if (string.IsNullOrEmpty(UniqueId)) // 이미 있으면 덮어쓰지 않음
            UniqueId = $"{dataid}_UID_{Guid.NewGuid().ToString().Substring(0, 8)}";

        DataId = dataid;
        Pos = new Vector3Data(position);
        RoomPos = new Vector3Data(Vector3.zero);
        Hp = Data?.MaxHp ?? 100f; 
        MaxExp = Data?.MaxExp ?? 500f;    
        CurrentExp = Data?.CurrentExp ?? 0; 
        MaxStamina = Data?.MaxStamina ?? 100f; 
        Atk = Data?.Atk ?? 10f;
        CurrentStamina = MaxStamina;
        MoveSpeed =  Data?.MoveSpeed ?? 3f;
        WalkSpeed = Data?.WalkSpeed ?? 1.5f;

        EquippedItemIds = new();
    }

   

    public void SetInfo(CreatureData data)
    {
        if(data == null)
        {
            Debug.LogError("Character data 없음");
            return;
        }


        if(this.Data == null)
        {
            this.Data = data;
            this.Name = data.Name;
            return;
        }

        this.Data = data;

        if (string.IsNullOrEmpty(this.Name))
       {
            this.Name = data.Name;
        }



    }


}
