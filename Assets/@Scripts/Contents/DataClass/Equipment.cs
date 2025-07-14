using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Equipment 
{
    public string UniqueId { get; set; }

    public string key = "";

    public Data.EquipmentData EquipmentData;

    bool _isEquipped = false;
    public bool IsOwned { get; set; } = false;
    public bool IsConfirmed { get; set; } = false;
    public string EquippedByCharacterId { get; set; } = null;

    public bool IsEquipped
    {
        get
        {
            return _isEquipped;
        }
        set
        {
            _isEquipped = value;
        }
    }

    public Equipment(string key)
    {
        this.key = key;

        EquipmentData = Managers.Data.EquipmentDic[key];

        // UniqueId가 이미 있으면 유지, 없으면 새로 생성
        if (string.IsNullOrEmpty(UniqueId))
        {
            UniqueId = $"EQ_{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
        IsOwned = true;
    }


    public void SetInfo(EquipmentData data)
    {
        EquipmentData = data;
    }

}
