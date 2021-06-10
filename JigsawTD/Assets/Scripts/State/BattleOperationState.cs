using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateName
{
    BuildingState, WaveState, PickingState,WonState, LoseState
}
public abstract class BattleOperationState
{
    public abstract StateName StateName { get; }
    protected GameManager gameManager;

    public BattleOperationState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public virtual IEnumerator EnterState()
    {
        yield break;
    }

    public virtual IEnumerator ExitState(BattleOperationState newState)
    {
        yield break;
    }
}
