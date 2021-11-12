using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeeper : Boss
{
    public override string ExplosionEffect => "BossExplosionYellow";


    private int LifeCount;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset,float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
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
        StaticData.Instance.GainMoneyEffect((Vector2)model.position,Mathf.Min(30, Mathf.RoundToInt(GameRes.CurrentWave)));
    }

    protected override void OnDie()
    {
        base.OnDie();
        GainMoney();
    }

}
