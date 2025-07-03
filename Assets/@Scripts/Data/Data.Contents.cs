using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace Data
{

    #region CreatureData
    public class CreatureData
    {
        public string DataId;
        public string PrefabLabel;
        public string Name;
        public int Level;
        public float MaxExp;
        public float curretExp;
        public float MaxHp;
        public float Atk;
        public float MaxStamina;
        public float MoveSpeed;
        public float WalkSpeed;
        public float HpRate;
        public float AtkRate;
        public float MoveSpeedRate;
        public string IconLabel;
        public List<string> SkillTypeList;

    }

    [Serializable]

    public class CreatureDataLoader : ILoader<string, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();

        public Dictionary<string, CreatureData> MakeDict()
        {
            Dictionary<string, CreatureData> dict = new Dictionary<string, CreatureData>();
            foreach(CreatureData creature in creatures)
                dict.Add(creature.DataId, creature);
            return dict;
        }
    }
    #endregion

    #region FoodData

    [Serializable]
    public class FoodData
    {
        public string DataId;
        public string Name;
        public string Description;
        public string SpriteName;
        public float Price;
    }

    [Serializable]
    public class FoodDataLoader : ILoader<string, FoodData>
    {
        public List<FoodData> foods = new List<FoodData>();

        public Dictionary<string, FoodData> MakeDict()
        {
            Dictionary<string, FoodData> dict = new Dictionary<string, FoodData>();
            foreach (FoodData food in foods)
                dict.Add(food.DataId, food);
            return dict;
        }
    }

    #endregion


    #region BuildingData

    [Serializable]
    public class BuildingData
    {
        public string DataId;
        public string Name;
        public Define.EBuildingType Type;
        public string Description;
        public float BuildTime;
        public float ProductionTime;
        public Vector2Int Size;
        public string DataName;
        public int UnlockLevel;
        public int MaxLevel;
        public int Exp;
    }

    [Serializable]
    public class BuildingDataLoader : ILoader<string, BuildingData>
    {
        public List<BuildingData> buildings = new List<BuildingData>();

        public Dictionary<string, BuildingData> MakeDict()
        {
            Dictionary<string, BuildingData> dict = new Dictionary<string, BuildingData>();
            foreach (BuildingData build in buildings)
                dict.Add(build.DataId, build);
            return dict;
        }
    }


    #endregion

    #region Equipment

    public class EquipmentData
    {
        public string DataId;
        public EEquipmentType EquipmentType;
        public string Name;
        public string Description;
        public string SpriteName;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> Equipments = new List<EquipmentData>();

        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
            foreach (EquipmentData equipment in Equipments)
                dict.Add(equipment.DataId, equipment);
            return dict;
        }
    }
    #endregion

    #region BuildingLevelData

    [Serializable]
    public class BuildingLevelData
    {
        public string BuildingId;
        public int Level;
        public float ProductionTime;
        public int Capacity;
        public int UpgradeCost;
        public string ProducedFoodId;

        [NonSerialized] public FoodData ProducedFood; // 런타임 연결
    }

    [Serializable]
    public class BuildingLevelDataLoader : ILoader<(string, int), BuildingLevelData>
    {
        public List<BuildingLevelData> levels = new List<BuildingLevelData>();

        public Dictionary<(string, int), BuildingLevelData> MakeDict()
        {
            var dict = new Dictionary<(string, int), BuildingLevelData>();
            foreach (var data in levels)
            {
                dict.Add((data.BuildingId, data.Level), data);
            }
            return dict;
        }
    }


    #endregion
}
