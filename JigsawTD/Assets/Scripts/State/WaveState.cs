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
        yield return new WaitForSeconds(0.5f);
        m_WaveSystem.Running = true;
        //±≥æ∞“Ù¿÷…Ë÷√
        if (m_WaveSystem.RunningSequence.Wave == StaticData.Instance.LevelMaxWave)
        {
            Sound.Instance.PlayBg("lastwave");
        }
        else
        {
            switch (m_WaveSystem.RunningSequence.EnemyAttribute[0].EnemyType)
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
                default:
                    Sound.Instance.PlayBg("lastwave");
                    break;
            }
        }
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
