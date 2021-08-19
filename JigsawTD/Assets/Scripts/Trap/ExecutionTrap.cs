using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionTrap : TrapContent
{

    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
        float realdamage;
        enemy.DamageStrategy.ApplyDamage((enemy.DamageStrategy.MaxHealth -enemy.DamageStrategy.CurrentHealth)*0.01f* enemy.TrapIntentify,
            out realdamage,true);
        DamageAnalysis += (int)realdamage;
    }
}
