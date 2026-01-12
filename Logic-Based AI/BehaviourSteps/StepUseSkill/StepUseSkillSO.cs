using UnityEngine;

[CreateAssetMenu(fileName = "New Use Skill Behaviour Data", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Steps/Use Skill", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class StepUseSkillSO : StepStateSO
{
    public override void OnEnter(Character character)
    {
        
    }

    protected override void RunState(Character character)
    {
        character.skillSystem.TryToUseSkill(character.selectedSkill, character.currentTarget);
    }
    public override void OnExit(Character character)
    {

    }
}
