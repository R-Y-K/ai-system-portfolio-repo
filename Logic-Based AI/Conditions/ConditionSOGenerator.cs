#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ConditionSOGenerator
{
    [MenuItem("Tools/Regenerate/ConditionSOs")]
    public static void GenerateAllConditions()
    {
        // Find where the ConditionSO.cs script lives
        string[] guids = AssetDatabase.FindAssets("ConditionSO t:Script");
        if (guids.Length == 0)
        {
            Debug.LogError("Could not find ConditionSO script in project.");
            return;
        }

        string scriptPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        string scriptDir = Path.GetDirectoryName(scriptPath);
        string conditionsFolder = Path.Combine(scriptDir, "ConditionSOs");

        // Ensure "ConditionSOs" folder exists
        if (!AssetDatabase.IsValidFolder(conditionsFolder))
        {
            AssetDatabase.CreateFolder(scriptDir, "ConditionSOs");
        }

        // Go through all ConditionTypes
        foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
        {
            string assetName = type.ToString();
            string assetPath = Path.Combine(conditionsFolder, assetName + ".asset");

            // Try to load existing
            ConditionSO existing = AssetDatabase.LoadAssetAtPath<ConditionSO>(assetPath);

            if (existing == null)
            {
                // Create new
                ConditionSO newSO = ScriptableObject.CreateInstance<ConditionSO>();
                newSO.name = assetName;
                newSO.type = type;

                AssetDatabase.CreateAsset(newSO, assetPath);
                Debug.Log($"[ConditionSOGenerator] Created new ConditionSO: {assetPath}");
            }
            else
            {
                // If exists, check if type matches
                if (existing.type != type)
                {
                    existing.type = type;
                    EditorUtility.SetDirty(existing);
                    Debug.LogWarning($"[ConditionSOGenerator] Fixed type mismatch in {assetPath}");
                }
                else
                {
                    Debug.Log($"[ConditionSOGenerator] ConditionSO already correct: {assetPath}");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("[ConditionSOGenerator] All ConditionSOs generated and validated successfully.");
    }
}
#endif

