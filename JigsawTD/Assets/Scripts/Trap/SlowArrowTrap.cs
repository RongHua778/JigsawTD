using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArrowTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        if (enemy.Direction == ((GameTile)m_GameTile).TileDirection)
        {
            BuffInfo buff = new BuffInfo(EnemyBuffName.SlowDown, 1f * TrapIntensify * enemy.TrapIntentify, 5f);
            enemy.Buffable.AddBuff(buff);
        }
        else
        {
            Debug.Log("敌人方式为：" + enemy.Direction + "陷阱方向为" + ((GameTile)m_GameTile).TileDirection);
        }
        enemy.TrapIntentify = 0;
    }
}
