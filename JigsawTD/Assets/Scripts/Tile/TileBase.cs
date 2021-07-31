using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TileBase : ReusableObject
{

    private GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set => content = value;
    }
    Vector2Int _offsetCoord;
    public Vector2Int OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    private bool isLanded = false;//ÊÇ·ñ´¦ÓÚ°æÍ¼×´Ì¬
    public virtual bool IsLanded { get => isLanded; set => isLanded = value; }

    public virtual void SetContent(GameTileContent content)
    {
        content.transform.SetParent(this.transform);
        content.transform.position = transform.position + Vector3.forward * 0.01f;
        content.m_GameTile = this;
        Content = content;
    }

    protected virtual void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameEvents.Instance.TileClick();
        }
    }


    protected virtual void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            GameEvents.Instance.TileUp(this);
        }
    }
}
