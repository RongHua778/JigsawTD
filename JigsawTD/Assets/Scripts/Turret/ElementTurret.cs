using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTurret : TurretContent
{

    //ËþµÄÔªËØÊôÐÔ
    private Element element;
    public Element Element { get => element; set => element = value; }

    public override void OnSpawn()
    {
        base.OnSpawn();
        Element = m_TurretAttribute.element;

    }
    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Quality = 1;
        Element = Element.Gold;
    }
}
