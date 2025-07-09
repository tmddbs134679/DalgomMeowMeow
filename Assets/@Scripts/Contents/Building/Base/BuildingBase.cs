using System.Collections;
using System.Collections.Generic;
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

    // [Header("동물 배치")]
    //protected Animal assignedAnimal;
    //주석추가

    public int CurrentLevel { get; protected set; } = 1;

    public int UniqueId { get; private set; }


    protected virtual void Start()
    {
        if (Timer == null)
            Init();
    }

    public override bool Init()
    {
        Timer = new BuildingTimer(BuildingData.Interval);
        BuildingManager.Instance.Register(this);
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
}