using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.CompositeTurret;


    public Blueprint CompositeBluePrint;

    public override bool GameUpdate()
    {
        foreach (var skill in Strategy.TurretSkills)
        {
            if (skill.Duration > 0)
                skill.Tick(Time.deltaTime);
        }
        return base.GameUpdate();
    }


    public override void ContentLanded()
    {
        GameManager.Instance.compositeTurrets.Add(this);
        base.ContentLanded();
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile.Content.ContentType == GameTileContentType.TurretBase)//放在加成地形上时，获得地形技能
            {
                TurretBaseContent content = tile.Content as TurretBaseContent;
                ((StrategyComposite)Strategy).AddTileSkill(content.m_TurretBaseAttribute.tileSkill);
            }
            ObjectPool.Instance.UnSpawn(tile);
        }
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        CompositeBluePrint = null;
        GameManager.Instance.compositeTurrets.Remove(this);

    }

}
