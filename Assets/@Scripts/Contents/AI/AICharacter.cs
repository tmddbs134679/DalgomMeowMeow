using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using JetBrains.Annotations;
using Scripts.Contents.AI.FSM.State;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEditor.UI;
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
    [HideInInspector]
    public bool _isHelloReady = true;

    [Header("AI 캐릭터 현재 상태")]
    public Define.EAIState CurrentState;
    [Header("캐릭터 이미지들")]
    public Material[] emo;
    public Sprite[] sprites;
    [HideInInspector]
    public Sprite sprite;

    public Character CharacterData { get; set; }

    [HideInInspector]
    public int CurrentAnimation { get; set; }

    [HideInInspector]
    public bool isClicked = false;
    private Transform head;
    [HideInInspector]
    public Camera camera;
    [HideInInspector]
    public float tempSpeed;
    [SerializeField]
    private GameObject infoButton;

    public float Exp;
    public event Action<AICharacter> AnimalLeaved;
    public event Action<AICharacter> AnimalArrived;
    public event Action<AICharacter> AnimalDelivered;
    public Action<int> CharacterGainExp;
    public Action<int> Levelup;
    private float clickStartTime = 0f;
    private float longPressThreshold = 0.5f;
    private bool longPressHandled = false;
    [SerializeField]
    private LayerMask CharacterLayerMask;
    private bool isFollowing;

    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);


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
        Exp = CharacterData.CurrentExp;
        if (BuildingPlacer.Instance.isAI) OnClick();

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject &&
                    Controller.CurrentState() is not CharacterHelloState)
                {
                    clickStartTime += Time.deltaTime;
                    if (clickStartTime > longPressThreshold)
                    {
                        isFollowing = true;
                        animator.SetInteger("animation", 49);
                        isClicked = false;
                        nav.speed = 0;
                        if (isFollowing)
                        {

                        }

                        Vector3 hitPoint = hit.point;
                        Vector3 hitpoint = new Vector3(hitPoint.x, 2, hitPoint.z);
                        this.transform.position = hitpoint;
                    }

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            SetAnimation(CurrentAnimation);
            if(tempSpeed > 0)
                nav.speed = tempSpeed;
            this.transform.position = new Vector3(transform.position.x, 0.616f, transform.position.z);
            clickStartTime = 0f;
        }
    }

    private void LateUpdate()
    {
        _controller?.OnLateUpdate(Time.deltaTime);
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
        CurrentState = ch.CurrentState;
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
        //Managers.Debug.Log($"스태미나 사용 : {amount}, 남은 스태미나: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
    }

    public void RecoverStamina(float amount)
    {
        CharacterData.CurrentStamina = Mathf.Min(100, CharacterData.CurrentStamina + amount);
        //Managers.Debug.Log($"스태미나 회복 : {amount}, 현재: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
    }

    public void OnLevelUp()
    {
        CharacterData.MoveSpeed += 1;
        CharacterData.Hp += 10;
        CharacterData.MaxExp += 5; // 레벨업 시 최대 경험치 증가
        CharacterData.Level++;
        Managers.Debug.Log($"레벨업! 현재 레벨: {CharacterData.Level}", Define.EDebugType.AI);
        Levelup?.Invoke(CharacterData.Level);
    }

    public void GainExp(int value)
    {
        CharacterData.CurrentExp += value;
        while (CharacterData.CurrentExp >= CharacterData.MaxExp)
        {
            CharacterData.CurrentExp -= CharacterData.MaxExp;
            OnLevelUp();
        }
        Managers.Debug.Log($"경험치 획득: {value}, 현재 경험치: {CharacterData.CurrentExp}, 최대 경험치: {CharacterData.MaxExp}", Define.EDebugType.AI);
        CharacterGainExp?.Invoke(value);
    }

    public void OnDisable()
    {
        Controller?.Dispose();
        AIManager.Instance.Unregister(this);
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
                if (hit.collider.gameObject == this.gameObject &&
                    Controller.CurrentState() is not CharacterHelloState)
                {
                    isClicked = !isClicked;
                    if (isClicked)
                    {
                        tempSpeed = nav.speed;
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
            infoButton.SetActive(true);
            animator.SetInteger("animation", 36);
        }
        
        else if (!isClicked && tempSpeed >0)
        {
            SetAnimation(CurrentAnimation);
            infoButton.SetActive(false);
            nav.speed = tempSpeed;
        }
    }

}



