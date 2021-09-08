using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTrap : TrapContent
{
    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        float slowRate = enemy.PassedTraps.Count * 0.5f * m_GameTile.TrapIntensify;
        BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, slowRate, 2f);
        enemy.Buffable.AddBuff(info);
    }

}