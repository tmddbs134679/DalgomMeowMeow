using Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BattleCharacter : BaseObject
{
    [SerializeField] private float _detectRange = 10f;  
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackDelay = 1f; // 공격 딜레이 (초 단위)
    [SerializeField] private CharacterStatSo _data; // 캐릭터 스탯 데이터
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _flashDuration = 0.05f;
    [SerializeField] private Color _originalColor;  // 원래 색상 (피격 효과를 위해 사용됨)

    public NavMeshAgent Agent { get; private set; } // NavMeshAgent 컴포넌트

    public float AttackDamage = 10f; // 공격력
    public float Health = 100f; // 체력
    public float MoveSpeed = 3.5f; // 이동 속도

    public bool HasLookedForward = true; // 전방을 바라봤는지 여부 (아군 전용 로직)


    public string TargetLayer = "Enemy"; // 타겟 태그
    public bool UsingSkill = false; // 스킬 사용 여부
    

    private float _attacktimer = 0f;
    private Transform _targetLocation;
    private BattleCharacter _targetCharacter; // 현재 타겟 캐릭터

    [SerializeField]protected SkinnedMeshRenderer[] _characterRenderer; // 캐릭터 렌더러
    private Coroutine _damageFlashCoroutine; //피격 효과 코루틴

    private bool _isInBattle = false;

    public Animator Animator; // 애니메이터 컴포넌트
    [SerializeField]protected Vector3 _originalPosition; // 원래 위치 저장
    
    


    public event Action<BattleCharacter> OnCharacterDied;
    public event Action<BattleCharacter, bool> OnBattleStateChanged;



    public int AnimationHash;
    
    public bool IsDead { get; private set; } = false;
   
    public bool IsInBattle { get => _isInBattle;
        set
        {
            if (_isInBattle != value)
            {
                _isInBattle = value;
                OnBattleStateChanged?.Invoke(this, _isInBattle);
            }
        }
    }



    protected virtual void Awake()
    {
        ObjectType = Define.EObjectType.Enemy; // 객체 타입 설정
        Agent = GetComponent<NavMeshAgent>();

    }

    protected virtual void Start()
    {
        _originalPosition = transform.localPosition;
        Agent.speed = MoveSpeed; // NavMeshAgent의 이동 속도 설정
        Agent.stoppingDistance = _attackRange; // 공격 범위 내에서 멈추도록 설정

    }

    void Update()
    {
        if (IsDead) return; // 캐릭터가 죽었으면 업데이트 중지


        if (_targetLocation == null)
        {
            var newTarget = FindClosestEnemyInRange(_detectRange);
            if (newTarget != null)
            {
                SetTarget(newTarget);
                if (_targetLocation != null)
                {
                    Agent.SetDestination(_targetLocation.position);
                    IsInBattle = true; // 타겟이 생기면 전투 상태로 변경
                    Animator.SetInteger(AnimationHash, 5); 
                }
            }
        }
        else
        {
            float dist = Vector3.Distance(transform.position, _targetLocation.position);

            if (dist <= _attackRange)
            {
                Agent.isStopped = true; //거리가 가까우면 공격

                Vector3 direction = (_targetLocation.position - transform.position).normalized;
                direction.y = 0; // y축 회전 배제

                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direction);    //공격시 타겟 바라봄

                TryAttack();
            }
            else
            {
                Agent.isStopped = false;
                Agent.SetDestination(_targetLocation.position); // 계속 추적
            }
        }
    }

    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectRange);
    }

    public void Init(CreatureData data)
    {
        AttackDamage = data.Atk;
        Health = data.MaxHp;
        MoveSpeed = data.MoveSpeed;
    }


    #region FindTarget
    private Transform FindClosestEnemyInRange(float range)      //오버랩 스피어를 통해 적 탐지
    {
        int targetLayerMask = LayerMask.GetMask(TargetLayer);
        Collider[] hits = Physics.OverlapSphere(transform.position, range, targetLayerMask);

        Transform closest = null;
        float minDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (var hit in hits)
        {
            float dist = Vector3.Distance(myPos, hit.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = hit.transform;
            }
        }
        return closest;
    }


    private void SetTarget(Transform newTarget)     //탐지된 적의 죽음이벤트를 구독 or 구독해제
    {
        if (_targetLocation != null && _targetLocation.TryGetComponent<BattleCharacter>(out var oldChar))
            oldChar.OnCharacterDied -= HandleTargetDeath;

        _targetLocation = newTarget;

        if (_targetLocation != null && _targetLocation.TryGetComponent<BattleCharacter>(out _targetCharacter))
        {
            _targetCharacter.OnCharacterDied += HandleTargetDeath;

        }
    }
    



    private void HandleTargetDeath(BattleCharacter deadChar)
    {
        if (_targetLocation == deadChar.transform)
        {
            SetTarget(null);
            Agent.isStopped = false;
            IsInBattle = false; // 타겟이 사망하면 전투 상태 해제
        }
    }
    #endregion





    #region DealDamage
    public void TryAttack()
    {
        if (IsDead || UsingSkill) return; // 캐릭터가 죽었거나 스킬 사용 중이면 공격하지 않음

        _attacktimer += Time.deltaTime;
        if (_attacktimer >= _attackDelay)
        {
            Animator.SetInteger(AnimationHash, UnityEngine.Random.Range(1, 4)); // 공격 애니메이션 출력
            _targetCharacter.TakeDamage(this.AttackDamage); // 타겟의 체력 감소
            _attacktimer = 0f; // 공격 후 타이머 초기화
        }
    }


    public void TakeDamage(float Damage)
    {
        if (IsDead) return; // 이미 죽은 캐릭터는 데미지를 받지 않음
        Health -= Damage; // 공격력만큼 체력 감소
        if (Health == 0)
        {
            Die();
            foreach (var col in GetComponentsInChildren<Collider>())
                col.enabled = false; // ← 탐지 방지
        }
        else
        {
            if (_damageFlashCoroutine != null)
                StopCoroutine(_damageFlashCoroutine);

            _damageFlashCoroutine = StartCoroutine(DamageFlash());
        }
    }



    public virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;

        OnCharacterDied?.Invoke(this);
        Agent.isStopped = true;

        Animator.SetInteger(AnimationHash, 0); // 죽음 애니메이션 출력
    }

    public void SetOff()
    {
        gameObject.SetActive(false); // 또는 Destroy(gameObject);
    }



    private void OnDisable()
    {
        if (_targetLocation != null && _targetLocation.TryGetComponent<BattleCharacter>(out var targetChar))
            targetChar.OnCharacterDied -= HandleTargetDeath;
    }


   
    private IEnumerator DamageFlash()
    {
        foreach (var renderer in _characterRenderer)
        {
            if (renderer != null && renderer.material.HasProperty("_BaseColor"))
                renderer.material.SetColor("_BaseColor", _damageColor);
        }

        yield return new WaitForSeconds(_flashDuration);

        foreach (var renderer in _characterRenderer)
        {
            if (renderer != null && renderer.material.HasProperty("_BaseColor"))
                renderer.material.SetColor("_BaseColor", Color.white);
        }

        _damageFlashCoroutine = null;
    }

    #endregion

    private IEnumerator SkillActive()
    {
        UsingSkill = true;
        yield return new WaitForSeconds(1f); // 스킬 지속 시간 (예: 1초)
        UsingSkill = false; // 스킬 사용 종료
    }

    public override void OnClick()
    {
        throw new NotImplementedException();
    }
}
