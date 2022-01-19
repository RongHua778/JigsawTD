using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMarkTrap : TrapContent
{
    //接下来两个格内敌人所受伤害增加50%
    //public override void OnContentPreCheck(int index, List<BasicTile> path)
    //{
    //    base.OnContentPreCheck(index,path);
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if ((index + i) < path.Count)
    //        {
    //            path[index + i].TileDamageIntensify += 0.5f * m_GameTile.TrapIntensify;
    //        }
    //    }
    //}

    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy, content);
        BuffInfo buff = new BuffInfo(EnemyBuffName.TileBaseDamageIntensify, 0.35f * TrapIntensify * enemy.DamageStrategy.TrapIntensify, 3);
        enemy.Buffable.AddBuff(buff);
        enemy.DamageStrategy.TrapIntensify = 1;
    }


}
