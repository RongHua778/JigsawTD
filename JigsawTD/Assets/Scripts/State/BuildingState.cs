using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : State
{
    public BuildingState(GameManager gameManager) : base(gameManager)
    {
    }

    public override IEnumerator EnterState()
    {
        //Debug.Log("Eneter Building State");
        yield return new WaitForSeconds(2f);
        gameManager._levelUIManager.GetNewBuildings();
        yield break;
    }

    public override IEnumerator ExitState(State newState)
    {
        //Debug.Log("Exit Building State");
        gameManager.Board.GetPathTiles();
        yield return new WaitForSeconds(1f);
        gameManager.EnterNewState(newState);
        yield break;
    }
}
