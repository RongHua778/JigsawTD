using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElementStrategy : BasicStrategy
{
    public override StrategyType strategyType => StrategyType.Element;
    public Element Element;
    public ElementStrategy(TurretAttribute attribute, int quality, Element element, TurretContent turret) : base(attribute, quality, turret)
    {
        this.Element = element;
    }



}
