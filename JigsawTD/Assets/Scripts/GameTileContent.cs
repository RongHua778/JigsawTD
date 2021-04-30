using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameTileContentType
{
    Empty, Destination, SpawnPoint, Turret, Rock
}
public class GameTileContent : ReusableObject
{
    [SerializeField]
    GameTileContentType type = default;
    public GameTileContentType Type => type;
    GameTileContentFactory originFactory;
    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            //Debug.Assert(originFactory == null, "Redefined orign factory!"+this.name);
            originFactory = value;
        }
    }

    public void Recycle()
    {
        originFactory.Reclaim(this);
    }

    public override void OnSpawn()
    {
        
    }

    public override void OnUnSpawn()
    {
        
    }
}
