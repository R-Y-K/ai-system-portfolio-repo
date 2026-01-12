using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "FindLowestHealth",
    menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Step Data/Find Target Condition/Find Lowest Health",
    order = -1000 // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
)]
public class FindLowestHealthSO : StepFindTargetCriteriaSO
{
    // Find Lowest Health Target criteria to find the ally/enemy with the lowest health percentage
    public override Character FindTarget(Character controller, List<Character> candidates)
    {
        if (candidates == null || candidates.Count == 0)
            return null;

        Character lowest = null;
        float lowestHealthPct = float.MaxValue;

        foreach (var candidate in candidates)
        {
            if (candidate == null || candidate.resourceSystem == null)
                continue;

            // skip dead
            if (candidate.resourceSystem.currentHealth <= 0f)
                continue;

            float healthPct = candidate.resourceSystem.GetHealthPercentage();
            if (healthPct < lowestHealthPct)
            {
                lowestHealthPct = healthPct;
                lowest = candidate;
            }
        }

        return lowest;
    }
}
