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


}
