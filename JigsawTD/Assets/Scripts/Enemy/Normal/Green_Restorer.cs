using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green_Restorer : Enemy
{
    protected override void SetStrategy()
    {
        DamageStrategy = new RestorerStrategy(this);
    }
    public override bool GameUpdate()
    {
        ((RestorerStrategy)DamageStrategy).damagedCounter += Time.deltaTime;
        if (((RestorerStrategy)DamageStrategy).damagedCounter > 1f)
        {
            DamageStrategy.CurrentHealth += DamageStrategy.MaxHealth * 0.05f * Time.deltaTime;
        }
        return base.GameUpdate();
    }


}
