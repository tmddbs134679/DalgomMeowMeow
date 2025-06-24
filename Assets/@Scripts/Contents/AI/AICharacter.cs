using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
using UnityEngine;
using UnityEngine.AI;

public class AICharacter : MonoBehaviour
{
    public AIController Controller { get { return _controller; }}
    private AIController _controller;
    
    public CharacterStatSo Stat { get { return _stat;} }
    [SerializeField] private CharacterStatSo _stat;

    public NavMeshAgent nav;
    public BuildingBase currentBuilding;
    private Collider _collider;

    [HideInInspector]
    public CharacterAction characterAction;

    public event Action AnimalLeaved;
    public event Action AnimalArrived;

    private bool _isHelloReady = true;
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        characterAction = GetComponent<CharacterAction>();
        _collider = GetComponentInChildren<Collider>();
        ControllerRegister();
    }

    private void Update()
    {
        _controller?.OnUpdate(Time.deltaTime);
    }
    
    public void ControllerRegister()
    {
        _controller = new AIController(new CharacterIdleState(), this);
        _controller.RegisterState(new CharacterBuildingState(), this);
        _controller.RegisterState(new CharacterCookState(), this);;
        _controller.RegisterState(new CharacterFarmingState(), this);
        _controller.RegisterState(new CharacterPlayState(), this);
        _controller.RegisterState(new CharacterRestState(), this);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<AICharacter>(out var others))
        {
            if(this._isHelloReady && others._isHelloReady && other.GetInstanceID() > this.GetInstanceID())
            {
                //애니메이션 적용
                Debug.Log("Hello Motion Triggered with " + others.name);
                _isHelloReady = false;
                others._isHelloReady = false;
                StartCoroutine(HelloMotion(others));
            }
        }
    }

    private IEnumerator HelloMotion(AICharacter other)
    {
        yield return new WaitForSeconds(10f);
        _isHelloReady = true;
        other._isHelloReady = true;
    }
    
    private IEnumerator HelloMotion()
    {
        float temp = _stat.MoveSpeed;
        _stat.MoveSpeed = 0f;
        yield return new WaitForSeconds(3f);
        _stat.MoveSpeed = temp;
    }

    public void OnAnimalArrived()
    {
        animalArrived?.Invoke();
    }

    
}
