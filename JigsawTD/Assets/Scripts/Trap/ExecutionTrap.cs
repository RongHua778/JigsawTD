using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionTrap : TrapContent
{
    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
        float realdamage;
        enemy.ApplyDamage((enemy.MaxHealth-enemy.CurrentHealth)*0.01f* trapIntensify2,
            out realdamage, true);
        DamageAnalysis += (int)realdamage;
    }
}
