using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        enemy.TrapIntentify += 1f;
    }

}