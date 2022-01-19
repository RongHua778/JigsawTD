using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveState : BattleOperationState
{
    WaveSystem m_WaveSystem;
    BoardSystem m_BoardSystem;
    public WaveState(GameManager gameManager, WaveSystem waveSystem, BoardSystem boardSystem) : base(gameManager)
    {
        m_BoardSystem = boardSystem;
        m_WaveSystem = waveSystem;
    }

    public override StateName StateName => StateName.WaveState;

    public override IEnumerator EnterState()
    {
        gameManager.OperationState = this;
        //计算陷阱列表及效果
        m_BoardSystem.GetPathTiles();
        m_BoardSystem.TransparentPath(0.15f);
        foreach (var turret in GameManager.Instance.compositeTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnAnalysis();
            ((TurretContent)turret).Strategy.StartTurnSkills();
        }
        foreach (var turret in GameManager.Instance.elementTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnAnalysis();
        }
        GameRes.MaxPath = BoardSystem.shortestPath.Count;
        yield return new WaitForSeconds(0.5f);
        m_WaveSystem.RunningSpawn = true;
        //switch (m_WaveSystem.RunningSequence[0].EnemyType)
        //{
        //    case EnemyType.Soilder:
        //        Sound.Instance.PlayBg("soldier");
        //        break;
        //    case EnemyType.Runner:
        //        Sound.Instance.PlayBg("runner");
        //        break;
        //    case EnemyType.Restorer:
        //        Sound.Instance.PlayBg("restorer");
        //        break;
        //    case EnemyType.Tanker:
        //        Sound.Instance.PlayBg("tanker");
        //        break;
        //    case EnemyType.Borner:
        //        Sound.Instance.PlayBg("Borner");
        //        break;
        //    case EnemyType.Froster:
        //        Sound.Instance.PlayBg("Froster");
        //        break;
        //    case EnemyType.Healer:
        //        Sound.Instance.PlayBg("Healer");
        //        break;
        //    default:
        //        Sound.Instance.PlayBg("lastwave");
        //        break;
        //}
        switch (m_WaveSystem.RunningSequence[0].EnemyType)
        {
            case EnemyType.Soilder:
            case EnemyType.Runner:
            case EnemyType.Restorer:
            case EnemyType.Tanker:
            case EnemyType.Borner:
            case EnemyType.Froster:
            case EnemyType.Healer:
            case EnemyType.GoldKeeper:
                Sound.Instance.PlayBg("Music_Normal");
                break;
            default:
                Sound.Instance.PlayBg("Music_Boss");
                break;
        }
        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
       
        yield return new WaitForSeconds(0.5f);
        m_BoardSystem.TransparentPath(1f);
        //重置所有防御塔的回合临时加成
        foreach (var turret in GameManager.Instance.elementTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnIntensify();
        }
        foreach (var turret in GameManager.Instance.compositeTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnIntensify();
        }
        yield return new WaitForSeconds(0.1f);
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
