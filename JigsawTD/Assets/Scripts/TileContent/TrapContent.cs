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

    protected float trapCounter;
    public float trapIntensify2=1;

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
    public override void OnContentPassOnce(Enemy enemy)
    {
        base.OnContentPassOnce(enemy);
        trapBuffs = m_TrapAttribute.BuffInfos;
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in trapBuffs)
        {
            enemy.Buffable.AddBuff(trap, TrapIntensify);
        }
    }

    public virtual void OnContentPassMoreThanOnce(Enemy enemy)
    {

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
            List<EnemyTrapManager> m= ((Enemy)target.Enemy).PassedTraps;
            if (m.Count > 0)
            {
                bool contained=false;
                for (int i = 0; i < m.Count; i++)
                {
                    if (m[i].trap == this)
                    {
                        contained = true;
                    }
                }
                if (!contained)
                {
                    ((Enemy)target.Enemy).PassedTraps.Add(new EnemyTrapManager(this, false));
                }
            }
            else
            {
                ((Enemy)target.Enemy).PassedTraps.Add(new EnemyTrapManager(this, false));
            }
            ((Enemy)target.Enemy).TriigerTrap();
        }
        else
        {
            Debug.LogWarning(collision.name + ":错误的碰撞触发");
        }

    }

    public virtual void OnGameUpdating(Enemy enemy)
    {
        
    }

    public virtual void NextTrap(TrapContent nextTrap)
    {

    }
}
