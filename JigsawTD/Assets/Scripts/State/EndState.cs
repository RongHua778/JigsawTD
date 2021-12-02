using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : BattleOperationState
{
    public EndState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.LoseState;

    public override IEnumerator EnterState()
    {
        Sound.Instance.PlayBg("Music_Preparing");
        yield return null;
    }
}
