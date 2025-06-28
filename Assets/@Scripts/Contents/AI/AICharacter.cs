using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Contents.AI.FSM.State;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class AICharacter : BaseObject
{
    public AIController Controller { get { return _controller; } }
    private AIController _controller;

    public CharacterStatSo Stat { get { return runtimeStat; } }
    private CharacterStatSo runtimeStat;

    [SerializeField] private CharacterStatSo originStat;

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

    private bool _isHelloReady = true;

   public Sprite[] sprites;
    public Sprite sprite;

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


        if (runtimeStat != null)
            runtimeStat.OnStatChanged -= ApplyStat;
        else
            runtimeStat = originStat.Clone();

        runtimeStat.OnStatChanged += ApplyStat;

        ApplyStat();
        AIManager.Instance.Register(this);
        ControllerRegister();

        return true;
    }


    public void ControllerRegister()
    {
        _controller = new AIController(new CharacterIdleState(), this, Define.EAIState.Idle);
        _controller.RegisterState(new CharacterBuildingState(), this,Define.EAIState.Building);
        _controller.RegisterState(new CharacterCookState(), this, Define.EAIState.Cooking); ;
        _controller.RegisterState(new CharacterFarmingState(), this, Define.EAIState.Farming);
        _controller.RegisterState(new CharacterPlayState(), this, Define.EAIState.Playing);
        _controller.RegisterState(new CharacterRestState(), this, Define.EAIState.Resting);
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

    public void ApplyStat()
    {
        nav.speed = runtimeStat.MoveSpeed;
       
    }

    public void UseStamina(float amount)
    {
        if (runtimeStat.Stamina - amount < 0)
        {
            return;
        }
        runtimeStat.Stamina = Mathf.Max(0, runtimeStat.Stamina - amount);
        Debug.Log($"Stamina used: {amount}, Remaining: {runtimeStat.Stamina}");
    }

    public void OnLevelUp()
    {
        runtimeStat.MoveSpeed += 1;
        runtimeStat.Hp += 10;
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
