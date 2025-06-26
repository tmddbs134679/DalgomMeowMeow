using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : BattleCharacter
{


    public void ReturnToStartPosition()
    {
        Vector3 worldTarget = transform.parent.TransformPoint(_originalPosition);
        Agent.isStopped = false; // NavMeshAgent가 이동을 시작할 수 있도록 설정
        Agent.SetDestination(worldTarget);
        Animator.SetInteger("animation", 4);
        HasLookedForward = false; // 시작 위치로 돌아가면 전방을 바라보지 않음
        Agent.speed = MoveSpeed * 3;
    }


    public Vector3 GetOriginalWorldPosition()
    {
        return this.transform.position;
    }

    public void SmoothLookForward(Vector3 direction, float duration = 0.3f)
    {
        StartCoroutine(RotateSmoothly(direction, duration));
    }

    private IEnumerator RotateSmoothly(Vector3 targetDirection, float duration)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(targetDirection);
        float time = 0f;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
    }
}
