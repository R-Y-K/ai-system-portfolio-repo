using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectRandomSkill", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Step Data/Select Skill Conditions/Random Skill", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class RandomSkillSO : StepSelectSkillCriteriaSO
{
    public override void SelectSkill(Character character, List<SkillDataSO> skillList)
    {
        int randomIndex = Random.Range(0, skillList.Count);
        character.selectedSkill = skillList[randomIndex];
        if (debugMode)
        {
            Debug.Log($"Selected random skill: {skillList[randomIndex].skillName}");
        }
    }
}
