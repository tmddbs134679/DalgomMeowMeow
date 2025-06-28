

using UnityEngine;

public abstract class BaseState<T> 
{
    protected float elapsedTime;

    public BaseState() { }
    public virtual void Init(T owner) { }
    public virtual void OnEnter()
    {
        Debug.Log("Enter : " + this.GetType().Name);
        elapsedTime = 0f;
    }

    public virtual void OnUpdate(float deltaTime)
    {
        elapsedTime += deltaTime;
    }


    public virtual void OnExit() { }







}