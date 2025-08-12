using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Scripts.Contents.AI.FSM.State;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static Define;

public class AICharacter : BaseObject
{
    public AIController Controller { get { return _controller; } }
    private AIController _controller;

    public  AICharacterStat Stat { get { return _stat; } }
    private AICharacterStat _stat;

    public AICharacterLevelEffectHandler Effect { get { return _effect; } }
    private AICharacterLevelEffectHandler _effect;

    public AICharacterView View  { get { return _view; } }
    private AICharacterView _view;

    public AICharacterInteraction Interaction { get { return _interaction; } }
    private AICharacterInteraction _interaction;

    public CharacterAction Action { get; private set; }

    public List<string> EquippedItemIds { get; set; } = new();


    [Header("AI 캐릭터 현재 상태")]
    public Define.EAIState CurrentState;

    [Header("AI 캐릭터 배정된 건물")]
    public BuildingBase currentBuilding;
    
    #region Bone
    [SerializeField] private Transform hatBone;
    [SerializeField] private Transform bagBone;
    [SerializeField] private Transform accessoryBone;
    [SerializeField] public Dictionary<EEquipmentType, Transform> equipmentBones = new Dictionary<EEquipmentType, Transform>();

    #endregion

    #region Hide
    [HideInInspector]
    public EAIState loadState;

    [HideInInspector]
    public BuildingBase loadBuilding;

    [HideInInspector]
    public bool _isHelloReady = true;
    
    //UI 상에 보일 캐릭터들
    [HideInInspector]
    public bool IsReplica = false;

    [HideInInspector]
    public bool inMain = false; // 캐릭터가 메인 안에 있는지 여부
    [HideInInspector]
    public ECropType _ecropType;


    #endregion

    #region Action
    public Action<AICharacter> AnimalLeaved;
    public Action<AICharacter> AnimalArrived;
    public Action<AICharacter> AnimalDelivered;
    public Action<int> CharacterGainExp;
    public Action<float> Levelup;
    #endregion

    private void Update()
    {
        if (IsReplica) return;

        if (Controller == null) return;

        if (!View.Nav.enabled)
        {
            View.Nav.enabled = true;
            return;
        }

        Controller.OnUpdate(Time.deltaTime);
        if (BuildingPlacer.Instance == null || !BuildingPlacer.Instance.isAI) Interaction.LongPressClick();

    }


    private void LateUpdate()
    {
        if (_controller == null) return;
        Interaction.ClickToSet();
    }



    #region 생성 시 초기화 및 불러오기
    public override bool Init()
    {
        ObjectType = Define.EObjectType.Character;
        InitEquipBones();
        Action    = GetComponent<CharacterAction>();
        _view              = GetComponentInChildren<AICharacterView>();
        _effect            = GetComponentInChildren<AICharacterLevelEffectHandler>();
        _interaction       = GetComponentInChildren<AICharacterInteraction>();
        _stat              = GetComponentInChildren<AICharacterStat>();

        return true;
    }

    public void SetInfo(Character ch)
    {
        Stat.Init(this);
        Stat.data = ch;
        // 위치값
        if (Managers.Scene.CurrentScene is GameScene)
        {
            transform.position = Stat.data.Pos.ToVector3();
            CurrentState = Stat.data.CurrentState;
            loadState = Stat.data.CurrentState;
            loadBuilding = Stat.data.LoadBuilding;
        }


        // TODO : FSM 등 상태 적용
        _controller = new AIController(new CharacterResetState(),this, Define.EAIState.None);
        Effect.Init();
        View.Init(this);
        Interaction.Init(this);


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

    #region 클릭 상호작용
    public override void OnClick()
    {
        if (InputUtility.IsPointerOverUI())
            return;

        if (!Interaction.isClicked && Controller.CurrentState() is not CharacterHelloState)
        {
            System.Random random = new System.Random();
            string randomCatSound = Define.CAT_SOUNDS[random.Next(Define.CAT_SOUNDS.Length)];
            Managers.Sound.Play(Define.ESound.Effect, randomCatSound);
        }

        if (gameObject == this.gameObject &&
            Controller.CurrentState() is not CharacterHelloState)
        {
            Interaction.isClicked = !Interaction.isClicked;
            Interaction.clickStartTime = 0;
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
        Stat.data = character;

        Managers.Equipment.ApplyEquipmentPreview(this, character);
    }

    #endregion

    #region 캐릭터 제거 시
    public void OnDestroy()
    {
        Controller?.Dispose();
        Managers.AI.Unregister(this);
    }
    #endregion
}


