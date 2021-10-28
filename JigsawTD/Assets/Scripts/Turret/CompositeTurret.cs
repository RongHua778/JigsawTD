using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.CompositeTurret;
    public override bool GameUpdate()
    {
        if (GameManager.Instance.OperationState.StateName == StateName.WaveState)
        {
            foreach (var skill in Strategy.TurretSkills)
            {
                if (skill.Duration > 0)
                    skill.Tick(Time.deltaTime);
            }
        }
        return base.GameUpdate();
    }


    public override void ContentLanded()
    {
        GameManager.Instance.compositeTurrets.Add(this);
        base.ContentLanded();
        m_GameTile.tag = "OnlyCompositeTurret";
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            if (tile.Content.ContentType == GameTileContentType.CompositeTurret)
            {
                CompositeTurret turret = tile.Content as CompositeTurret;
                ElementSkill skill = (ElementSkill)this.Strategy.TurretSkills[1];
                turret.Strategy.AddElementSkill(skill);
                skill.OnEquip();
                ((BasicTile)turret.m_GameTile).EquipTurret();
                BoardSystem.PreviewEquipTile = null;
                ObjectPool.Instance.UnSpawn(m_GameTile);
            }
            else
            {
                ObjectPool.Instance.UnSpawn(tile);
            }
        }
        ReusableObject partical = ObjectPool.Instance.Spawn(StaticData.Instance.LandedEffect);
        partical.transform.position = transform.position + Vector3.up * 0.2f;
    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        GameManager.Instance.compositeTurrets.Remove(this);

    }

}
