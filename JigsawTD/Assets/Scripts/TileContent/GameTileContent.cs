using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, ElementTurret, RefactorTurret, Trap, TurretBase
}
public abstract class GameTileContent : ReusableObject
{
    public ContentStruct m_ContentStruct;
    public virtual bool IsWalkable { get => true; }
    public virtual GameTileContentType ContentType { get; }

    private GameTile m_gameTile;
    public GameTile m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual void ContentLanded()//该content放在地上时触发
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = false;
        }
        LevelManager.Instance.GameContents.Add(this);
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

    public virtual void OnContentPass(Enemy enemy, GameTileContent content = null)
    {

    }

    public virtual void SaveContent(out ContentStruct contentSruct)//放置下的时候，进入保存LIST
    {
        m_ContentStruct = new ContentStruct();
        contentSruct = m_ContentStruct;
        m_ContentStruct.ContentType = (int)ContentType;
        //m_ContentStruct.posX = Mathf.RoundToInt( transform.position.x);
        //m_ContentStruct.posY = Mathf.RoundToInt(transform.position.y);
        m_ContentStruct.Pos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        m_ContentStruct.Direction = (int)m_gameTile.TileDirection;
        
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        LevelManager.Instance.GameContents.Remove(this);
    }

}
