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
        public float Level;
        public float MaxExp;
        public float CurrentExp;
        public EAIState CurrentState;
        public float MaxHp;
        public float Atk;
        public float MaxStamina;
        public float CurrentStamina;
        public float MoveSpeed;
        public float HpRate;
        public float AtkRate;
        public float MoveSpeedRate;
        public string IconLabel;
        public string SkillIcon;
        public float WalkSpeed;

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
        public int Count;
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

    #region GachaData
    [Serializable]
    public class GachaData
    {
        public string DataId;
        public float Probability;
    }

    [Serializable]
    public class GachaDataLoader : ILoader<string, GachaData>
    {
        public List<GachaData> Gachas = new();

        public Dictionary<string, GachaData> MakeDict()
        {
            Dictionary<string, GachaData> dict = new();
            foreach (var gacha in Gachas)
            {
                dict[gacha.DataId] = gacha;
            }
            return dict;
        }
    }
    #endregion

    #region CheckOut

    public class CheckOutData
    {
        public int Day;
        public int RewardItemId;
        public int RewardItemValue;
    }

    [Serializable]
    public class CheckOutDataLoader : ILoader<int, CheckOutData>
    {
        public List<CheckOutData> checkouts = new List<CheckOutData>();

        public Dictionary<int, CheckOutData> MakeDict()
        {
            Dictionary<int, CheckOutData> dict = new Dictionary<int, CheckOutData>();
            foreach (CheckOutData checkOut in checkouts)
                dict.Add(checkOut.Day, checkOut);
            return dict;
        }
    }
    #endregion

    #region Material

    public class MaterialData
    {
        public int DataId;
        public Define.EMaterialType MaterialType;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> Materials = new List<MaterialData>();

        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData material in Materials)
                dict.Add(material.DataId, material);
            return dict;
        }
    }
    #endregion


    #region EquipmentGacha

    public class EquipmentGachaData
    {
        public string EquipmentID;
        public float GachaRate;
        public EEquipmentGrade Grade;
    }

    [Serializable]
    public class EquipmentGachaDataLoader : ILoader<string, EquipmentGachaData>
    {
        public List<EquipmentGachaData> EquipmentGachaTable = new List<EquipmentGachaData>();

        public Dictionary<string, EquipmentGachaData> MakeDict()
        {
            Dictionary<string, EquipmentGachaData> dict = new Dictionary<string, EquipmentGachaData>();
            foreach (EquipmentGachaData gacha in EquipmentGachaTable)
                dict.Add(gacha.EquipmentID, gacha);
            return dict;
        }
    }
    #endregion


    #region EquipmentGacha

    public class SkillData
    {
        public string DataId;
        public string Name;
        public string Description;
        public float CoolTime;
    }

    [Serializable]
    public class SkillDataDataLoader : ILoader<string, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<string, SkillData> MakeDict()
        {
            Dictionary<string, SkillData> dict = new Dictionary<string, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataId, skill);
            return dict;
        }
    }
    #endregion

    #region QuestData

    public class QuestData
    {
        public string QuestId;
        public string Title;
        public EQuestType QuestType;
        public EQuestConditionType QuestConditionType;
        public ETargetType TargetType;
        public int GoalCount;
        public int Reward;
        public string PreviousQuest;
    }

    [Serializable]
    public class QuestDataLoader : ILoader<string, QuestData>
    {
        public List<QuestData> quests = new List<QuestData>();
    
        public Dictionary<string, QuestData> MakeDict()
        {
            Dictionary<string, QuestData> dict = new Dictionary<string, QuestData>();
            foreach (QuestData quest in quests)
                dict.Add(quest.QuestId, quest);
            return dict;
        }
    }
    #endregion
}
