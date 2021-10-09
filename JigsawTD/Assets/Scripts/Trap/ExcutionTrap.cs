using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcutionTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy);
        float damage = (enemy.DamageStrategy.MaxHealth - enemy.DamageStrategy.CurrentHealth) * 0.2f * TrapIntensify * enemy.EnemyTrapIntensify;
        float damageReturn;
        enemy.DamageStrategy.ApplyDamage(damage, out damageReturn, true);
        DamageAnalysis += (int)damageReturn;
    }
}
