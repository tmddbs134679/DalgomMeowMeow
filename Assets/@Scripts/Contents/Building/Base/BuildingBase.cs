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
    [Header("기본 정보")] public BuildingDataSO BuildingData;
    public BuildingState CurrentState = BuildingState.Locked;

    [Header("생산 타이머")] public BuildingTimer Timer;
    public int StoredCount { get; protected set; } = 0; // 누적된 생산 수량
    
    protected AICharacter assignedAnimal;

    // [Header("동물 배치")]
    //protected Animal assignedAnimal;

    protected virtual void Start()
    {
        if (Timer == null)
            Init();
    }

    public virtual void Init()
    {
        Timer = new BuildingTimer(BuildingData.Interval);
        BuildingManager.Instance.Register(this);
    }

    public virtual void ConnectToAnimal()
    {
        // 이벤트 등록
        //assignedAnimal.OnAnimalArrived += AssignAnimal;
        //animal.OnUnassigned += HandleAnimalUnassigned;
    }

    public virtual void Unlock()
    {
        CurrentState = BuildingState.Producing;
    }
    public virtual void Lock()
    {
        CurrentState = BuildingState.Idle;
    }
    
    public virtual void AssignAnimal()
    {
        Unlock();
    }
    public virtual void UnassignAnimal()
    {
        CurrentState = BuildingState.Idle;
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