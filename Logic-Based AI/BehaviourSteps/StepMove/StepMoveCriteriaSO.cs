using UnityEngine;

public abstract class StepMoveCriteriaSO : ScriptableObject
{
    public NavigationType navigationType = NavigationType.WithUnityNavMeshAgent;

    public bool debugMode;
    protected float lastTime;
    public abstract void Move(Character character);
}
