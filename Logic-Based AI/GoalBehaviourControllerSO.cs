using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Goal Behaviour",
    menuName = "YAAB/ScriptableObjects/Behaviour Controller/Goal Behaviour",
    order = -1000)]
public class GoalBehaviourControllerSO : GoalStateSO
{
    [SerializeField] private List<StepBehaviourControllerSO> stepBehaviours;

    public override void OnEnter(Character character)
    {
        if (stepBehaviours == null || stepBehaviours.Count == 0)
        {
            Debug.LogWarning($"{name}: No StepBehaviours assigned. AI idle.");
            return;
        }

        // Start with first StepBehaviour
        character.controllerManager.CurrentStepBehaviourIndex = 0;
        ChangeState(character, stepBehaviours[0]);
    }

    protected override void RunState(Character character)
    {
        var stepBehaviour = GetCurrentStepBehaviour(character);
        if (stepBehaviour == null) return;

        stepBehaviour.Tick(character);

        if (stepBehaviour.IsComplete(character))
        {
            character.controllerManager.CurrentStepBehaviourIndex++;
            if (character.controllerManager.CurrentStepBehaviourIndex < stepBehaviours.Count)
            {
                ChangeState(character, stepBehaviours[character.controllerManager.CurrentStepBehaviourIndex]);
            }
            else
            {
                // Goal complete
                Debug.Log($"{name}: Goal complete for {character.name}");
            }
        }
    }

    public override void OnExit(Character character)
    {
        var stepBehaviour = GetCurrentStepBehaviour(character);
        stepBehaviour?.OnExit(character);
    }

    private StepBehaviourControllerSO GetCurrentStepBehaviour(Character character)
    {
        int idx = character.controllerManager.CurrentStepBehaviourIndex;
        if (idx >= 0 && idx < stepBehaviours.Count)
            return stepBehaviours[idx];
        return null;
    }

    public override void ChangeState(Character character, StepBehaviourControllerSO newState)
    {
        GetCurrentStepBehaviour(character)?.OnExit(character);
        character.controllerManager.currentStepBehaviour = newState;
        newState?.OnEnter(character);
    }
}