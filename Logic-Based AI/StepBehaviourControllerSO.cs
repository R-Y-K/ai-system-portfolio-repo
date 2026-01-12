using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Step Behaviour",
    menuName = "YAAB/ScriptableObjects/Behaviour Controller/Step Behaviour",
    order = -1000)]
public class StepBehaviourControllerSO : StepStateSO
{
    [SerializeField] private List<StepStateSO> stepStates;

    public override void OnEnter(Character character)
    {
        // Initialize all child states
        foreach (var step in stepStates)
        {
            if (step != null && step.CanEnter(character))
                step.OnEnter(character);
        }
    }

    protected override void RunState(Character character)
    {
        foreach (var step in stepStates)
        {
            if (step == null) continue;

            // Only run if it can enter and isn’t already complete
            if (step.CanEnter(character) && !step.IsComplete(character))
            {
                step.Tick(character);
            }
        }
    }

    public override void OnExit(Character character)
    {
        character.selectedSkill = null; // Clear selected skill after use to ensure it is fresh next time we enter this step.
        character.currentTarget = null; // Clear target after use to ensure it is fresh next time we enter this step.

        foreach (var step in stepStates)
        {
            step?.OnExit(character);
        }
    }
}