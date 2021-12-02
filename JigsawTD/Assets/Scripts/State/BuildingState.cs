using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : BattleOperationState
{
    public override StateName StateName => StateName.BuildingState;
    private BoardSystem m_Board;
    public BuildingState(GameManager gameManager, BoardSystem gameBoard) : base(gameManager)
    {
        m_Board = gameBoard;
    }

    public override IEnumerator EnterState()
    {
        yield return new WaitForSeconds(0.1f);
        Sound.Instance.PlayBg("Music_Preparing");


        yield break;
    }

    public override IEnumerator ExitState(BattleOperationState newState)
    {
        yield return new WaitForSeconds(0.1f);
        gameManager.StartCoroutine(newState.EnterState());
        yield break;
    }
}
