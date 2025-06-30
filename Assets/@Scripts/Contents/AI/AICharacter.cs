using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scripts.Contents.AI.FSM.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class AICharacter : BaseObject
{
    public AIController Controller { get { return _controller; } }
    private AIController _controller;

    [HideInInspector]
    public NavMeshAgent nav;

    public BuildingBase currentBuilding;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Material currentEmo;
    public Material[] emo;

    [HideInInspector]
    public CharacterAction characterAction;

    public event Action<AICharacter> AnimalLeaved;
    public event Action<AICharacter> AnimalArrived;

    public bool _isHelloReady = true;

    public Sprite[] sprites;
    public Sprite sprite;
    public Character CharacterData { get;  set; }

    private void Awake()
    {
        ObjectType = Define.EObjectType.Character;
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        _controller?.OnUpdate(Time.deltaTime);
    }

    public override bool Init()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        characterAction = GetComponent<CharacterAction>();

        currentEmo = skinnedMeshRenderer.materials[1];
        emo = AIManager.Instance.EmotionMaterials;

        AIManager.Instance.Register(this);
        ControllerRegister();

        return true;
    }

    public void SetInfo(Character ch)
    {
        CharacterData = ch;
        // 위치값
        transform.position = ch.Pos.ToVector3();

        // TODO : FSM 등 상태 적용
    }

    public void ControllerRegister()
    {
        _controller = new AIController(new CharacterResetState(), this, Define.EAIState.None);
        _controller.RegisterState(new CharacterIdleState(), this, Define.EAIState.Idle);
        _controller.RegisterState(new CharacterBuildingState(), this, Define.EAIState.Building);
        _controller.RegisterState(new CharacterCookState(), this, Define.EAIState.Cooking); ;
        _controller.RegisterState(new CharacterFarmingState(), this, Define.EAIState.Farming);
        _controller.RegisterState(new CharacterPlayState(), this, Define.EAIState.Playing);
        _controller.RegisterState(new CharacterRestState(), this, Define.EAIState.Resting);
        _controller.RegisterState(new CharacterMoveToState(), this, Define.EAIState.MoveTo);
        _controller.RegisterState(new CharacterDeliverState(), this, Define.EAIState.Delivery);
        _controller.RegisterState(new CharacterHelloState(), this, Define.EAIState.Hello);
    }

    public void OnAnimalArrived()
    {
        AnimalArrived?.Invoke(this);
    }

    public void OnAnimalLeaved()
    {
        AnimalLeaved?.Invoke(this);
    }

    public void UseStamina(float amount)
    {
        if (CharacterData.CurrentStamina - amount < 0)
        {
            return;
        }
        CharacterData.CurrentStamina = Mathf.Max(0, CharacterData.CurrentStamina - amount);
        Debug.Log($"스태미나 사용 : {amount}, 남은 스태미나: {CharacterData.CurrentStamina}");
    }

    public void RecoverStamina(float amount)
    {
        CharacterData.CurrentStamina = Mathf.Min(100, CharacterData.CurrentStamina + amount);
        Debug.Log($"스태미나 회복 : {amount}, 현재: {CharacterData.CurrentStamina}");
    }

    public void OnLevelUp()
    {
        CharacterData.MoveSpeed += 1;
        CharacterData.Hp += 10;
    }

    public void OnDisable()
    {
        AIManager.Instance.Unregister(this);
        Controller?.Dispose();
    }

    public void SetEmotion(int index)
    {
        if (emo == null || emo.Length <= index) return;

        currentEmo = emo[index];
        var mats = skinnedMeshRenderer.materials;
        mats[1] = emo[index];
        skinnedMeshRenderer.materials = mats;
    }

    public override void OnClick()
    {

    }

}



