using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile
{
    public TrapAttribute m_TrapAttribute = default;

    public override BasicTileType BasicTileType => BasicTileType.Trap;

    public override void OnTilePass(Enemy enemy)
    {
        base.OnTilePass(enemy);
        if (m_TrapAttribute.TrapInfos.Count <= 0)
            return;
        foreach (TrapInfo trap in m_TrapAttribute.TrapInfos)
        {
            enemy.Buffable.AddBuff(trap);
        }
    }

}
