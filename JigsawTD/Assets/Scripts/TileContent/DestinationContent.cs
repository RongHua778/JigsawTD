using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationContent : TrapContent
{
    public override GameTileContentType ContentType => GameTileContentType.Destination;
    protected override void Awake()
    {
        IsReveal = true;
        Important = true;
    }

    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {

    }

}


