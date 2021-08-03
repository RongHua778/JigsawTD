using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{
    public override void OnLeaveOnce(Enemy enemy)
    {
        enemy.TrapIntentify += 1f;
    }

    public override void OnLeaveManyTimes(Enemy enemy)
    {
        enemy.TrapIntentify+=1f;
    }


}