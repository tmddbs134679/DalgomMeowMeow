using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillLibrary : MonoBehaviour
{
    private Dictionary<int, Func<BattleCharacter, IEnumerator>> _skillMap;
    private Dictionary<int, Coroutine> _runningCoroutines = new();
    private LayerMask _playerCharacter;

    private bool _isHealPlay;
    private bool _isPunchPlay;
    private void Awake()
    {
        _playerCharacter = LayerMask.GetMask("Player"); // 플레이어 캐릭터 레이어 마스크 설정
        _skillMap = new Dictionary<int, Func<BattleCharacter, IEnumerator>>
        {
            { 1, HealDance },
            { 2, NyangPunch },
            { 3, AttackSpeedBuff }
        };
    }


    public void UseSkill(int skillNum, BattleCharacter targetCharacter) //호출할때 넘겨주는 방식
    {
        if (targetCharacter.UsingSkill == true) // 이미 스킬을 사용중이면 실행하지 않음
        {
            Managers.Debug.LogWarning("이미 스킬을 사용중입니다.", Define.EDebugType.AI);
            return;
        }
        if (_skillMap.TryGetValue(skillNum, out var coroutineFunc))
        {
            if (_runningCoroutines.TryGetValue(skillNum, out var running))  //실행된게 있으면 멈추기
            {
                StopCoroutine(running);
            }

            Coroutine co = StartCoroutine(coroutineFunc(targetCharacter));  //코루틴 실행
            _runningCoroutines[skillNum] = co;  //실행중인 코루틴 넣기
            targetCharacter.Animator.SetInteger(targetCharacter.SkillHash, skillNum); // 스킬 애니메이션 출력
        }
        else
        {
            Managers.Debug.LogError($"Skill number {skillNum} not found in skill map.", Define.EDebugType.AI);
        }
    }

    #region Healing
    private IEnumerator HealDance(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.Animator.SetTrigger(battleCharacter.Skill); // 스킬 애니메이션 트리거 활성화
        yield return StartCoroutine(Heal(battleCharacter.transform.position)) ;
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }
    
    private IEnumerator Heal(Vector3 position)
    {
        for (int i = 0; i < 4; i++) // 총 2초간 (0.5초마다 힐)
        {
            Collider[] hits = Physics.OverlapSphere(position, 10, _playerCharacter);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BattleCharacter>(out var character))
                {
                    character.HpControl((character.MaxHP/5)*-1);
                    Managers.Debug.Log($"회복되었습니다.", Define.EDebugType.AI);
                }
            }

            yield return new WaitForSeconds(0.75f); // 힐 딜레이
        }
    }

    #endregion


    #region NyangPunch

    private IEnumerator NyangPunch(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.Animator.SetTrigger(battleCharacter.Skill); // 스킬 애니메이션 트리거 활성화

        yield return StartCoroutine(EffectManager.Instance.Punch(battleCharacter.TargetLocation.position));
        Collider[] hits = Physics.OverlapSphere(battleCharacter.transform.position, 3.5f, LayerMask.GetMask("Enemy")); // 적 캐릭터 레이어 마스크 사용
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                targetCharacter.HpControl(battleCharacter.AttackDamage);
            }
        }
        battleCharacter.UsingSkill = false;
    }

    #endregion


    #region ASBoost
    private IEnumerator AttackSpeedBuff(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter._attackDelay /= 2f; // 공격 딜레이를 절반으로 줄임
        battleCharacter.Animator.speed = 2f; // 애니메이션 속도를 두 배로 증가시킴
        yield return StartCoroutine(EffectManager.Instance.FireHand(battleCharacter.leftHandPivot, battleCharacter.rightHandPivot)); // FireHand 효과 실행
        battleCharacter._attackDelay *= 2f; // 공격 딜레이를 원래대로 되돌림
        battleCharacter.Animator.speed = 1f; // 애니메이션 속도를 원래대로 되돌림
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }
    #endregion
}