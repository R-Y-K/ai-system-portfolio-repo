using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "FindNearestTarget",
    menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Step Data/Find Target Condition/Find Nearest Target",
    order = -1000
)]
public class FindNearestTargetSO : StepFindTargetCriteriaSO
{
    // Find Nearest Target criteria to find nearest target to user from a list of potential targets.
    public override Character FindTarget(Character character, List<Character> candidates)
    {
        if (candidates == null || candidates.Count == 0)
            return null;

        Character nearest = null;
        float shortestDistance = float.MaxValue;

        foreach (var candidate in candidates)
        {
            if (candidate == null) continue;

            float distance = Vector3.Distance(candidate.transform.position, character.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = candidate;
            }
        }

        return nearest;
    }
}
