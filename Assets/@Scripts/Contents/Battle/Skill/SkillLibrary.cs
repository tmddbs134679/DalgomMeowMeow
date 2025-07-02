using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
            { 1, HealDance },
            //{ 2, IronTail },
            //{ 3, HeadButt }
        };
    }


    public void UseSkill(int skillNum, BattleCharacter targetCharacter) //호출할때 넘겨주는 방식
    {
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
        yield return StartCoroutine(Heal()) ;
        battleCharacter.UsingSkill = false; // 스킬 사용 종료
    }

    private IEnumerator Heal()
    {
        for (int i = 0; i < 4; i++) // 총 2초간 (0.5초마다 힐)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, 10, _playerCharacter);

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
}
