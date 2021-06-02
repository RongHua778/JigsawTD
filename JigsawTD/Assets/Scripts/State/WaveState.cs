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
        gameManager.EnemySpawnHelper.GetSequence();
        EnemySequence sequence = gameManager.EnemySpawnHelper.RunningSequence;
        if (sequence.Wave == StaticData.Instance.LevelMaxWave)
        {
            Sound.Instance.PlayBg("lastwave");
        }
        else
        {
            switch (sequence.EnemyAttribute.EnemyType)
            {
                case EnemyType.Soilder:
                    Sound.Instance.PlayBg("soldier");
                    break;
                case EnemyType.Runner:
                    Sound.Instance.PlayBg("runner");
                    break;
                case EnemyType.Restorer:
                    Sound.Instance.PlayBg("restorer");
                    break;
                case EnemyType.Tanker:
                    Sound.Instance.PlayBg("tanker");
                    break;
            }
        }
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.EnterNewState(newState);
        yield break;
    }
}
