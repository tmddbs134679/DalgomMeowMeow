using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BattleCharacter
{
    private BattleManager _battleManager;
    private void Start()
    {
        _battleManager = GetComponentInParent<BattleManager>();
    }
    public override void Die()
    {
        _battleManager.EnemyCount--;
        Debug.Log("적 캐릭터 사망: " + name);
        base.Die();
    }
}
