using Data;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Equipment 
{
    public string key = "";

    public Data.EquipmentData EquipmentData;

    bool _isEquipped = false;
    public bool IsOwned { get; set; } = false;
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

        IsOwned = true;
    }


}
