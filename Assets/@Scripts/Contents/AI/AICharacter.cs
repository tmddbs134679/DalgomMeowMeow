using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    public AIController AIController { get { return _aiController; }}
    private AIController _aiController;
    
    public CharacterStatSo Stat { get { return _stat;} }
    [SerializeField] private CharacterStatSo _stat;

    public CharacterAction characterAction;
    
    
    private void Awake()
    {
        characterAction = GetComponent<CharacterAction>();
        ControllerRegister();
    }

    private void Update()
    {
        _aiController?.OnUpdate(Time.deltaTime);
    }
    
    public void ControllerRegister()
    {
        _aiController = new AIController(new CharacterIdleState(), this);
    }
}
