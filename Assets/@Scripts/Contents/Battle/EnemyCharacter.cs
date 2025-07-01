using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : BattleCharacter
{
    private BattleManager _battleManager;
    protected override void Start()
    {
        base.Start();

        Animator = GetComponentInChildren<Animator>();
        AnimationHash = Animator.StringToHash("animation"); // 애니메이션 해시 초기화
        _battleManager = GetComponentInParent<BattleManager>();
        _characterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();

    }
    public override void Die()
    {
        _battleManager.EnemyCount--;
        base.Die();
    }
}
