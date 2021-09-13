using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointContent : TrapContent
{
    public override GameTileContentType ContentType => GameTileContentType.SpawnPoint;

    protected override void Awake()
    {
        IsReveal = true;
        Important = true;
    }

    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {

    }
}
