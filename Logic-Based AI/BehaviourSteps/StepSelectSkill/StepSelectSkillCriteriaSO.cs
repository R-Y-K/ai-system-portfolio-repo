using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class StepSelectSkillCriteriaSO : ScriptableObject
{
    [SerializeField] protected bool debugMode;
    public abstract void SelectSkill(Character character, List<SkillDataSO> skillList);
}
