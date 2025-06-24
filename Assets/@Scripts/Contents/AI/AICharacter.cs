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

    [HideInInspector]
    public CharacterAction characterAction;
    
    
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        characterAction = GetComponent<CharacterAction>();
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
}
