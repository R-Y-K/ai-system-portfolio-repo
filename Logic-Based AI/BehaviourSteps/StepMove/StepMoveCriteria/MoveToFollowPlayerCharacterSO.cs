using UnityEngine;

[CreateAssetMenu(fileName = "MoveToFollowCurrentTarget", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Step Data/Move Condition/Follow Current Target", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class MoveToFollowPlayerCharacterSO : StepMoveCriteriaSO
{
    float desiredFollowDistance = 3.0f; // Desired distance to maintain from the target while following.
    public override void Move(Character character)
    {
        switch (navigationType)
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
            if (desiredFollowDistance >= Vector3.Distance(character.transform.position, character.objectDetectionSystem.GetPlayerCharacter().transform.position))
            {
                // We are within the desired follow distance, no need to move closer.
                return;
            }
            Vector3 targetPosition = character.objectDetectionSystem.GetPlayerCharacter().transform.position;

            Vector3 direction = (targetPosition - character.transform.position).normalized;
            character.transform.Translate(direction * character.characterData.baseMovementSpeed * Time.deltaTime, Space.World);
        }
        void WithNavMesh(Character character)
        {
            var player = character.objectDetectionSystem.GetPlayerCharacter();
            if (player == null) return;

            float distance = Vector3.Distance(character.transform.position, player.transform.position);

            if (distance <= desiredFollowDistance)
            {
                // Stop moving
                if (!character.navMeshAgent.isStopped)
                {
                    character.navMeshAgent.velocity = Vector3.zero; // Stop velocity immediately.
                    character.navMeshAgent.isStopped = true;
                    character.navMeshAgent.ResetPath();
                }

                // Rotate to face the target
                float rotationSpeed = 10f;
                Vector3 directionToTarget = (player.transform.position - character.transform.position).normalized;
                directionToTarget.y = 0;
                if (directionToTarget != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                    character.transform.rotation = Quaternion.Slerp(
                        character.transform.rotation,
                        lookRotation,
                        Time.deltaTime * rotationSpeed
                    );
                }
            }
            else
            {
                // Resume moving if previously stopped
                if (character.navMeshAgent.isStopped) character.navMeshAgent.isStopped = false;

                character.navMeshAgent.stoppingDistance = desiredFollowDistance - 0.1f;
                character.navMeshAgent.speed = character.characterData.baseMovementSpeed;
                character.navMeshAgent.SetDestination(player.transform.position);
            }
        }
    }
}
