using System.Collections.Generic;
using UnityEngine;

public abstract class StateSO : ScriptableObject
{
    [SerializeField] protected List<ConditionSO> enterConditions = new();
    [SerializeField] protected List<ConditionSO> cannotEnterConditions = new();
    [SerializeField] protected List<ConditionSO> isCompleteConditions = new();

    public abstract void OnEnter(Character character);
    public abstract void OnExit(Character character);

    // Called once before entering state
    public bool CanEnter(Character character) // Require all enter conditions to be satisfied and none of the cannot enter conditions to be satisfied.
    {
        foreach (var condition in cannotEnterConditions)
        {
            if (condition.IsSatisfied(character)) return false;
        }
        foreach (var condition in enterConditions)
        {
            if (!condition.IsSatisfied(character)) return false; 
        }
        return true;
    }

    // Called every frame while state is active
    public bool Tick(Character character) 
    {
        if (IsComplete(character)) return false; // Do not run if the purpose of the state has been fulfilled.
        RunState(character);
        return true;
    }

    // Called once before leaving state
    public bool IsComplete(Character character) // Require any isComplete condition to be satisfied.
    {
        foreach (var condition in isCompleteConditions)
        {
            if (condition.IsSatisfied(character)) return true;
        }
        return false;
    }

    // Concrete states implement their own logic
    protected abstract void RunState(Character character);
}
