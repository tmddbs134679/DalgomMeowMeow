using UnityEngine;
using UnityEngine.AI;

public class CloneActivity : MonoBehaviour
{
    [SerializeField] private Animator _animator; // 애니메이터 컴포넌트
    [SerializeField] private Transform _targetLocation;
    [SerializeField] private NavMeshAgent _agent; // NavMeshAgent 컴포넌트
    [SerializeField] private BattleCharacter _targetCharacter; // 타겟 캐릭터
    [SerializeField] private Transform _parent;
    private bool targetloss = false; // 타겟이 없을 때 부모 위치로 이동하기 위한 플래그
    private PlayerCharacter _parentCharactger;

    [Header("Stats")]
    [SerializeField] private float _maxHP;
    [SerializeField] private float _attack;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange; // 공격 범위
    [SerializeField] private float _detectRangef; // 적 탐지 범위
    [SerializeField] private const float _attackDelay = 1f; // 공격 딜레이 시간

    private float _currentHP; // 최대 체력 (초기화용)
    private float _attacktimer = 1f; // 공격 딜레이 타이머
    private int _animationHash; // 애니메이션 해시

    public float Health
    {
        get => _currentHP;
        set
        {
            if (value <= 0)
            {
                _currentHP = 0;
                this.gameObject.SetActive(false);
            }
            else if (value > _maxHP)
            {
                _currentHP = _maxHP;
            }
            else
            {
                _currentHP = value;
            }
        }
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화
        _animationHash = Animator.StringToHash("animation");
        _agent = GetComponent<NavMeshAgent>(); // NavMeshAgent 컴포넌트 초기화
        _parentCharactger = _parent.GetComponent<PlayerCharacter>();
    }

    void Update()
    {
        if(_parentCharactger.victory)
        {
            this.gameObject.SetActive(false); 
        }

        if (_targetLocation == null)
        {
            var newTarget = FindClosestEnemyInRange(_detectRangef);
            if (newTarget != null)
            {
                SetTarget(newTarget);
                if (_targetLocation != null)
                {
                    _agent.SetDestination(_targetLocation.position);
                    _animator.SetInteger(_animationHash, 5);
                }
            }
            else if (!targetloss) // 타겟이 없고 플래그가 false일 때
            {
                _agent.SetDestination(_parent.position); // 타겟이 없으면 부모 근처로 이동
            }


        }
        else
        {
            float dist = Vector3.Distance(transform.position, _targetLocation.position);

            if (dist <= _attackRange)
            {
                _agent.isStopped = true; //거리가 가까우면 공격

                Vector3 direction = (_targetLocation.position - transform.position).normalized;
                direction.y = 0; // y축 회전 배제

                if (direction != Vector3.zero)
                    transform.rotation = Quaternion.LookRotation(direction);    //공격시 타겟 바라봄

                TryAttack();
            }
            else
            {
                _agent.isStopped = false;
                _agent.SetDestination(_targetLocation.position); // 계속 추적
            }
        }
    }

    public void Init(float maxhp, float atk, float moveSpeed, float range, Transform parent)
    {
        _maxHP = maxhp;
        _attack = atk;
        _moveSpeed = moveSpeed;
        _attackRange = range; // 공격 범위 설정
        _detectRangef = 10f; // 적 탐지 범위 설정

        _agent.stoppingDistance = _attackRange; // NavMeshAgent의 정지 거리 설정
        _currentHP = _maxHP; // 현재 체력 초기화
        _agent.speed = _moveSpeed; // NavMeshAgent 속도 설정
        _parentCharactger = parent.GetComponent<PlayerCharacter>();

        _animationHash = Animator.StringToHash("animation"); 
        Vector3 pos = new Vector3(parent.position.x, parent.position.y, parent.position.z - 2);
        _parent.position = pos; // 부모 위치 설정
    }

    #region FindTarget
    private Transform FindClosestEnemyInRange(float range)      //오버랩 스피어를 통해 적 탐지
    {
        int targetLayerMask = LayerMask.GetMask("Enemy");
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
            _agent.isStopped = false;
        }
    }
    #endregion

    public void TryAttack()
    {

        _attacktimer += Time.deltaTime;
        if (_attacktimer >= _attackDelay)
        {
            _animator.SetInteger(_animationHash, UnityEngine.Random.Range(1, 4)); // 공격 애니메이션 출력
            _attacktimer = 0f; // 공격 후 타이머 초기화
        }
    }

    public void Attack()
    {
        if (_targetCharacter.Invincible == true)
            return;
        _targetCharacter.HpControl(this._attack); // 타겟의 체력 감소
    }

    public void HpControl(float Damage)
    {
        Health -= Damage; // 공격력만큼 체력 감소
    }
}
