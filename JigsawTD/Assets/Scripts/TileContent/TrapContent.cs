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
    public bool passingOnce = false;
    public bool relocatable = false;
    public override void ContentLanded()
    {
        base.ContentLanded();
        m_GameTile.tag = "UnDropablePoint";
        StaticData.SetNodeWalkable(m_GameTile, true);
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
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
        //以下是新加的内容
        //for (int i = 0; i < enemy.PassedTraps.Count; i++)
        //{
        //    enemy.PassedTraps[i].trapIntensify2 = 1f;
        //}
        //trapIntensify2 = 1f;
        //if (enemy.PassedTraps.Count > 0)
        //{
        //    //TrapContent m = enemy.PassedTraps[enemy.PassedTraps.Count - 1];
        //    //受到上一个陷阱的强化
        //    for (int i = 0; i < enemy.PassedTraps.Count - 1; i++)
        //    {
        //        enemy.PassedTraps[i].NextTrap(enemy.PassedTraps[i + 1]);
        //    }
        //    enemy.PassedTraps[enemy.PassedTraps.Count - 1].NextTrap(this);
        //}

        if (passingOnce)
        {
            PassOnce(enemy);
        }
        else
        {
            PassManyTimes(enemy);
            OnLeaveManyTimes(enemy);
        }
        //******
        trapBuffs = m_TrapAttribute.BuffInfos;
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in trapBuffs)
        {
            enemy.Buffable.AddBuff(trap, TrapIntensify);
        }

    }

    public virtual void OnPassOnce(Enemy enemy)
    {
    }

    private void PassOnce(Enemy enemy)
    {
        //int contained = 0;
        List<TrapContent> m = enemy.PassedTraps;
        //for (int i = 0; i < m.Count; i++)
        //{
        //    if (m[i] == this)
        //    {
        //        contained++; ;
        //    }
        //}
        if (!m.Contains(this))
        {
            m.Add(this);
            OnPassOnce(enemy);
            OnLeaveOnce(enemy);
        }
    }

    public void LeaveOnce(Enemy enemy)
    {
        //int contained = 0;
        //List<TrapContent> m = enemy.PassedTraps;
        //for (int i = 0; i < m.Count; i++)
        //{
        //    if (m[i] == this)
        //    {
        //        contained++; ;
        //    }
        //}
    }
    public virtual void OnLeaveOnce(Enemy enemy)
    {
        enemy.TrapIntentify = 1f;
    }

    public virtual void OnLeaveManyTimes(Enemy enemy)
    {
        enemy.TrapIntentify = 1f;
    }

    public virtual void PassManyTimes(Enemy enemy)
    {
        enemy.PassedTraps.Add(this);
    }

    public override void OnContentSelected(bool value)
    {
        BoardSystem.selectedTrap = m_TrapAttribute.Name;
        BoardSystem.relocatable = relocatable;
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
            ((Enemy)target.Enemy).TriigerTrap();
            //List<EnemyTrapManager> m= ((Enemy)target.Enemy).PassedTraps;
            //if (m.Count > 0)
            //{
            //    bool contained=false;
            //    for (int i = 0; i < m.Count; i++)
            //    {
            //        if (m[i].trap == this)
            //        {
            //            contained = true;
            //        }
            //    }
            //    if (!contained)
            //    {
            //        ((Enemy)target.Enemy).PassedTraps.Add(new EnemyTrapManager(this, false));
            //    }
            //}
            //else
            //{
            //    ((Enemy)target.Enemy).PassedTraps.Add(new EnemyTrapManager(this, false));
            //}
        }
        else
        {
            Debug.LogWarning(collision.name + ":错误的碰撞触发");
        }

    }


    public virtual void OnGameUpdating(Enemy enemy)
    {
        
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
    }
}
