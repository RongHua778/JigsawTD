using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, Turret, Trap
}
public abstract class GameTileContent : ReusableObject
{
    public virtual GameTileContentType ContentType { get; }

    private GameTile m_gameTile;
    public GameTile m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual void ContentLanded()//该content放在地上时触发
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
        SetGroundTile(false);
    }

    public virtual void OnContentSelected(bool value)
    {
        //showtips
    }

    protected virtual void ContentLandedCheck(Collider2D col)//根据下方已有坚固格的类型决定自己的行为
    {
        m_GameTile.tag = "UnDropablePoint";
        
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile.gameObject);
        }
    }
    protected void SetGroundTile(bool value)
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));//修改groundtile层
        if (col != null)
        {
            GroundTile groundTile = col.GetComponent<GroundTile>();
            groundTile.IsLanded = value;
        }
    }

    public virtual void CorretRotation()
    {
        
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        SetGroundTile(true);
        m_GameTile = null;


    }
}
