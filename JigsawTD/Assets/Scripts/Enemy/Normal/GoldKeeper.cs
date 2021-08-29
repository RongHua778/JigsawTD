using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldKeeper : Enemy
{
    public override EnemyType EnemyType => EnemyType.GoldKeeper;
    private int LifeCount;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        LifeCount = 1;
    }
    protected override void OnEnemyUpdate()
    {
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 1 - 0.05f * LifeCount)
        {
            LifeCount++;
            GainMoney();
        }
    }

    private void GainMoney()
    {
        GameManager.Instance.GainWaveBaseMoney((Vector2)model.position + Vector2.up * 0.2f);
        Sound.Instance.PlayEffect("Sound_GainCoin");
    }

    protected override void OnDie()
    {
        base.OnDie();
        GainMoney();
    }

}
