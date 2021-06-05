using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState : BattleOperationState
{
    WaveSystem m_WaveSystem;
    public WaveState(GameManager gameManager,WaveSystem waveSystem) : base(gameManager)
    {
        m_WaveSystem = waveSystem;
    }

    public override StateName StateName => StateName.WaveState;

    public override IEnumerator EnterState()
    {
        m_WaveSystem.GetSequence();
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.EnterNewState(newState);
        yield break;
    }
}
