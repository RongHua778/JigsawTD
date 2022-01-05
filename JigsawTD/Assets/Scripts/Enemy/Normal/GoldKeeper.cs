using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeeper : Boss
{

    private int LifeCount;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        LifeCount = 1;
    }
    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 1 - 0.05f * LifeCount)
        {
            GainMoney();
        }
    }

    private void GainMoney()
    {
        LifeCount++;
        StaticData.Instance.GainMoneyEffect(model.position, Mathf.Min(40, Mathf.RoundToInt(GameRes.CurrentWave * 1.5f)));
    }

    protected override void OnDie()
    {
        base.OnDie();
        GainMoney();
    }

}
