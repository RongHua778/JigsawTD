using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : State
{

    public BuildingState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.BuildingState;

    public override IEnumerator EnterState()
    {
        yield return new WaitForSeconds(0.5f);
        LevelUIManager.Instance.Preparing();
        gameManager._bluePrintShop.NextRefreshTrun--;
        //重置所有防御塔的回合临时加成
        foreach(var turret in gameManager.turrets.behaviors)
        {
            ((Turret)turret).ClearTurnIntensify();
        }
        yield break;
    }

    public override IEnumerator ExitState(State newState)
    {
        gameManager.Board.GetPathTiles();
        yield return new WaitForSeconds(1f);
        gameManager.EnterNewState(newState);
        yield break;
    }
}
