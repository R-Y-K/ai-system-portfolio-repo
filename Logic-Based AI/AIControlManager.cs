using System.Collections.Generic;
using UnityEngine;

public class AIControlManager : MonoBehaviour
{
    private Character character;
    public bool isAIEnabled = true;

    [SerializeField] private List<BehaviourControllerSO> behaviourControllers;
    [SerializeField] BehaviourControllerSO activeController;

    // Runtime state indices (per Character, never inside SOs!)

    public GoalBehaviourControllerSO currentGoalBehaviour;
    public StepBehaviourControllerSO currentStepBehaviour;
    public int CurrentGoalIndex { get; set; }
    public int CurrentStepBehaviourIndex { get; set; }
    public int CurrentStepStateIndex { get; set; }

    private void Awake()
    {
        character = GetComponent<Character>();
        if (character == null)
        {
            Debug.LogError("AI_ControllerManager: No Character component found.");
        }
    }

    private void Start()
    {
        // Default to first role if none selected
        if (behaviourControllers.Count > 0)
        {
            SwitchController(behaviourControllers[0]);
        }
        else
        {
            Debug.LogWarning("AI_ControllerManager: No BehaviourControllers assigned.");
        }
    }

    private void Update()
    {
        if (!isAIEnabled) return; // Don't run if AI is is not active.
        activeController?.Tick(character, this);
    }

    public void SwitchController(BehaviourControllerSO newController)
    {
        if (activeController != null)
        {
            activeController.OnExit(character, this);
        }

        activeController = newController;

        if (activeController != null)
        {
            CurrentGoalIndex = 0;
            CurrentStepBehaviourIndex = 0;
            CurrentStepStateIndex = 0;

            activeController.OnEnter(character, this);
        }
    }
}
