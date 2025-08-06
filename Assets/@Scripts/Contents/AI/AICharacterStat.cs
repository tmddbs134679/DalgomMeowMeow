using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class AICharacterStat : MonoBehaviour
{
    AICharacter AICharacter;

    public Character data;


    public void Init(AICharacter _aiCharacter)
    {
        AICharacter = _aiCharacter;

    }


    #region 캐릭터 스탯 관련
    public void OnLevelUp()
    {
        //스탯
        data.MoveSpeed += 0.5f; // 레벨업 시 이동 속도 증가
        data.MoveSpeed = MathF.Min(6, data.MoveSpeed);
        data.MaxExp *= 1.3f; // 레벨업 시 최대 경험치 증가
        data.Atk += 2f; // 레벨업 시 공격력 증가
        data.MaxStamina += 5f; // 레벨업 시 최대 스태미너 증가
        data.Level++;
        if (Managers.Scene.CurrentScene is GameScene)
        {
            AICharacter.Effect.PlayLevelUpEffect(AICharacter.Interaction._camera);
        }

        AICharacter.Levelup?.Invoke(data.Level);

    }

    public void GainExp(int value)
    {
        data.CurrentExp += value;
        while (data.CurrentExp >= data.MaxExp)
        {
            data.CurrentExp -= data.MaxExp;
            OnLevelUp();
        }

        AICharacter.CharacterGainExp?.Invoke(value);
    }

    public void UseStamina(float amount)
    {
        if (data.CurrentStamina - amount < 0)
        {
            return;
        }
        data.CurrentStamina = Mathf.Max(0, data.CurrentStamina - amount);
    }

    public void RecoverStamina(float amount)
    {
        data.CurrentStamina = Mathf.Min(data.MaxStamina, data.CurrentStamina + amount);
    }
    #endregion
}
