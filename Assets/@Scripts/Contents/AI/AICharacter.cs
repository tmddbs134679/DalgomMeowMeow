using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using JetBrains.Annotations;
using Scripts.Contents.AI.FSM.State;
using Unity.Mathematics;
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

    [HideInInspector]
    public Material currentEmo;

    [HideInInspector]
    public CharacterAction characterAction;

    public List<string> EquippedItemIds { get; set; } = new();

    public event Action<AICharacter> AnimalLeaved;
    public event Action<AICharacter> AnimalArrived;
    public event Action<AICharacter> AnimalDelivered;
    public Define.EAIState CurrentState;
    [HideInInspector]
    public bool _isHelloReady = true;

    [HideInInspector]
    public bool isClicked = false;
    public Material[] emo;
    public Sprite[] sprites;
    [HideInInspector]
    public Sprite sprite;
    public Character CharacterData { get; set; }
    [HideInInspector]
    public int CurrentAnimation { get; set; }

    public Transform head;
    public Camera camera;
    public float tempSpeed;

    private void Awake()
    {
        ObjectType = Define.EObjectType.Character;
    }

    private void Start()
    {
        Init();
        camera = Camera.main;
        head = transform.Find("root/pelvis/spine_01/spine_02/spine_03/neck_01");
    }

    private void Update()
    {
        _controller?.OnUpdate(Time.deltaTime);
    }

    private void LateUpdate()
    {
        _controller?.OnLateUpdate(Time.deltaTime);
        OnClick();
        Clicked();
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

    public void SetAnimation(int animNum)
    {
        animator.SetInteger("animation", animNum);
        CurrentAnimation = animNum;
    }

    public void OnAnimalArrived()
    {
        AnimalArrived?.Invoke(this);
    }

    public void OnAnimalLeaved()
    {
        AnimalLeaved?.Invoke(this);
    }

    public void OnAnimalDelivered()
    {
        AnimalDelivered?.Invoke(this);
    }

    public void UseStamina(float amount)
    {
        if (CharacterData.CurrentStamina - amount < 0)
        {
            return;
        }
        CharacterData.CurrentStamina = Mathf.Max(0, CharacterData.CurrentStamina - amount);
        Managers.Debug.Log($"스태미나 사용 : {amount}, 남은 스태미나: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
    }

    public void RecoverStamina(float amount)
    {
        CharacterData.CurrentStamina = Mathf.Min(100, CharacterData.CurrentStamina + amount);
        Managers.Debug.Log($"스태미나 회복 : {amount}, 현재: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 내 캐릭터에만 반응하도록
                if (hit.collider.gameObject == this.gameObject )
                {
                    isClicked = !isClicked;
                    if (isClicked)
                    {
                        tempSpeed = nav.speed;
                    }
                   else
                    {
                        SetAnimation(CurrentAnimation);
                        nav.speed = tempSpeed;
                    }
                }
            }
        }
    }
    private void Clicked()
    {
        if (isClicked)
        {
            nav.speed = 0;
            this.gameObject.transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y + 180, 0);
            head.transform.localRotation = quaternion.Euler(0, 0, -12);
            animator.SetInteger("animation", 36);
        }
    }



        //public void OnDragStart(Vector3 hitPos)
        //{

        //}

        //public void OnDrag(Vector3 hitPos)
        //{
        //}

        //public void OnDragEnd()
        //{
        //}

        //public void OnLongPress()
        //{
        //}
}



