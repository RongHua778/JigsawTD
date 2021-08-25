using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Trap;
    private TrapAttribute trapAttribute;
    public TrapAttribute TrapAttribute { get => trapAttribute; set => trapAttribute = value; }

    public bool needReset = false;//是否需要重置朝向

    int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    public float TrapIntensify = 1;

    //美术设置
    private bool isReveal = false;
    public bool IsReveal { get => isReveal; set => isReveal = value; }

    [HideInInspector] public bool Important;
    private SpriteRenderer trapGFX;
    private Sprite initSprite;
    private Sprite unrevealSprite;

    //public float trapIntensify2=1;
    //public bool passingOnce = false;
    //public bool relocatable = false;

    protected virtual void Awake()
    {
        trapGFX = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        initSprite = trapGFX.sprite;
        unrevealSprite = StaticData.Instance.UnrevealTrap;
        trapGFX.sprite = unrevealSprite;
    }
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
        enemy.LastTrap = this;
        enemy.PassedTraps.Add(this);
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

        //if (passingOnce)
        //{
        //    PassOnce(enemy);
        //}
        //else
        //{
        //    PassManyTimes(enemy);
        //    OnLeaveManyTimes(enemy);
        //}

    }

    //public virtual void OnPassOnce(Enemy enemy)
    //{
    //}

    //private void PassOnce(Enemy enemy)
    //{
    //    //int contained = 0;
    //    List<TrapContent> m = enemy.PassedTraps;
    //    //for (int i = 0; i < m.Count; i++)
    //    //{
    //    //    if (m[i] == this)
    //    //    {
    //    //        contained++; ;
    //    //    }
    //    //}
    //    if (!m.Contains(this))
    //    {
    //        m.Add(this);
    //        OnPassOnce(enemy);
    //        OnLeaveOnce(enemy);
    //    }
    //}

    //public void LeaveOnce(Enemy enemy)
    //{
    //    //int contained = 0;
    //    //List<TrapContent> m = enemy.PassedTraps;
    //    //for (int i = 0; i < m.Count; i++)
    //    //{
    //    //    if (m[i] == this)
    //    //    {
    //    //        contained++; ;
    //    //    }
    //    //}
    //}
    //public virtual void OnLeaveOnce(Enemy enemy)
    //{
    //    enemy.TrapIntentify = 1f;
    //}

    //public virtual void OnLeaveManyTimes(Enemy enemy)
    //{
    //    enemy.TrapIntentify = 1f;
    //}

    //public virtual void PassManyTimes(Enemy enemy)
    //{
    //    enemy.PassedTraps.Add(this);
    //}

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
        OnContentPass((Enemy)target.Enemy);
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
        m_GameTile.IsLanded = false;//设为不可放置
    }

    public void RevealTrap()
    {
        if (!IsReveal)
        {
            if (!needReset)
                ((GameTile)m_GameTile).SetRandomRotation();
            trapGFX.sprite = initSprite;
            IsReveal = true;
        }
    }
}
