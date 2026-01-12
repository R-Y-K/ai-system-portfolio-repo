using UnityEngine;

public enum ConditionType
{
    UserHasTarget, UserDoesNotHaveTarget,
    CurrentTargetDead, CurrentTargetNotDead,
    InIntendedSkillRange, NotInIntendedSkillRange,
    SkillUsable, SkillNotUsable,
    SkillSelected, SkillNotSelected,
    OffensiveSkillsOnCooldown, OffensiveSkillsNotOnCooldown,
    SupportiveSkillsOnCooldown, SupportiveSkillsNotOnCooldown,
    AllSkillsOnCooldown, AllSkillsNotOnCooldown,
    AllAlliesAtFullHealth, AllAlliesNotAtFullHealth,
    EnemyCharactersNearby, EnemyCharactersNotNearby,
    AllyCharactersNearby, AllyCharactersNotNearby,
    PlayerCharacterNearby, PlayerCharacterNotNearby,
    // ... add more
}
public class ConditionSO : ScriptableObject
{
    public ConditionType type;
    public float floatValue;   // for thresholds (health %, distance, etc.)
    public string stringValue; // for skill names, tags, etc.

    public bool IsSatisfied(Character character)
    {
        switch (type)
        {
            // case ConditionType.ExampleCondition: { return false; else return true; }

            case ConditionType.UserHasTarget: { if (character.currentTarget != null) return true; else return false; }
            case ConditionType.UserDoesNotHaveTarget: { if (character.currentTarget == null) return true; else return false; }
            case ConditionType.CurrentTargetDead:
                {
                    if (character.currentTarget == null) return true;
                    if (character.currentTarget.resourceSystem.IsDead()) return true;
                    else return false;
                }
            case ConditionType.CurrentTargetNotDead:
                {
                    if (character.currentTarget == null) return false;
                    if (!character.currentTarget.resourceSystem.IsDead()) return true;
                    else return false;
                }
            case ConditionType.SkillSelected: { if (character.selectedSkill != null) return true; else return false; }
            case ConditionType.SkillNotSelected: { if (character.selectedSkill == null) return true; else return false; }
            case ConditionType.InIntendedSkillRange: { if (character.skillSystem.SkillActivationRangeCheck(character.selectedSkill, character.currentTarget)) return true; else return false; }
            case ConditionType.NotInIntendedSkillRange: { if (!character.skillSystem.SkillActivationRangeCheck(character.selectedSkill, character.currentTarget)) return true; else return false; }
            case ConditionType.SkillUsable:
                {
                    if (character.selectedSkill == null) return false;

                    if (CentralizedCooldownManagementSystem.Instance.CheckGlobalAndSkillCooldown(character.characterID, character.selectedSkill)) return true;
                    else return false;
                }
            case ConditionType.SkillNotUsable:
                {
                    if (character.selectedSkill == null) return true;

                    if (!CentralizedCooldownManagementSystem.Instance.CheckGlobalAndSkillCooldown(character.characterID, character.selectedSkill)) return true;
                    else return false;
                    
                }
            case ConditionType.OffensiveSkillsOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (skill.skillType == SkillType.OffensiveSkill)
                        {
                            if (!CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                                return false; // Found an offensive skill that is NOT on cooldown
                        }
                    }
                    return true; // All offensive skills are on cooldown
                }
            case ConditionType.OffensiveSkillsNotOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (skill.skillType == SkillType.OffensiveSkill)
                        {
                            if (CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                                return false; // Found an offensive skill that IS on cooldown
                        }
                    }
                    return true; // All offensive skills are not on cooldown
                }
            case ConditionType.SupportiveSkillsOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (skill.skillType == SkillType.SupportiveSkill)
                        {
                            if (!CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                                return false; // Found a supportive skill that is NOT on cooldown
                        }
                    }
                    return true; // All supportive skills are on cooldown
                }
            case ConditionType.SupportiveSkillsNotOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (skill.skillType == SkillType.SupportiveSkill)
                        {
                            if (CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                                return false; // Found a supportive skill that IS on cooldown
                        }
                    }
                    return true; // All supportive skills are not on cooldown
                }
            case ConditionType.AllSkillsOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (!CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                            return false; // Found a skill that is NOT on cooldown
                    }
                    return true; // All skills are on cooldown
                }
            case ConditionType.AllSkillsNotOnCooldown:
                {
                    foreach (var skill in character.SkillData)
                    {
                        if (CentralizedCooldownManagementSystem.Instance.IsSkillCooldownActive(character.characterID, skill.skillName))
                            return false; // Found a skill that IS on cooldown
                    }
                    return true; // All skills are not on cooldown
                }
            case ConditionType.AllAlliesAtFullHealth:
                {
                    var allies = character.objectDetectionSystem.GetAllies();
                    if (allies == null || allies.Count == 0)
                    {
                        if (character.controllerDebugMode) Debug.Log("[AllAlliesFullHealth] No allies found, returning true");
                        return true;
                    }

                    foreach (var ally in allies)
                    {
                        if (ally == null || ally.resourceSystem == null) continue;
                        if (ally.resourceSystem.currentHealth <= 0f) continue;
                        float hpPct = ally.resourceSystem.GetHealthPercentage();
                        if (hpPct < 1f)
                        {
                            if (character.controllerDebugMode) Debug.Log($"[AllAlliesFullHealth] Ally {ally.name} is not full HP ({hpPct * 100:F1}%), returning false");
                            return false;
                        }
                    }

                    if (character.controllerDebugMode) Debug.Log("[AllAlliesFullHealth] All allies at full health, returning true");
                    return true; // All allies are at full health
                }
            case ConditionType.AllAlliesNotAtFullHealth:
                {
                    var allies = character.objectDetectionSystem.GetAllies();
                    if (allies == null || allies.Count == 0)
                    {
                        if (character.controllerDebugMode) Debug.Log("[AllAlliesNotAtFullHealth] No allies found, returning false");
                        return false;
                    }
                    foreach (var ally in allies)
                    {
                        if (ally == null || ally.resourceSystem == null) continue;
                        if (ally.resourceSystem.currentHealth <= 0f) continue;
                        float hpPct = ally.resourceSystem.GetHealthPercentage();
                        if (hpPct < 1f)
                        {
                            if (character.controllerDebugMode) Debug.Log($"[AllAlliesNotAtFullHealth] Ally {ally.name} is not full HP ({hpPct * 100:F1}%), returning true");
                            return true; // Found at least one ally not at full health
                        }
                    }

                    if (character.controllerDebugMode) Debug.Log("[AllAlliesNotAtFullHealth] All allies at full health, returning false");
                    return false; // All allies are not at full health.
                }
            case ConditionType.EnemyCharactersNearby:
                {
                    if (character.objectDetectionSystem.GetEnemies().Count > 0) return true;
                    else return false;
                }
            case ConditionType.EnemyCharactersNotNearby:
                {
                    if (character.objectDetectionSystem.GetEnemies().Count == 0) return true;
                    else return false;
                }
            case ConditionType.AllyCharactersNearby:
                {
                    if (character.objectDetectionSystem.GetAllies().Count > 0) return true;
                    else return false;
                }
            case ConditionType.AllyCharactersNotNearby:
                {
                    if (character.objectDetectionSystem.GetAllies().Count == 0) return true;
                    else return false;
                }
            case ConditionType.PlayerCharacterNearby:
                {
                    if (character.objectDetectionSystem.GetPlayerCharacter() != null) return true;
                    else return false;
                }
            case ConditionType.PlayerCharacterNotNearby:
                {
                    if (character.objectDetectionSystem.GetPlayerCharacter() == null) return true;
                    else return false;
                }
            default:
                return false;
        }
    }
}

