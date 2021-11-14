using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooporative : CompositeTurret
{
    public override void ContentLanded()
    {
        base.ContentLanded();
        GameRes.TotalCooporative++;
    }
}
