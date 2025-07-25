using System;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Contents.AI.FSM.State;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static Define;
using static UnityEditor.MaterialProperty;

public class AICharacter : BaseObject
{
    public AIController Controller { get { return _controller; } }
    private AIController _controller;

    public List<string> EquippedItemIds { get; set; } = new();
    public Character Data { get; set; }

    [Header("AI 캐릭터 현재 상태")]
    public Define.EAIState CurrentState;

    [Header("AI 캐릭터 배정된 건물")]
    public BuildingBase currentBuilding;

    //[Header("캐릭터 스탯")]
    //public float Stamina;
    //public float MaxStamina;
    //public float Atk;
    //public float Hp;
    //public float MoveSpeed;

    [Header("캐릭터 현재 위치")]
    public bool inMain = false; // 캐릭터가 메인 안에 있는지 여부

    public ECropType _ecropType;
    #region Bone
    [SerializeField] private Transform hatBone;
    [SerializeField] private Transform bagBone;
    [SerializeField] private Transform accessoryBone;
    [SerializeField] public Dictionary<EEquipmentType, Transform> equipmentBones = new Dictionary<EEquipmentType, Transform>();

    #endregion

    #region Hide
    [HideInInspector]
    public int CurrentAnimation { get; set; }

    [HideInInspector]
    public bool isClicked = false;
    private Transform head;
    [HideInInspector]
    public Camera _camera;
    private float tempCameraSize = 0;
    private Vector3 tempCameraPos = Vector3.zero;
    [HideInInspector]
    public float tempSpeed;
    [HideInInspector]
    public bool isFollowing;
    [HideInInspector]
    public EAIState loadState;
    [HideInInspector]
    public BuildingBase loadBuilding;

    [HideInInspector]
    public NavMeshAgent nav;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public SkinnedMeshRenderer skinnedMeshRenderer;

    [HideInInspector]
    public Material currentEmo;

    [HideInInspector]
    public CharacterAction characterAction;

    [HideInInspector]
    public bool _isHelloReady = true;

    [HideInInspector]
    public CharacterEmoSet emo;
    [HideInInspector]
    public Sprite[] sprites;

    [HideInInspector]
    public Sprite sprite;

    //UI 상에 보일 캐릭터들
    [HideInInspector]
    public bool IsReplica = false;

    #endregion


    #region Action
    public Action<AICharacter> AnimalLeaved;
    public Action<AICharacter> AnimalArrived;
    public Action<AICharacter> AnimalDelivered;
    public Action<int> CharacterGainExp;
    public Action<float> Levelup;
    #endregion

    private GameObject infoButton;
    private float clickStartTime = 0f;
    private float longPressThreshold = 0.2f;
    private LayerMask groundLayer;

    private void Awake()
    {
        ObjectType = Define.EObjectType.Character;
        InitEquipBones();
    }

    private void Start()
    {
        _camera = Camera.main;
        head = transform.Find("root/pelvis/spine_01/spine_02/spine_03/neck_01");
        infoButton = transform.Find("Canvas").gameObject;

    }

    private void Update()
    {
        if (IsReplica) return;

        if (_controller == null) return;

        if (!nav.enabled)
        {
            nav.enabled = true;
            return;
        }

        Controller.OnUpdate(Time.deltaTime);
        if (BuildingPlacer.Instance == null || !BuildingPlacer.Instance.isAI) LongPressClick();

    }


    private void LateUpdate()
    {
        if (_controller == null) return;
        ClickToSet();
    }



    #region 생성 시 초기화 및 불러오기
    public override bool Init()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        characterAction = GetComponent<CharacterAction>();
        groundLayer = LayerMask.GetMask("Ground");
        currentEmo = skinnedMeshRenderer.materials[1];
        return true;
    }

    public void SetInfo(Character ch)
    {
        Data = ch;
        // 위치값
        transform.position = Data.Pos.ToVector3();
        CurrentState = Data.CurrentState;
        loadState = Data.CurrentState;
        loadBuilding = Data.LoadBuilding;


        // TODO : FSM 등 상태 적용
        ControllerRegister();
    }
    #endregion

    #region FSM Register
    public void ControllerRegister()
    {
        _controller = new AIController(new CharacterResetState(), this, Define.EAIState.None);
        _controller.RegisterState(new CharacterIdleState(), this, Define.EAIState.Idle);
        _controller.RegisterState(new CharacterBuildingState(), this, Define.EAIState.Build);
        _controller.RegisterState(new CharacterCookState(), this, Define.EAIState.Cook); ;
        _controller.RegisterState(new CharacterFarmingState(), this, Define.EAIState.Farm);
        _controller.RegisterState(new CharacterPlayState(), this, Define.EAIState.Play);
        _controller.RegisterState(new CharacterRestState(), this, Define.EAIState.Rest);
        _controller.RegisterState(new CharacterMoveToState(), this, Define.EAIState.MoveTo);
        _controller.RegisterState(new CharacterDeliverState(), this, Define.EAIState.Deliver);
        _controller.RegisterState(new CharacterHelloState(), this, Define.EAIState.Hello);
        _controller.RegisterState(new CharacterFishingState(), this, Define.EAIState.Fishing);
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

    public ECropType DistinguishCrops(ECropType type)
    {
        _ecropType = type;
        return _ecropType;
    }
    #endregion

    #region 캐릭터 스탯 관련
    public void OnLevelUp()
    {
        Data.MoveSpeed += 0.5f; // 레벨업 시 이동 속도 증가
        Data.MoveSpeed = MathF.Min(6, Data.MoveSpeed);
        Data.MaxExp *= 1.3f; // 레벨업 시 최대 경험치 증가
        Data.Level++;
        Managers.Debug.Log($"레벨업! 현재 레벨: {Data.Level}", Define.EDebugType.AI);
        Levelup?.Invoke(Data.Level);
    }

    public void GainExp(int value)
    {
        Data.CurrentExp += value;
        while (Data.CurrentExp >= Data.MaxExp)
        {
            Data.CurrentExp -= Data.MaxExp;
            OnLevelUp();
        }
        Managers.Debug.Log($"경험치 획득: {value}, 현재 경험치: {Data.CurrentExp}, 최대 경험치: {Data.MaxExp}", Define.EDebugType.AI);
        CharacterGainExp?.Invoke(value);
    }

    public void UseStamina(float amount)
    {
        if (Data.CurrentStamina - amount < 0)
        {
            return;
        }
        Data.CurrentStamina = Mathf.Max(0, Data.CurrentStamina - amount);
        //Managers.Debug.Log($"스태미나 사용 : {amount}, 남은 스태미나: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
    }

    public void RecoverStamina(float amount)
    {
        Data.CurrentStamina = Mathf.Min(100, Data.CurrentStamina + amount);
        //Managers.Debug.Log($"스태미나 회복 : {amount}, 현재: {CharacterData.CurrentStamina}", Define.EDebugType.AI);
    }
    #endregion

    #region 캐릭터 제거 시
    public void OnDestroy()
    {
        Controller?.Dispose();
        Managers.AI.Unregister(this);
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
        if (emo == null) return;
        currentEmo = emo.EmotionMaterials[index];
        var mats = skinnedMeshRenderer.materials;
        mats[1] = emo.EmotionMaterials[index];
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!isClicked)
        {
            System.Random random = new System.Random();
            string randomCatSound = Define.CAT_SOUNDS[random.Next(Define.CAT_SOUNDS.Length)];
            Managers.Sound.Play(Define.ESound.Effect, randomCatSound);
        }
           
                if (gameObject == this.gameObject &&
                    Controller.CurrentState() is not CharacterHelloState)
                {
                      if (!isClicked)
            {
                tempCameraPos = _camera.transform.position;
                tempCameraSize = _camera.orthographicSize;
            }
            if (isClicked)
            {
                _camera.orthographicSize = tempCameraSize;
                _camera.transform.position = tempCameraPos;
            }
                    clickStartTime = 0;
                    isClicked = !isClicked;

                }

    }
    private void LongPressClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
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
                        nav.enabled = false;
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
                //SetSpeed(0);
                this.transform.position = new Vector3(mouspot.x, hit.point.y + 2f, mouspot.z);

            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (clickStartTime <= 0.2f && hit.collider.gameObject == this.gameObject && isClicked)
                {
                    // 부드럽게 줌인
                    _camera.DOOrthoSize(2f, 1f); // 1초 동안 줌인

                    // 부드럽게 위치 이동
                    Vector3 targetPos = new Vector3(transform.position.x - 20.3f, 30.5f, transform.position.z - 20.6f);
                    _camera.transform.DOMove(targetPos, 1f); // 1초 동안 이동
                }
            }

            if (isFollowing)
            {
                nav.enabled = true;
                this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Managers.AI.ValidateNavMeshPosition(this);
            }
            //_camera.orthographicSize = 2f;
            //_camera.transform.position = new Vector3(transform.position.x - 20.3f, 30.5f, transform.position.z - 20.6f);
            isFollowing = false;
            SetAnimation(CurrentAnimation);
            clickStartTime = 0f;
        }
    }
    private void ClickToSet()
    {
        if (isClicked)
        {
            SetSpeed(0);
            this.gameObject.transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y + 180, 0);
            head.transform.localRotation = quaternion.Euler(0, 0, -12);
            infoButton.SetActive(true);
            animator.SetInteger("animation", 36);
        }

        else if (!isClicked && !isFollowing)
        {
            if (Controller.CurrentState() is CharacterIdleState)
                SetSpeed(Data.WalkSpeed);
            else if (Controller.CurrentState() is CharacterDeliverState)
                SetSpeed(Data.MoveSpeed / 2);
            else
                SetSpeed(Data.MoveSpeed);

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


    public void ReplicaSetting(Character character)
    {
        // Managers.Game.EquipCharacterVisual(this, character);
        Data = character;

        Managers.Equipment.ApplyEquipmentPreview(this, character);
    }

    #endregion
}


