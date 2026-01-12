using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveTowardsTargetWithoutNavMesh", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Step Data/Move Condition/Move Towards Target", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class MoveToUseSkillOnCurrentTargetSO : StepMoveCriteriaSO
{
    public override void Move(Character character)
    {
        switch(navigationType)
        {
            case NavigationType.WithoutNavMesh:
                WithoutNavMesh(character);
                break;
            case NavigationType.WithUnityNavMeshAgent:
                WithNavMesh(character);
                break;
            default:
                Debug.LogWarning($"MoveTowardsTargetSO: Navigation type '{navigationType}' not recognized. Cannot move.");
                break;
        }
        void WithoutNavMesh(Character character)
        {
            // Without NavMesh
            if (character.skillSystem.SkillActivationRangeCheck(character.selectedSkill, character.currentTarget))
            {
                // We are in range, no need to move.
                return;
            }
            Vector3 targetPosition = character.currentTarget.transform.position;

            Vector3 direction = (targetPosition - character.transform.position).normalized;
            character.transform.Translate(direction * character.characterData.baseMovementSpeed * Time.deltaTime, Space.World);
        }
        void WithNavMesh(Character character)
        {
            // With NavMesh

            if (character.selectedSkill == null)
            {
                if (debugMode) Debug.LogWarning($"MoveToUseSkillOnCurrentTargetSO: No selected skill found on character: '{character.gameObject.name}'. Cannot move towards target.");
                return;
            }

            // If we are in range, no need to move.
            if (character.skillSystem.SkillActivationRangeCheck(character.selectedSkill, character.currentTarget))
            {
                character.navMeshAgent.velocity = Vector3.zero; // Stop velocity immediately.
                character.navMeshAgent.isStopped = true;
                character.navMeshAgent.ResetPath(); // Stop moving.
                // Instead we turn to face the target.
                if (character.currentTarget != null)
                {
                    float rotationSpeed = 10f;
                    Vector3 directionToTarget = (character.currentTarget.transform.position - character.transform.position).normalized;
                    directionToTarget.y = 0; // Keep only horizontal rotation.
                    if (directionToTarget != Vector3.zero) // Prevent errors when direction is zero.
                    {
                        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // Smooth rotation.
                    }
                }
                return;
            }

            if (character.navMeshAgent.destination == character.currentTarget.transform.position)
            {
                // Already moving towards the target position, no need to update destination.
                return;
            }

            character.navMeshAgent.stoppingDistance = character.selectedSkill.skillRange - 0.1f; // Make sure we inside skill range.
            character.navMeshAgent.speed = character.characterData.baseMovementSpeed;
            character.navMeshAgent.SetDestination(character.currentTarget.transform.position);
        }
    }
}
