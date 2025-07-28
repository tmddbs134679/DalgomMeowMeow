using System.Collections;
using UnityEngine;

public class PlayerCharacter : BattleCharacter
{
    private BattleManager _battleManager;

    public Material CharacterMaterial;
    public float OutlineWidth;
    public bool victory = false;

    protected override void Awake()
    {
        base.Awake();
        
        _characterRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        

        
    }
    protected override void Start()
    {
        
        base.Start();
        _battleManager = GetComponentInParent<BattleManager>();
        _originalPosition = transform.localPosition;
        if (IsDead)
        {
            return; // 이미 죽은 상태라면 Start를 실행하지 않음
        }
        string numberPart = SkillID.Replace("K","").Replace(".sprite",""); // 시작하면 숫자 파싱
        Skillnum = int.Parse(numberPart);
    }

    public void SetOutline()
    {
        CharacterMaterial = CharacterObject.GetComponentInChildren<SkinnedMeshRenderer>().material; // 캐릭터 머티리얼 초기화
        OutlineWidth = CharacterMaterial.GetFloat("_Outline_Width"); // 아웃라인 너비 초기화
        CharacterMaterial.SetFloat("_Outline_Width", 0f);
    }

    public void ReturnToStartPosition()
    {
        Vector3 worldTarget = transform.parent.TransformPoint(_originalPosition);
        Agent.isStopped = false; // NavMeshAgent가 이동을 시작할 수 있도록 설정
        Agent.SetDestination(worldTarget);
        Animator.SetInteger(AnimationHash, 4);
        HasLookedForward = false; // 시작 위치로 돌아가면 전방을 바라보지 않음
        Agent.speed = MoveSpeed * 3;
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

    public override void Die()
    {
        _battleManager.PlayerCount--;
        base.Die();
    }
}
