using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStat", menuName = "ScriptableObjects/CharacterStat")]
public class CharacterStatSo : ScriptableObject
{
    public event Action OnStatChanged;

    public int DataId;
    public string PrefabLabel;
    public string IconLabel;

    [SerializeField]
    private float maxHp;
    public float MaxHp
    {
        get => maxHp;
        set
        {
            if (Mathf.Approximately(maxHp, value)) return;
            maxHp = value;
            OnStatChanged?.Invoke();
        }
    }

    [SerializeField]
    private float hp;
    public float Hp
    {
        get => hp;
        set
        {
            if (Mathf.Approximately(hp, value)) return;
            hp = value;
            OnStatChanged?.Invoke();
        }
    }

    [SerializeField]
    private float atk;
    public float Atk
    {
        get => atk;
        set
        {
            if (Mathf.Approximately(atk, value)) return;
            atk = value;
            OnStatChanged?.Invoke();
        }
    }
    [SerializeField]
    private float stamina;

    public float Stamina
    {
        get => stamina;
        set
        {
            if (Mathf.Approximately(stamina, value)) return;
            stamina = value;
            OnStatChanged?.Invoke();
        }
    }


    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            if (Mathf.Approximately(moveSpeed, value)) return;
            moveSpeed = value;
            OnStatChanged?.Invoke();
        }
    }

    public float HpRate;
    public float AtkRate;
    public float MoveSpeedRate;

    


    // 추가 애니메이터 데이터 등은 필요 시 확장
}

public static class ScriptableObjectUtils
{
    public static T Clone<T>(this T original) where T : ScriptableObject
    {
        return UnityEngine.Object.Instantiate(original);
    }
   
}
