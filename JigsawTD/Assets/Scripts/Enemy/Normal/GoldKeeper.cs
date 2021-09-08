using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeeper : Enemy
{
    public override EnemyType EnemyType => EnemyType.GoldKeeper;
    private int LifeCount;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify, path);
        LifeCount = 1;
    }
    protected override void OnEnemyUpdate()
    {
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 1 - 0.05f * LifeCount)
        {
            GainMoney();
        }
    }

    private void GainMoney()
    {
        LifeCount++;
        StaticData.Instance.GainMoneyEffect((Vector2)model.position, Mathf.RoundToInt(GameRes.CurrentWave * 1.5f));
    }

    protected override void OnDie()
    {
        base.OnDie();
        GainMoney();
    }

}
