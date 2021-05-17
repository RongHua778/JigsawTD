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
        Debug.Log("Eneter Wave State");
        gameManager.EnemySpawnHelper.GetSequence();
        yield break;
    }

    public override IEnumerator ExitState(State newState)
    {
        Debug.Log("Exit Wave State");
        gameManager.EnterNewState(newState);
        yield break;
    }
}
