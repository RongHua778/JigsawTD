using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArrowTrap : TrapContent
{

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        if (enemy.Direction == m_GameTile.TileDirection)
        {
            BuffInfo buff = new BuffInfo(EnemyBuffName.SlowDown, 1f * m_GameTile.TrapIntensify, 5f);
            enemy.Buffable.AddBuff(buff);
        }
        else
        {
            Debug.Log("���˷�ʽΪ��" + enemy.Direction + "���巽��Ϊ" + m_GameTile.TileDirection);
        }
    }
}
