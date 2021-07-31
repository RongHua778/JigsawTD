using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, ElementTurret, CompositeTurret, Trap, Ground,TurretBase
}
public abstract class GameTileContent : ReusableObject
{

    public virtual bool IsWalkable { get => true; }
    public virtual GameTileContentType ContentType { get; }

    private TileBase m_gameTile;
    public TileBase m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual void ContentLanded()//该content放在地上时触发
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = false;
        }
    }

    public virtual void OnContentSelected(bool value)
    {

    }

    protected virtual void ContentLandedCheck(Collider2D col)//根据下方已有坚固格的类型决定自己的行为
    {

    }

    public virtual void CorretRotation()
    {

    }

    public virtual void OnContentPass(Enemy enemy)
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        //m_GameTile = null;
    }
}
