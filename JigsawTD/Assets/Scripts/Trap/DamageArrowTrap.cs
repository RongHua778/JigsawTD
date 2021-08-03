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

    private void OnTriggerExit2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        if (target != null)
        {
            if (target.Enemy != null)
            ((Enemy)target.Enemy).DamageIntensify -= 0.5f;
        }
        else
        {
            Debug.LogWarning(collision.name + ":´íÎóµÄÅö×²´¥·¢");
        }

    }

    public override void ContentLanded()
    {
        base.ContentLanded();
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 1);
    }
}
