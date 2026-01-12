using System.Collections.Generic;
using UnityEngine;

public abstract class StepFindTargetCriteriaSO : ScriptableObject
{
    public abstract Character FindTarget(Character character, List<Character> list);
}
