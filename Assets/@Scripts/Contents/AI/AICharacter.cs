using System;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static Define;

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

    #region Bone
    [SerializeField] private Transform hatBone;
    [SerializeField] private Transform bagBone;
    [SerializeField] private Transform accessoryBone;
    [SerializeField] public Dictionary<EEquipmentType, Transform> equipmentBones = new Dictionary<EEquipmentType, Transform>();

    #endregion
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
    private float longPressThreshold = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isFollowing;

    //UI 상에 보일 캐릭터들
    public bool IsReplica = false;

    private void Awake()
    {
        ObjectType = Define.EObjectType.Character;
        InitEquipBones();
    }

    private void Start()
    {
        Init();
        camera = Camera.main;
        head = transform.Find("root/pelvis/spine_01/spine_02/spine_03/neck_01");
    }

    private void Update()
    {
        if (IsReplica)
            return;

        if (CharacterData == null)
        {
            Managers.Debug.Log("캐릭터 데이터 없음", Define.EDebugType.AI);
            return;
        }
        _controller?.OnUpdate(Time.deltaTime);
        if (BuildingPlacer.Instance.isAI) OnClick();

        LongPressClick();
    }


    private void LateUpdate()
    {
        if (CharacterData == null)
        {
            Managers.Debug.Log("캐릭터 데이터 없음", Define.EDebugType.AI);
            return;
        }
        _controller?.OnLateUpdate(Time.deltaTime);
        Clicked();
    }

    public override bool Init()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        characterAction = GetComponent<CharacterAction>();

        groundLayer = LayerMask.GetMask("Ground");

        currentEmo = skinnedMeshRenderer.materials[1];
        emo = AIManager.Instance.EmotionMaterials;
        if (_controller == null) { ControllerRegister(); }
        Controller.Setup();
        AIManager.Instance.Register(this);
   

        return true;
    }

    public void SetInfo(Character ch)
    {
        CharacterData = ch;
        // 위치값
        transform.position = ch.Pos.ToVector3();
        CurrentState = ch.CurrentState;
        // TODO : FSM 등 상태 적용

        if (_controller == null)
        {
            ControllerRegister();
        }
    }

    //public void Setinfo(Character ch, )

    #region FSM Register
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
    #endregion

    #region 건물 상호작용
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
    #endregion

    #region 캐릭터 스탯 관련
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
    #endregion

    #region 캐릭터 제거 시
    public void OnDestroy()
    {
        Controller?.Dispose();
        AIManager.Instance.Unregister(this);
    }
    #endregion

    #region 애니메이션 / 속도 설정
    public void SetAnimation(int animNum)
    {
        animator.SetInteger("animation", animNum);
        CurrentAnimation = animNum;
    }

    public void SetEmotion(int index)
    {
        if (emo == null || emo.Length <= index) return;

        currentEmo = emo[index];
        var mats = skinnedMeshRenderer.materials;
        mats[1] = emo[index];
        skinnedMeshRenderer.materials = mats;
    }

    public void SetSpeed(float speed)
    {
        nav.speed = speed;
    }


    #endregion

    #region 클릭 상호작용
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
                    clickStartTime = 0;
                    isClicked = !isClicked;

                }
            }
        }
    }

    private void LongPressClick()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject &&
                    (
                    Controller.CurrentState() is CharacterIdleState ||
                    Controller.CurrentState() is CharacterMoveToState ||
                    Controller.CurrentState() is CharacterDeliverState)
                    )

                {
                    clickStartTime += Time.deltaTime;
                    if (clickStartTime > longPressThreshold)
                    {
                        isClicked = false;
                        isFollowing = true;
                    }

                }
            }
        }

        if (isFollowing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundLayer))
            {
                infoButton.SetActive(false);
                Vector3 mouspot = hit.point;
                animator.SetInteger("animation", 49);
                SetSpeed(0);
                this.transform.position = new Vector3(mouspot.x, 2f, mouspot.z);
            }
        }
        if (Input.GetMouseButtonUp(0)  && isFollowing)
        {
            if (isFollowing)
                this.transform.position = new Vector3(transform.position.x, 0.616f, transform.position.z);
            isFollowing = false;
            if (Controller.CurrentState() is CharacterIdleState)
                SetSpeed(CharacterData.WalkSpeed);
            else if (Controller.CurrentState() is CharacterDeliverState)
                SetSpeed(CharacterData.MoveSpeed / 2);
            else
                SetSpeed(CharacterData.MoveSpeed);
                SetAnimation(CurrentAnimation);
            clickStartTime = 0f;
        }
    }
    private void Clicked()
    {
        if (isClicked)
        {
            SetSpeed(0);
            this.gameObject.transform.rotation = Quaternion.Euler(0, camera.transform.eulerAngles.y + 180, 0);
            head.transform.localRotation = quaternion.Euler(0, 0, -12);
            infoButton.SetActive(true);
            animator.SetInteger("animation", 36);
        }

        else if (!isClicked && !isFollowing)
        {
            if (Controller.CurrentState() is CharacterIdleState)
                SetSpeed(CharacterData.WalkSpeed);
            else if (Controller.CurrentState() is CharacterDeliverState)
                SetSpeed(CharacterData.MoveSpeed / 2);
            else
                SetSpeed(CharacterData.MoveSpeed);

            SetAnimation(CurrentAnimation);
            infoButton.SetActive(false);
        }
    }
    #endregion


    #region 장비설정

    private void InitEquipBones()
    {
        equipmentBones[EEquipmentType.Hat] = hatBone;
        equipmentBones[EEquipmentType.Bag] = bagBone;
        equipmentBones[EEquipmentType.Accessory] = accessoryBone;

    }


    #endregion
}


