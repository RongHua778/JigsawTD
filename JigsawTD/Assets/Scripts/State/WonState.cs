using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonState : State
{
    public WonState(GameManager gameManager) : base(gameManager)
    {
    }

    public override StateName StateName => StateName.WonState;
}
