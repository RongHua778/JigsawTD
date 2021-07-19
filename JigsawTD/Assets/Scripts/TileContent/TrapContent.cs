using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Trap;
    [HideInInspector]public TrapAttribute m_TrapAttribute;
    public bool needReset = false;//是否需要重置朝向

    int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    private List<BuffInfo> trapBuffs = new List<BuffInfo>();
    public float TrapIntensify = 1;

    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = "UnDropablePoint";
        StaticData.SetNodeWalkable(m_GameTile, true);
    }

    public override void CorretRotation()
    {
        base.CorretRotation();
        if (needReset)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        trapBuffs = m_TrapAttribute.BuffInfos;
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in trapBuffs)
        {
            enemy.Buffable.AddBuff(trap, TrapIntensify);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        if (target != null)
        {
            if (target.Enemy != null) 
            ((Enemy)target.Enemy).CurrentTrap = this;
        }
        else
        {
            Debug.LogWarning(collision.name + ":错误的碰撞触发");
        }

    }

}
