using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState : BattleOperationState
{

    public WaveState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.WaveState;

    public override IEnumerator EnterState()
    {
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.EnterNewState(newState);
        yield break;
    }
}
