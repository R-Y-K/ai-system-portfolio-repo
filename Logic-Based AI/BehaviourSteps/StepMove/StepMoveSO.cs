using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Behavior Data", menuName = "YAAB/ScriptableObjects/Behaviour Controller/Behaviour Steps/Move", order = -1000)] // Need to set priority to -1000 to ensure it appears at the top of the create list in the inspector.
public class StepMoveSO : StepStateSO
{
    [Header("Move Criteria")]
    [SerializeField] StepMoveCriteriaSO moveCriteria;
    public override void OnEnter(Character character)
    {

    }

    protected override void RunState(Character character)
    {
        moveCriteria.Move(character);
    }

    public override void OnExit(Character character)
    {

    }
}