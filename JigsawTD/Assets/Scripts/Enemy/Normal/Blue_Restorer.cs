using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue_Restorer : Enemy
{
    public override EnemyType EnemyType => EnemyType.Restorer;

    float damagedCounter;
    public override bool GameUpdate()
    {
        damagedCounter += Time.deltaTime;
        if (damagedCounter > 1f)
        {
            CurrentHealth += MaxHealth * 0.05f * Time.deltaTime;
        }
        return base.GameUpdate();
    }

    public override void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        base.ApplyDamage(amount, out realDamage, isCritical);
        damagedCounter = 0;
    }

}
