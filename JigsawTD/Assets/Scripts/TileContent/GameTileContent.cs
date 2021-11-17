using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, ElementTurret, CompositeTurret, Trap, TurretBase
}
public abstract class GameTileContent : ReusableObject
{
    protected ContentStruct m_ContentStruct;
    public virtual bool IsWalkable { get => true; }
    public virtual GameTileContentType ContentType { get; }

    private GameTile m_gameTile;
    public GameTile m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual void ContentLanded()//��content���ڵ���ʱ����
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));
        if (col != null)
        {
            col.GetComponent<GroundTile>().IsLanded = false;
        }
        SaveContent();
    }

    public virtual void OnContentSelected(bool value)
    {

    }

    protected virtual void ContentLandedCheck(Collider2D col)//�����·����м�̸�����;����Լ�����Ϊ
    {

    }

    public virtual void CorretRotation()
    {

    }

    public virtual void OnContentPass(Enemy enemy, GameTileContent content = null)
    {

    }

    protected virtual void SaveContent()//�����µ�ʱ�򣬽��뱣��LIST
    {
        m_ContentStruct = new ContentStruct();
        m_ContentStruct.ContentType = (int)ContentType;
        m_ContentStruct.posX = Mathf.RoundToInt( transform.position.x);
        m_ContentStruct.posY = Mathf.RoundToInt(transform.position.y);
        m_ContentStruct.Direction = (int)m_gameTile.TileDirection;
        LevelManager.Instance.SaveContents.Add(m_ContentStruct);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        LevelManager.Instance.SaveContents.Remove(m_ContentStruct);
    }

}
