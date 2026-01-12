using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Target Behaviour Data", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Steps/Find Target", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class StepFindTargetSO : StepStateSO
{
    [Header("Find Target Criteria")]
    [SerializeField] StepFindTargetCriteriaSO findTargetCriteria;

    public override void OnEnter(Character character)
    {
        // Always remove old data when entering this step to ensure it is fresh. 
        // Could also check if the old data is the same as the current data and skip the tick, but this is simpler and less error prone. Fix if it becomes a problem. 
        character.currentTarget = null;
    }
    protected override void RunState(Character character)
    {
        // First determine what type of skill we are dealing with.
        SkillDataSO skillData = character.selectedSkill;
        SkillType skillType = skillData.skillType;

        List<Character> candidateList = null;

        switch (skillType)
        {
            case SkillType.OffensiveSkill: // We are dealing with a damaging skill, so we look for hostile targets.
                candidateList = character.objectDetectionSystem.GetEnemies();
                break;

            case SkillType.SupportiveSkill: // We are dealing with a supporting skill, so we look for friendly targets.
                candidateList = character.objectDetectionSystem.GetAllies();
                break;

            default:
                Debug.LogWarning($"FindTargetCondition: '{skillType}' not recognized. Cannot determine target type.");
                return;
        }

        // If no candidates, bail early.
        if (candidateList == null)
        {
            if (debugMode) Debug.LogWarning($"No targets found by FindTargetCondition for skill type '{skillType}'. If this is not intended, please check setup.");
            return;
        }

        // Find target using the criteria.
        Character foundTarget = findTargetCriteria.FindTarget(character, candidateList);

        // Store result in character script
        character.currentTarget = foundTarget;
    }
    public override void OnExit(Character character)
    {
        
    }
}
