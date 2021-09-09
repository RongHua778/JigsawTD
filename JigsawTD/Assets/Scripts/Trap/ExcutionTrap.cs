using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExcutionTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy);
        float percentage = enemy.PointIndex * 0.002f * m_GameTile.TrapIntensify;
        float damage = enemy.DamageStrategy.MaxHealth * percentage;
        float damageReturn;
        enemy.DamageStrategy.ApplyDamage(damage, out damageReturn, true);
        DamageAnalysis += (int)damageReturn;
    }
}
