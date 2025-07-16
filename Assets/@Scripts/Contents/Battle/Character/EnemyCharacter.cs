using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BattleCharacter
{
    private BattleManager _battleManager;
    protected override void Start()
    {
        base.Start();
        SetAnimation();
    
        _battleManager = GetComponentInParent<BattleManager>();
        _characterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

    }
    public override void Die()
    {
        _battleManager.EnemyCount--;
        base.Die();
    }
}
