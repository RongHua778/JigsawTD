using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundTile : TileBase
{
    [SerializeField] SpriteRenderer eventSprite = default;
    private TileEvent m_TileEvent;
    public TileEvent TileEvent { get => m_TileEvent; set => m_TileEvent = value; }
    [SerializeField] bool eventTriggered = true;
    public GameTile TileAbrove;
    public override bool IsLanded
    {
        get => base.IsLanded;
        set
        {
            base.IsLanded = value;
            gameObject.layer = value ? LayerMask.NameToLayer(StaticData.GroundTileMask) : LayerMask.NameToLayer(StaticData.TempGroundMask);
        }
    }

    public void SetEvent(TileEvent tileEvent)
    {
        this.TileEvent = tileEvent;
        eventSprite.gameObject.SetActive(true);
        eventTriggered = false;
        //eventSprite.sprite = tileEvent.EventSprite;

    }

    public void OnEventTrigger()
    {
        //if (!eventTriggered && TileEvent != null)
        //{
        //    eventTriggered = true;
        //    //触发事件
        //    GameManager.Instance.ShowEvent(TileEvent);
        //}

        if (!eventTriggered)
        {
            eventTriggered = true;
            //触发事件
            GameManager.Instance.ShowEvent(new ExplosionEnemy());
        }
    }

    public override void OnSpawn()
    {

    }

    public override void OnUnSpawn()
    {

    }
}
