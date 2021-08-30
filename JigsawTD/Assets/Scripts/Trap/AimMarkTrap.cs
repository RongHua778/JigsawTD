using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimMarkTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        float damage = enemy.DamageStrategy.MaxHealth * (1 - enemy.DamageStrategy.CurrentHealth / enemy.DamageStrategy.MaxHealth) * 0.1f;
        float damageReturn;
        enemy.DamageStrategy.ApplyDamage(damage, out damageReturn, true);
        DamageAnalysis += (int)damageReturn;
        //enemy.DamageIntensify += 0.1f * TrapIntensify * enemy.TrapIntentify;
        //enemy.TrapIntentify = 1;
    }
}
