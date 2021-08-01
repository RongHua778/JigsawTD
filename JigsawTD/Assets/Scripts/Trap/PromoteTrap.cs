using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{

    protected override void NextTrap(TrapContent nextTrap)
    {
        base.NextTrap(nextTrap);
        nextTrap.trapIntensify2 += trapIntensify2;
    }


}