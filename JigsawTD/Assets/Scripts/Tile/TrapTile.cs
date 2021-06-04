using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTile : GameTile
{
    public TrapAttribute m_TrapAttribute = default;

    int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    public override void TileLanded()
    {
        base.TileLanded();
        SetGroundTile();
    }




    public override void OnTilePass(Enemy enemy)
    {
        base.OnTilePass(enemy);
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in m_TrapAttribute.BuffInfos)
        {
            enemy.Buffable.AddBuff(trap);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        DamageAnalysis = 0;
    }
}
