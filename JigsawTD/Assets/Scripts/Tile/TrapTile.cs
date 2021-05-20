using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile
{
    public TrapAttribute m_TrapAttribute = default;

    public int TrapLevel;
    public override BasicTileType BasicTileType => BasicTileType.Trap;

    public override void OnTilePass(Enemy enemy)
    {
        base.OnTilePass(enemy);
        if (m_TrapAttribute.LevelInfos[TrapLevel].BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in m_TrapAttribute.LevelInfos[TrapLevel].BuffInfos)
        {
            enemy.Buffable.AddBuff(trap);
        }
    }

}
