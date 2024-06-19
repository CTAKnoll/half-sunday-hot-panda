using Behavior;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableUnitBehavior : ScriptableObject
{
    public abstract IUnitBehavior GetBehavior(GameObject parent);
}

public abstract class BaseUnitBehavior : IUnitBehavior
{
    protected GameObject Parent;

    public BaseUnitBehavior()
    {

    }

    public void SetParent()
    {

    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public abstract IUnitBehavior.Result Process(float deltaTime);
}
