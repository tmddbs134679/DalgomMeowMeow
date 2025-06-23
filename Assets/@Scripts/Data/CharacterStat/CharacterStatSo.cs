using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[CreateAssetMenu(fileName = "CharacterStat", menuName = "ScriptableObjects/CharacterStat")]
public class CharacterStatSo : ScriptableObject
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