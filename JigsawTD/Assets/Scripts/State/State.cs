using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateName
{
    BuildingState, WaveState, WonState, LoseState
}
public abstract class State
{
    protected GameManager gameManager;

    public State(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    public virtual IEnumerator EnterState()
    {
        yield break;
    }

    public virtual IEnumerator ExitState(State newState)
    {
        yield break;
    }
}
