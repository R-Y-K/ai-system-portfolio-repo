using UnityEngine;

public abstract class GoalStateSO : StateSO
{
    [SerializeField] protected bool debugMode;

    public abstract void ChangeState(Character character, StepBehaviourControllerSO newState);
}
