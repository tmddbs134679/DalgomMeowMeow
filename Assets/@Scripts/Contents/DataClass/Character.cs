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
    public string DataId { get; set; }  //정적 데이터 키
    public float Level { get; set; } = 1; //레벨
    public float MaxExp { get; set; }
    public float CurrentExp { get; set; } 
    public EAIState CurrentState { get; set; } 
    public BuildingBase LoadBuilding { get; set; }
    public Vector3Data Pos { get; set; } = new Vector3Data();
    public float MaxStamina { get; set; } 
    public float CurrentStamina { get; set; } 
    public float Atk { get; set; } = 10f; // 공격력
    public float Hp { get; set; }
    public float MoveSpeed { get; set; }
    public float WalkSpeed { get; set; } = 1.5f;
    public bool IsConfirmed { get; set; }
    public List<string> EquippedItemIds { get; set; } = new();

    public Dictionary<EEquipmentType, Equipment> EquippedItems = new();
    public void Init(string dataid, Vector3 position)
    {
        if (string.IsNullOrEmpty(UniqueId)) // 이미 있으면 덮어쓰지 않음
            UniqueId = $"{dataid}_UID_{Guid.NewGuid().ToString().Substring(0, 8)}";

        DataId = dataid;
        Pos = new Vector3Data(position);
        Level = Data?.Level ?? 1;
        Hp = Data?.MaxHp ?? 100f; // 최대 체력
        MaxExp = Data?.MaxExp ?? 15f; // 최대 경험치    
        CurrentExp = Data?.CurrentExp ?? 0; // 현재 경험치
        MaxStamina = Data?.MaxStamina ?? 100f; // 최대 스태미나
        Atk = Data?.Atk ?? 10f; // 공격력
        CurrentStamina = MaxStamina; // 현재 스태미나
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
