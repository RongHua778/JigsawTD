using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ElementTurret : TurretContent
{
    public override GameTileContentType ContentType => GameTileContentType.ElementTurret;
    //ËşµÄÔªËØÊôĞÔ
    //private Element element;
    public abstract Element Element { get; }

    public override void ContentLanded()
    {
        base.ContentLanded();
        GameManager.Instance.elementTurrets.Add(this);
    }


    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        GameManager.Instance.elementTurrets.Remove(this);
        Quality = 1;
    }
}
