using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BattleCharacter
{
    public override void Die()
    {
        Managers.Battle._enemyCount--; // 적 캐릭터가 죽으면 BattleManager의 적 유닛 수 감소
        Debug.Log("적 캐릭터 사망: " + name);
        base.Die();
    }
}
