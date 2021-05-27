using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState : State
{
    public WaveState(GameManager gameManager) : base(gameManager)
    {
    }

    public override IEnumerator EnterState()
    {
        gameManager.EnemySpawnHelper.GetSequence();
        yield break;
    }

    public override IEnumerator ExitState(State newState)
    {
        gameManager.EnterNewState(newState);
        yield break;
    }
}
