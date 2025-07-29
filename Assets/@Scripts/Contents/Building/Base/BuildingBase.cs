using Data;
using System;
using UnityEngine;

public enum BuildingState
{
    Locked,
    Idle,
    Producing,
    ReadyToCollect
}

public abstract class BuildingBase : BaseObject
{
    [Header("기본 정보")] public BaseBuildingSO BuildingData;
    public BuildingState CurrentState = BuildingState.Locked;

    [Header("생산 타이머")] public BuildingTimer Timer;
    public int StoredCount { get; protected set; } = 0; // 누적된 생산 수량
    public AICharacter assignedAnimal;
    [SerializeField] protected Animator _animator;

    // [Header("동물 배치")]
    //protected Animal assignedAnimal;
    //주석추가
    public int CurrentLevel { get; protected set; } = 1;

    public int UniqueId { get; private set; }
    public BuildMap _buildMap;


    public void SetUniqueId(int id) => UniqueId = id;
    public void SetLevel(int level) => CurrentLevel = level;

    //TextAnimation
    [SerializeField]protected UI_TextAnimation _textAnim;

    public Action OnAutoSave;

    protected virtual void Start()
    {
        if (Timer == null)
            Init();
    }

    public override bool Init()
    {
        Timer = new BuildingTimer(BuildingData.Interval);
        BuildingManager.Instance.Register(this);

        if(_animator != null)
            AnimStop();

        return true;
    }

    public virtual void ConnectToAnimal(AICharacter animal)
    {
        if (animal == null) return;

        DisconnectAnimal();
        assignedAnimal = animal;
        assignedAnimal.AnimalArrived += AssignAnimal;
        assignedAnimal.AnimalLeaved += UnassignAnimal;
    }
    public virtual void DisconnectAnimal()
    {
        if (assignedAnimal == null) return;

        if (_animator != null)
            AnimStop();

        assignedAnimal.AnimalArrived -= AssignAnimal;
        assignedAnimal.AnimalLeaved -= UnassignAnimal;

        assignedAnimal = null;
    }

    public virtual void Unlock()
    {
        CurrentState = BuildingState.Producing;
    }
    public virtual void Lock()
    {
        CurrentState = BuildingState.Idle;
    }
    
    public virtual void AssignAnimal(AICharacter animal)
    {
        if (assignedAnimal != animal) return;

        if (_animator != null)
            AnimStart();

        Unlock();
    }
    public virtual void UnassignAnimal(AICharacter animal)
    {

        CurrentState = BuildingState.Idle;
        DisconnectAnimal();
    }
    public abstract void Produce(); // 자식이 반드시 override

    public virtual void Tick(float deltaTime)
    {
        if (CurrentState != BuildingState.Producing) return;

        if (Timer.Tick(deltaTime))
        {
            Produce();
        }
    }
    public void SetBuildMap(BuildMap buildMap)
    {
        _buildMap = buildMap;
    }
    
    public virtual bool CanUpgrade()
    {
        return Managers.Data.BuildingLevelDic.ContainsKey((BuildingData.Id.ToString(), CurrentLevel + 1));
    }

    public virtual bool Upgrade()
    {
        if (!CanUpgrade()) return false;

        BuildingLevelData next = Managers.Data.BuildingLevelDic[(BuildingData.Id.ToString(), CurrentLevel + 1)];
        if (Managers.Game.Gold <= next.UpgradeCost)
            return false;

        Managers.Game.Gold -= next.UpgradeCost;
        CurrentLevel++;
        _buildMap.UpdateBuildLevel(UniqueId, CurrentLevel);
        ApplyLevel();
        
        return true;
    }
    protected virtual void ApplyLevel()
    {
        Managers.Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}",Define.EDebugType.Building);
        //Debug.Log($"[CookingBuilding] 업그레이드 완료 → Lv.{CurrentLevel}, 생산 요리: {LevelData.ProducedFood.Name}");
        // 외형 변경, 사운드 등도 여기에
    }

    protected virtual void AnimStart()
    {
        _animator.speed = 1;
        _animator.enabled = true;
    }

    protected virtual void AnimStop()
    {
        _animator.speed = 0;
        _animator.Play(0, 0, 0f);
        _animator.Update(0f);
        _animator.enabled = false;
    }
}