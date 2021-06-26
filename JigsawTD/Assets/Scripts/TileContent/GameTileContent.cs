using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, ElementTurret, CompositeTurret, Trap, Ground
}
public abstract class GameTileContent : ReusableObject
{

    public virtual bool IsWalkable { get => true; }
    public virtual GameTileContentType ContentType { get; }

    private TileBase m_gameTile;
    public TileBase m_GameTile { get => m_gameTile; set => m_gameTile = value; }

    public virtual void ContentLanded()//��content���ڵ���ʱ����
    {
        m_GameTile.tag = "UnDropablePoint";//EMPTY���д�˷���
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

    public virtual void OnContentPass(Enemy enemy)
    {

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        //m_GameTile = null;
    }
}
