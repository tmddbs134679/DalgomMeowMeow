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
        public int DataId;
        public string PrefabLabel;
        public float MaxHp;
        public float Hp;
        public float Atk;
        public float Stamina;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float MoveSpeedRate;
        public string IconLabel;
    }

    [Serializable]

    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureData> creatures = new List<CreatureData>();

        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
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
        public int Price;
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
}
