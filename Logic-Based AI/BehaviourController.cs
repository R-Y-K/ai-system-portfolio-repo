using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New BehaviourController",
    menuName = "YAAB/ScriptableObjects/Behaviour Controller/BehaviourController",
    order = -1000)]
public class BehaviourControllerSO : ScriptableObject
{
    [SerializeField] private List<GoalBehaviourControllerSO> goalBehaviours;

    public void OnEnter(Character character, AIControlManager manager)
    {
        if (goalBehaviours.Count == 0)
        {
            Debug.LogWarning($"{name}: No GoalBehaviours assigned.");
            return;
        }
    }

    public void Tick(Character character, AIControlManager manager)
    {
        if (character.resourceSystem.IsDead()) return;

        // Always re-evaluate from the top (priority order)
        for (int i = 0; i < goalBehaviours.Count; i++)
        {
            var candidate = goalBehaviours[i];

            // Must be allowed to start AND not marked complete
            if (candidate.CanEnter(character) && !candidate.IsComplete(character))
            {
                // Switch goal if it's different
                if (character.controllerManager.currentGoalBehaviour != candidate)
                {
                    ChangeGoal(character, candidate);
                    manager.CurrentGoalIndex = i;
                }

                character.controllerManager.currentGoalBehaviour.Tick(character);
                return;
            }
        }

        // Nothing matched → idle / fallback
        character.controllerManager.currentGoalBehaviour = null;
        // character.controllerManager.currentGoalBehaviour = goalBehaviours[0];
    }

    public void OnExit(Character character, AIControlManager manager)
    {
        character.controllerManager.currentGoalBehaviour?.OnExit(character);
        character.controllerManager.currentGoalBehaviour = null;
    }

    private void ChangeGoal(Character character, GoalBehaviourControllerSO newGoal)
    {
        character.controllerManager.currentGoalBehaviour?.OnExit(character);
        character.controllerManager.currentGoalBehaviour = newGoal;
        character.controllerManager.currentGoalBehaviour.OnEnter(character);
    }
}
