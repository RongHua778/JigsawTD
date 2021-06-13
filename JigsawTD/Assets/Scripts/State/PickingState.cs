using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickingState : BattleOperationState
{
    public override StateName StateName => StateName.PickingState;

    public PickingState(GameManager gameManager) : base(gameManager)
    {
        
    }

    public override IEnumerator EnterState()
    {
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
