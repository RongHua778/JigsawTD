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

    float trapIntensify = 1;
    public float TrapIntensify { get => trapIntensify; set => trapIntensify = value; }

    //美术设置
    private bool isReveal = false;
    public bool IsReveal { get => isReveal; set => isReveal = value; }

    [HideInInspector] public bool Important;
    private SpriteRenderer trapGFX;
    private Sprite initSprite;
    private Sprite unrevealSprite;


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

        GameManager.Instance.CheckDetectSkill();//任何一个塔放下来，都要所有防御塔检测一次侦测效果
    }



    public override void CorretRotation()
    {
        base.CorretRotation();
        if (needReset)
        {
            transform.rotation = Quaternion.identity;
        }
    }
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy);
        enemy.PassedTraps.Add(content == null ? this : (TrapContent)content);//复制闪烁陷阱
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


    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
        m_GameTile.IsLanded = false;//设为不可放置
    }

    public void RevealTrap()//揭示陷阱
    {
        if (!IsReveal)
        {
            if (!needReset)
                (m_GameTile).SetRandomRotation();
            trapGFX.sprite = initSprite;
            IsReveal = true;
        }
    }

}
