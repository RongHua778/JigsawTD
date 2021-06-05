using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Trap;
    public TrapAttribute m_TrapAttribute;

    int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in m_TrapAttribute.BuffInfos)
        {
            enemy.Buffable.AddBuff(trap);
        }
    }

    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        if (value)
        {
            GameManager.Instance.ShowTrapTips(this);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        DamageAnalysis = 0;
    }

}
