using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "New Select Skill Behaviour Data", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Steps/Select Skill", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class StepSelectSkillSO : StepStateSO
{
    [Header("Select Skill Criteria")]
    [SerializeField] SkillType skillType;
    [SerializeField] StepSelectSkillCriteriaSO selectSkillCriteria;

    public override void OnEnter(Character character)
    {
        // Always remove old data from the blackboard when entering this step to ensure it is fresh.
        // Could also check if the old data is the same as the current data and skip the tick, but this is simpler and less error prone. Fix if it becomes a problem. 
        character.selectedSkill = null;
    }

    protected override void RunState(Character character)
    {
        // Hmm... should this check be somewhere else? Should we check this in controller at the moment of using behaviour?
        // If there are no skills to select from, or all skills are on cooldown, we cannot select a skill.
        if (character.SkillData.Count == 0 || GetSkillsWithoutActiveCooldowns(character).Count == 0)
        {
            if (debugMode) Debug.LogWarning("No skills available to select from.");
            return;
        }
        selectSkillCriteria.SelectSkill(character, GetSkillsWithoutActiveCooldowns(character));
    }

    public override void OnExit(Character character)
    {
        
    }
    public List<SkillDataSO> GetSkillsWithoutActiveCooldowns(Character character)
    {
        List<SkillDataSO> skillsWithoutCooldownsActive = new List<SkillDataSO>();

        foreach (var skill in character.SkillData)
        {
            if (!CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
            {
                skillsWithoutCooldownsActive.Add(skill);
            }
        }

        // Filter skills by the desired type
        var filteredSkills = skillsWithoutCooldownsActive.Where(skill => skill.skillType == skillType).ToList();

        if (filteredSkills.Count == 0)
        {
            if (debugMode) Debug.LogWarning($"No skills of type {skillType} found.");
            //return null; // Never return null for collections, return empty collection instead.
            return new List<SkillDataSO>(); // Nothing to select.
        }
        return filteredSkills;
    }
}
