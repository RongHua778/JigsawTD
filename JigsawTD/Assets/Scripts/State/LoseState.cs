using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : State
{
    public LoseState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.LoseState;
}
