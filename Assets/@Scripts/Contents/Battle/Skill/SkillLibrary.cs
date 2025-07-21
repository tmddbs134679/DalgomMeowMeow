using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SkillLibrary : MonoBehaviour
{
    private Dictionary<int, Func<BattleCharacter, IEnumerator>> _skillMap;
    private Dictionary<int, Coroutine> _runningCoroutines = new();
    private LayerMask _playerCharacter;

    private void Awake()
    {
        _playerCharacter = LayerMask.GetMask("Player"); // 플레이어 캐릭터 레이어 마스크 설정
        _skillMap = new Dictionary<int, Func<BattleCharacter, IEnumerator>>
        {
            { 1001, HealDance },
            { 1002, NyangPunch },
            { 1003, AttackSpeedBuff },
            { 1004, Bigger },
            { 1005, RangedAttack },
            { 1006, Invincible },
            { 1007, TeamInvincible },
            { 1008, Rain },
            { 1009, BearforceSmash },
            { 1010, DebuffAtk },
            { 1011, Clone },
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
            targetCharacter.Animator.SetInteger(targetCharacter.SkillHash, skillNum); // 스킬 애니메이션 출력
            Coroutine co = StartCoroutine(coroutineFunc(targetCharacter));  //코루틴 실행
            _runningCoroutines[skillNum] = co;  //실행중인 코루틴 넣기
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
        battleCharacter.SkillCooldown = 10f; // 스킬 쿨타임 설정
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
        yield return StartCoroutine(Heal(battleCharacter.transform.position)) ;
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }
    
    private IEnumerator Heal(Vector3 position)
    {
        for (int i = 0; i < 4; i++) // 총 4초간 (0.5초마다 힐)
        {
            Collider[] hits = Physics.OverlapSphere(position, 10, _playerCharacter);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BattleCharacter>(out var character))
                {
                    character.HpControl((character.MaxHP/5)*-1);
                }
            }

            yield return new WaitForSeconds(1f); // 힐 딜레이
        }
    }

    #endregion


    #region NyangPunch

    private IEnumerator NyangPunch(BattleCharacter battleCharacter)
    {
        if (battleCharacter.TargetLocation != null)
        {
            battleCharacter.UsingSkill = true;
            battleCharacter.SkillCooldown = 5f; // 스킬 쿨타임 설정
            yield return StartCoroutine(battleCharacter._effectManager.Punch(battleCharacter.TargetLocation.position));
            battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
            Collider[] hits = Physics.OverlapSphere(battleCharacter.TargetLocation.position, 2.5f, LayerMask.GetMask("Enemy")); // 적 캐릭터 레이어 마스크 사용
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BattleCharacter>(out var targetCharacter))
                {
                    targetCharacter.HpControl(battleCharacter.AttackDamage*5);
                }
            }
            battleCharacter.UsingSkill = false;
        }
    }

    #endregion 


    #region ASBoost
    private IEnumerator AttackSpeedBuff(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 15f; // 스킬 쿨타임 설정
        battleCharacter.AttackDelay /= 2f; // 공격 딜레이를 절반으로 줄임
        battleCharacter.Animator.speed = 2f; // 애니메이션 속도를 두 배로 증가시킴
        battleCharacter.UsingSkill = false;
        yield return StartCoroutine(battleCharacter._effectManager.FireHand(battleCharacter.LeftHandPivot, battleCharacter.RightHandPivot)); // FireHand 효과 실행
        battleCharacter.UsingSkill = true;
        battleCharacter.AttackDelay *= 2f; // 공격 딜레이를 원래대로 되돌림
        battleCharacter.Animator.speed = 1f; // 애니메이션 속도를 원래대로 되돌림
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }
    #endregion


    #region Giant

    private IEnumerator Bigger(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 25f; // 스킬 쿨타임 설정
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
        yield return StartCoroutine(Grow(battleCharacter)); // 캐릭터 크기 증가 코루틴 실행
        battleCharacter.AttackDamage *= 2f; // 공격력 2배 증가
        battleCharacter.AttackRange *= 4f; // 공격 범위 2배 증가
        battleCharacter.Agent.stoppingDistance = battleCharacter.AttackRange;
        battleCharacter.UsingSkill = false;
        yield return new WaitForSeconds(10f); // 1초 대기
        battleCharacter.UsingSkill = true;
        yield return StartCoroutine(Shrink(battleCharacter)); // 캐릭터 크기 감소 코루틴 실행
        battleCharacter.AttackDamage /= 2f; // 공격력 원래대로 되돌림
        battleCharacter.AttackRange /= 4f; // 공격 범위 원래대로 되돌림
        battleCharacter.Agent.stoppingDistance = battleCharacter.AttackRange;
        battleCharacter.UsingSkill = false;
    }

    private IEnumerator Grow(BattleCharacter battleCharacter)
    {
        while (battleCharacter.CharacterObject.transform.localScale.x < 2f) // 최대 크기 2배
        {
            battleCharacter.CharacterObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); // 캐릭터 크기 증가
            battleCharacter._hpBar.offset.y += 0.15f;
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
    }

    private IEnumerator Shrink(BattleCharacter battleCharacter)
    {
        while (battleCharacter.CharacterObject.transform.localScale.x > 1f) // 원래 크기로 되돌리기
        {
            battleCharacter.CharacterObject.transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f); // 캐릭터 크기 감소
            battleCharacter._hpBar.offset.y -= 0.15f;
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
        battleCharacter.CharacterObject.transform.localScale = Vector3.one; // 최종적으로 원래 크기로 설정
    }


    #endregion


    #region RangedAttack

    private IEnumerator RangedAttack(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 20f; // 스킬 쿨타임 설정
        battleCharacter.AttackRange *= 10f; // 사거리 6배 증가
        battleCharacter.Agent.stoppingDistance = battleCharacter.AttackRange;
        battleCharacter.UsingSkill = false;
        yield return new WaitForSeconds(5f);
        battleCharacter.UsingSkill = true;
        battleCharacter.AttackRange /= 10f; // 사거리 원래대로 되돌림
        battleCharacter.Agent.stoppingDistance = battleCharacter.AttackRange;
        battleCharacter.UsingSkill = false;
    }

    #endregion


    #region Invincible
    private IEnumerator Invincible(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 15f; // 스킬 쿨타임 설정
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
        battleCharacter.Invincible = true; // 죽지 않도록 설정
        battleCharacter.UsingSkill = false;
        yield return StartCoroutine(battleCharacter._effectManager.Invincibility(battleCharacter.transform)); // 3초 동안 무적 상태 유지
        battleCharacter.UsingSkill = true;
        battleCharacter.Invincible = false; // 무적 상태 해제
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }
    #endregion


    #region TeamInvincible
    private IEnumerator TeamInvincible(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 25f; // 스킬 쿨타임 설정
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger);
        yield return new WaitForSeconds(1f); // 스킬 애니메이션 딜레이
        Collider[] hits = Physics.OverlapSphere(battleCharacter.transform.position, 5f, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BattleCharacter>(out var teamCharacter))
            {
                teamCharacter.Invincible = true;
                StartCoroutine(battleCharacter._effectManager.Invincibility(teamCharacter.transform)); // 팀원에게 무적 효과 적용
            }
        }
        battleCharacter.UsingSkill = false;
        yield return new WaitForSeconds(3f);
        battleCharacter.UsingSkill = true;
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BattleCharacter>(out var teamCharacter))
            {
                teamCharacter.Invincible = false;
            }
        }
        battleCharacter.UsingSkill = false;
    }
    #endregion


    #region Rain

    private IEnumerator Rain(BattleCharacter battleCharacter)
    {
        if (battleCharacter.TargetLocation != null)
        {
            battleCharacter.UsingSkill = true;
            battleCharacter.SkillCooldown = 20f; // 스킬 쿨타임 설정
            battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
            StartCoroutine(battleCharacter._effectManager.Rain(battleCharacter.TargetLocation.position));
            yield return StartCoroutine(RainDamage(battleCharacter, battleCharacter.TargetLocation.position)); // RainDamage 코루틴 실행
            battleCharacter.UsingSkill = false;
        }
    }

    private IEnumerator RainDamage(BattleCharacter battleCharacter, Vector3 pos)
    {
        for (int i = 0; i < 5; i++) // 5초 동안 지속
        {
            Collider[] hits = Physics.OverlapSphere(pos, 2.5f, LayerMask.GetMask("Enemy")); // 적 캐릭터 레이어 마스크 사용
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<BattleCharacter>(out var targetCharacter))
                {
                    targetCharacter.HpControl(battleCharacter.AttackDamage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }


    #endregion


    #region BearSmash
    private IEnumerator BearforceSmash(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 10f;
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger);

        yield return new WaitForSeconds(0.3f); // 타이밍 맞춤

        Vector3 center = battleCharacter.transform.position + battleCharacter.transform.forward * 2f;
        Collider[] enemies = Physics.OverlapSphere(center, 3f, LayerMask.GetMask("Enemy"));
        foreach (var hit in enemies)
        {
            if (hit.TryGetComponent(out BattleCharacter enemy))
            {
                enemy.HpControl(battleCharacter.AttackDamage * 2f);

                Vector3 dir = (enemy.transform.position - battleCharacter.transform.position).normalized;

                enemy.Agent.isStopped = true;
                enemy.Agent.Move(dir * 3f); // 넉백
                StartCoroutine(ResumeMovement(enemy, 0.5f));
                //battleCharacter._effectManager.ShockWave(center); // 시각 이펙트
            }
        }

        yield return new WaitForSeconds(0.5f);
        battleCharacter.UsingSkill = false;
    }
    private IEnumerator ResumeMovement(BattleCharacter character, float delay)
    {
        yield return new WaitForSeconds(delay);
        character.Agent.isStopped = false;
    }
    #endregion


    #region AtkDown
    private IEnumerator DebuffAtk(BattleCharacter battleCharacter)
    {
        if (battleCharacter.TargetLocation != null)
        {
            battleCharacter.UsingSkill = true;
            battleCharacter.SkillCooldown = 20f; // 스킬 쿨타임 설정
            battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger); // 스킬 애니메이션 트리거 활성화
            StartCoroutine(battleCharacter._effectManager.Debuff(battleCharacter.TargetLocation.position)); //이펙트
            AtkDown(battleCharacter.TargetLocation.position); // 효과
            battleCharacter.UsingSkill = false;
        }
        yield return null;
    }
    public void AtkDown(Vector3 pos)
    {
        Collider[] hits = Physics.OverlapSphere(pos, 2.5f, LayerMask.GetMask("Enemy")); // 적 캐릭터 레이어 마스크 사용
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BattleCharacter>(out var targetCharacter))
            {
                StartCoroutine(Debuffplay(targetCharacter)); // 디버프 플레이 코루틴 실행
            }
        }
    }

    private IEnumerator Debuffplay(BattleCharacter battleCharacter)
    {
        battleCharacter.AttackDamage /= 2f; // 공격력 절반 감소
        yield return new WaitForSeconds(10f);
        battleCharacter.AttackDamage *= 2f; // 공격력 원래대로 되돌림
    }


    #endregion


    #region Clone
    private IEnumerator Clone(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 40f; // 스킬 쿨타임 설정
        yield return new WaitForSeconds(0.5f); // 스킬 애니메이션 딜레이
        GameObject clone = Instantiate(battleCharacter._effectManager.Clone, battleCharacter.transform.position, Quaternion.identity); // 클론 이펙트 생성
        clone.GetComponent<CloneActivity>().Init(battleCharacter.Health*0.5f, battleCharacter.AttackDamage*0.75f, battleCharacter.MoveSpeed*1.2f, battleCharacter.AttackRange, battleCharacter.transform.position); // 클론 초기화
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
        yield return new WaitForSeconds(20f);
        Destroy(clone);
    }
    #endregion


    #region TeamAtkup
    private IEnumerator TeamAtkup(BattleCharacter battleCharacter)
    {
        battleCharacter.UsingSkill = true;
        battleCharacter.SkillCooldown = 25f; // 스킬 쿨타임 설정
        battleCharacter.Animator.SetTrigger(battleCharacter.SkillTrigger);
        yield return new WaitForSeconds(1f); // 스킬 애니메이션 딜레이
        Collider[] hits = Physics.OverlapSphere(battleCharacter.transform.position, 5f, LayerMask.GetMask("Player"));
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<BattleCharacter>(out var teamCharacter))
            {
                StartCoroutine(Atkup(teamCharacter)); // 팀원에게 무적 효과 적용
            }
        }
        battleCharacter.UsingSkill = false;
    }

    private IEnumerator Atkup(BattleCharacter teamCharacter)
    {
        teamCharacter.AttackDamage *= 1.5f; // 공격력 1.5배 증가
        yield return StartCoroutine(teamCharacter._effectManager.); // 10초 동안 지속
        teamCharacter.AttackDamage /= 1.5f; // 공격력 원래대로 되돌림
    }
    #endregion
}