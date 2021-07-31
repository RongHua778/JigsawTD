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
    public override void OnContentPass(Enemy enemy)
    {
        base.OnContentPass(enemy);
        //以下是新加的内容
        for (int i = 0; i < enemy.PassedTraps.Count; i++)
        {
            enemy.PassedTraps[i].trapIntensify2 = 1f;
        }
        if (enemy.PassedTraps.Count > 0)
        {
            TrapContent m = enemy.PassedTraps[enemy.PassedTraps.Count - 1];
            //受到上一个陷阱的强化
            if (enemy.PassedTraps.Count > 1)
            {
                for (int i = 0; i < enemy.PassedTraps.Count - 1; i++)
                {
                    enemy.PassedTraps[i].NextTrap(enemy.PassedTraps[i + 1]);
                }
                enemy.PassedTraps[enemy.PassedTraps.Count - 1].NextTrap(this);
            }
        }
        OnPassOnce(enemy);
        PassManyTimes(enemy);
        //******
        trapBuffs = m_TrapAttribute.BuffInfos;
        if (m_TrapAttribute.BuffInfos.Count <= 0)
            return;
        foreach (BuffInfo trap in trapBuffs)
        {
            enemy.Buffable.AddBuff(trap, TrapIntensify);
        }

    }

    private void OnPassOnce(Enemy enemy)
    {
        bool contained = false;
        List<TrapContent> m = enemy.PassedTraps;
        if (m.Count == 0)
        {
            m.Add(this);
            PassOnce(enemy);
        }
        else
        {
            for (int i = 0; i < m.Count; i++)
            {
                if (m[i] == this)
                {
                    contained = true;
                }
            }
            if (!contained)
            {
                m.Add(this);
                PassOnce(enemy);
            }
        }
    }

    public virtual void PassOnce(Enemy enemy)
    {

    }
    public virtual void PassManyTimes(Enemy enemy)
    {
        enemy.PassedTraps.Add(this);
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

    public virtual void NextTrap(TrapContent nextTrap)
    {

    }
}
