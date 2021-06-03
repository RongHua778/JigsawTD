using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, Turret, Trap
}
public abstract class GameTileContent : ReusableObject
{
    public abstract GameTileContentType ContentType { get; }

    private GameTile m_gameTile;
    public GameTile m_GameTile { get => m_gameTile; set => m_gameTile = value; }
    public virtual void OnContentLanded()//��content���ڵ���ʱ����
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);
        SetGroundTile(false);
    }

    public virtual void OnContentRelease()//��Content�뿪����ʱ����
    {
        SetGroundTile(true);
    }

    protected virtual void ContentLandedCheck(Collider2D col)//�����·����м�̸�����;����Լ�����Ϊ
    {

    }
    protected void SetGroundTile(bool value)
    {
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.GroundTileMask));//�޸�groundtile��
        if (col != null)
        {
            GroundTile groundTile = col.GetComponent<GroundTile>();
            groundTile.IsActive = value;
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        m_GameTile = null;
    }



}
