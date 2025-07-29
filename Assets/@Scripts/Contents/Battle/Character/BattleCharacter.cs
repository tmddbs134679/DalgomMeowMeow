using Data;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BattleCharacter : BaseObject
{

    [SerializeField] private SkillLibrary _skillLibrary; // 스킬 라이브러리
    [SerializeField] private ParticleSystem _heal;
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _detectRange = 10f;
    [SerializeField] private float _flashDuration = 0.05f;
    [SerializeField] protected SkinnedMeshRenderer[] _characterRenderer; // 캐릭터 렌더러
    [SerializeField] protected Vector3 _originalPosition; // 원래 위치 저장

    public EffectManager _effectManager; // 이펙트 매니저
    public CloneActivity _clone; // 클론 액티비티 (적용 여부는 캐릭터에 따라 다름)
    public HealthBarUI _hpBar;
    public GameObject damageTextPrefab; // 피격 시 데미지 텍스트 프리팹

    public NavMeshAgent Agent { get; private set; } // NavMeshAgent 컴포넌트
    public Animator Animator; // 애니메이터 컴포넌트

    public Transform TargetLocation;
    public Transform LeftHandPivot;
    public Transform RightHandPivot;
    public Transform HeadPivot;
    public Transform CharacterObject;


    #region Stats
    public string CharID;
    public float AttackDamage; // 공격력
    public string SkillID;
    public float MaxHP;
    public float MoveSpeed; // 이동 속도


    public float AttackDelay = 1f; // 공격 딜레이 (초 단위)
    public float AttackRange = 1.5f;
    public bool Invincible = false; // 무적 상태 여부
    
    public int Skillnum;
    public float SkillCooldown; // 스킬 쿨타임 (초 단위)
    #endregion

    public int AnimationHash;
    public int SkillTrigger;
    public int SkillHash;

    public bool HasLookedForward = true; // 전방을 바라봤는지 여부 (아군 전용 로직)
    public bool Stunned = false; // 스턴 상태 여부
    public bool UsingSkill = false; // 스킬 사용 여부
    public bool IsDead { get; private set; } = false;
    public bool IsInBattle
    {
        get => _isInBattle;
        set
        {
            if (_isInBattle != value)
            {
                _isInBattle = value;
                OnBattleStateChanged?.Invoke(this, _isInBattle);
            }
        }
    }
    public string TargetLayer = "Enemy"; // 타겟 태그
    
    private BattleCharacter _targetCharacter; // 현재 타겟 캐릭터
    private Coroutine _damageFlashCoroutine; //피격 효과 코루틴
    
    private float _attacktimer = 0f;
    private float _currentHP; // 최대 체력 (초기화용)
    public float Health
    {
        get => _currentHP;
        set
        {
            if (value <= 0)
            {
                _currentHP = 0;
                Die();
            }
            else if (value > MaxHP)
            {
                _currentHP = MaxHP;
            }
            else
            {
                _currentHP = value;
            }
        }
    }

    private bool _isInBattle = false;

    public event Action<BattleCharacter> OnCharacterDied;
    public event Action<BattleCharacter, bool> OnBattleStateChanged;

    
    protected virtual void Awake()
    {
        _skillLibrary = GetComponentInParent<SkillLibrary>();
        ObjectType = Define.EObjectType.Enemy; // 객체 타입 설정
        Agent = GetComponent<NavMeshAgent>();
        _effectManager = GetComponentInChildren<EffectManager>();
    }

    protected virtual void Start()
    {
        Health = MaxHP;
        _originalPosition = transform.localPosition;
        Agent.speed = MoveSpeed; // NavMeshAgent의 이동 속도 설정
        Agent.stoppingDistance = AttackRange; // 공격 범위 내에서 멈추도록 설정
    }

    void Update()
    {
        if (IsDead || Stunned) return; // 캐릭터가 죽었으면 업데이트 중지

        if (TargetLocation == null)
        {
            var newTarget = FindClosestEnemyInRange(_detectRange);
            if (newTarget != null)
            {
                SetTarget(newTarget);
                if (TargetLocation != null)
                {
                    Agent.SetDestination(TargetLocation.position);
                    IsInBattle = true; // 타겟이 생기면 전투 상태로 변경
                    Animator.SetInteger(AnimationHash, 5);
                }
            }
        }
        else
        {
            float dist = Vector3.Distance(transform.position, TargetLocation.position);

            if (dist <= AttackRange)
            {
                Agent.isStopped = true; //거리가 가까우면 공격

                Vector3 direction = (TargetLocation.position - transform.position).normalized;
                direction.y = 0; // y축 회전 배제

                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direction);    //공격시 타겟 바라봄

                TryAttack();
            }
            else
            {
                Agent.isStopped = false;
                Agent.SetDestination(TargetLocation.position); // 계속 추적
            }
        }
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
        if (TargetLocation != null && TargetLocation.TryGetComponent<BattleCharacter>(out var oldChar))
            oldChar.OnCharacterDied -= HandleTargetDeath;

        TargetLocation = newTarget;

        if (TargetLocation != null && TargetLocation.TryGetComponent<BattleCharacter>(out _targetCharacter))
        {
            _targetCharacter.OnCharacterDied += HandleTargetDeath;
        }

        if (TargetLocation != null && TargetLocation.TryGetComponent<CloneActivity>(out _clone))
        {
            return;
        }
    }


    private void HandleTargetDeath(BattleCharacter deadChar)
    {
        if (TargetLocation == deadChar.transform)
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
        if (_attacktimer >= AttackDelay)
        {
            Animator.SetInteger(AnimationHash, UnityEngine.Random.Range(1, 4)); // 공격 애니메이션 출력
            _attacktimer = 0f; // 공격 후 타이머 초기화
        }
    }

    public void Attack()
    {
        if(_targetCharacter.Invincible == true)
            return;
        _targetCharacter.HpControl(this.AttackDamage); // 타겟의 체력 감소
    }


    public void HpControl(float Damage)
    {
        if (IsDead) // 이미 죽었거나 무적 상태이면 아무런 행동도 하지 않음
            return; 

        Health -= Damage; // 공격력만큼 체력 감소
        if (Damage >= 0)
        {
            int rand = UnityEngine.Random.Range(1, 5);
            Managers.Sound.Play(Define.ESound.Effect, $"Hit{rand}"); // 피격 사운드 재생
            if (_damageFlashCoroutine != null)
                StopCoroutine(_damageFlashCoroutine);
            _damageFlashCoroutine = StartCoroutine(DamageFlash());
        }
        else if(Damage < 0)
        {
            Effect(new Color(0f, 1f, 105/255f, 200/255f)); // 힐 파티클 효과(연한 초록색)
        }
        GameObject go = Instantiate(damageTextPrefab);
        go.GetComponent<DamageUI>().Show(Damage, this.transform.position + Vector3.up * 2 , this.gameObject.layer);
    }

    public void Effect(Color color)
    {
        var particle = _heal.main; // 힐 파티클 색상 변경
        particle.startColor = color;
        _heal.Play(); // 힐 파티클 재생
    }



    public virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false; // ← 탐지 방지

        OnCharacterDied?.Invoke(this);
        Agent.isStopped = true;

        Animator.SetInteger(AnimationHash, 0); // 죽음 애니메이션 출력
    }

    public void BaseDie()
    {
        if (IsDead) return;
        IsDead = true;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = false; // ← 탐지 방지

        OnCharacterDied?.Invoke(this);
        Agent.isStopped = true;
    }

    public void SetOff()
    {
        gameObject.SetActive(false); // 또는 Destroy(gameObject);
    }



    private void OnDisable()
    {
        if (TargetLocation != null && TargetLocation.TryGetComponent<BattleCharacter>(out var targetChar))
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


    public void ActiveSkill()
    {
        if (IsDead)
            return;
        _skillLibrary.UseSkill(this.Skillnum, this); // 스킬 라이브러리에서 스킬 사용
    }

    public void SetAnimation()
    {
        Animator = GetComponentInChildren<Animator>();
        AnimationHash = Animator.StringToHash("animation"); // 애니메이션 해시 초기화
        SkillHash = Animator.StringToHash("Skills"); // 스킬 애니메이션 해시 초기화
        SkillTrigger = Animator.StringToHash("Skill"); // 스킬 애니메이션 이름 해시 초기화
    }

    public void PivotSet()
    {
        LeftHandPivot = CharacterObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_l/upperarm_l/lowerarm_l/hand_l/Hand_l_equipment");
        RightHandPivot = CharacterObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Hand_r_equipment");
        HeadPivot = CharacterObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/neck_01/head/Head_equipment");
    }



    public override void OnClick()
    {
        throw new NotImplementedException();
    }
}
