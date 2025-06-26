using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BattleCharacter
{
    public override void Die()
    {
        base.Die();
        //BattleManager.Instance._enemyCount; // 적 사망 시 BattleManager에 알림
        Debug.Log($"{name}이(가) 사망했습니다.");
    }
}
