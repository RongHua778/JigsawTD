using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArrowTrap : TrapContent
{

    public override void PassManyTimes(Enemy enemy)
    {
        base.PassManyTimes(enemy);
        enemy.DamageIntensify += 0.5f;
    }

    protected override void OnExitTrap(Enemy enemy)
    {
        enemy.DamageIntensify -= 0.5f;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
    }
}
