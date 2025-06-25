using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    public AIController Controller { get { return _controller; } }
    private AIController _controller;

    public CharacterStatSo Stat { get { return runtimeStat; } }
    private CharacterStatSo runtimeStat;

    [SerializeField] private CharacterStatSo originStat;

    [HideInInspector]
    public NavMeshAgent nav;
    public BuildingBase currentBuilding;
    private Collider _collider;

    [HideInInspector]
    public CharacterAction characterAction;

    public event Action<AICharacter> AnimalLeaved;
    public event Action<AICharacter> AnimalArrived;

    private bool _isHelloReady = true;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        _controller?.OnUpdate(Time.deltaTime);
    }

    private void Init()
    {
        nav = GetComponent<NavMeshAgent>();
        characterAction = GetComponent<CharacterAction>();
        _collider = GetComponentInChildren<Collider>();

        if (runtimeStat != null)
            runtimeStat.OnStatChanged -= ApplyStat;
        else
            runtimeStat = originStat.Clone();

        runtimeStat.OnStatChanged += ApplyStat;

        ApplyStat();

        ControllerRegister();
    }


    public void ControllerRegister()
    {
        _controller = new AIController(new CharacterIdleState(), this);
        _controller.RegisterState(new CharacterBuildingState(), this);
        _controller.RegisterState(new CharacterCookState(), this); ;
        _controller.RegisterState(new CharacterFarmingState(), this);
        _controller.RegisterState(new CharacterPlayState(), this);
        _controller.RegisterState(new CharacterRestState(), this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AICharacter>(out var others))
        {
            if (this._isHelloReady && others._isHelloReady && other.GetInstanceID() > this.GetInstanceID())
            {
                //애니메이션 적용
                Debug.Log("Hello Motion Triggered with " + others.name);
                StartCoroutine(HelloMotion());
                _isHelloReady = false;
                others._isHelloReady = false;
                StartCoroutine(HelloMotionReset(others));
            }
        }
    }

    private IEnumerator HelloMotionReset(AICharacter other)
    {
        yield return new WaitForSeconds(10f);
        _isHelloReady = true;
        other._isHelloReady = true;
    }

    private IEnumerator HelloMotion()
    {
        float temp = runtimeStat.MoveSpeed;
        runtimeStat.MoveSpeed = 0f;
        yield return new WaitForSeconds(3f);
        runtimeStat.MoveSpeed = temp;
    }

    public void OnAnimalArrived()
    {
        AnimalArrived?.Invoke(this);
    }

    public void OnAnimalLeaved()
    {
        AnimalLeaved?.Invoke(this);
    }

    private void ApplyStat()
    {
        nav.speed = runtimeStat.MoveSpeed;
    }

    public void UseStamina(float amount)
    {
        if (runtimeStat.Stamina - amount < 0)
        {
            Controller.ChangeState(nameof(CharacterIdleState));
        }
        runtimeStat.Stamina = Mathf.Max(0, runtimeStat.Stamina - amount);
        Debug.Log($"Stamina used: {amount}, Remaining: {runtimeStat.Stamina}");
    }


    public void GotoRest()
    {
        if (runtimeStat.Stamina < 5)
        {
            Controller.ChangeState(nameof(CharacterRestState));
        }
    }

    public void OnLevelUp()
    {
        runtimeStat.MoveSpeed += 1;
        runtimeStat.Hp += 10;
    }

    public void OnDisable()
    {
        Controller?.Dispose();
    }


}
