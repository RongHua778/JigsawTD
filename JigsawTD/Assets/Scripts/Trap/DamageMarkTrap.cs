using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMarkTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        BuffInfo buff = new BuffInfo(EnemyBuffName.DamageIntensify, 0.5f * TrapIntensify * enemy.TrapIntentify, 5f);
        enemy.Buffable.AddBuff(buff);
        enemy.TrapIntentify = 0;
    }


}
