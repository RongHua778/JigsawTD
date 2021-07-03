using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyElement : StrategyBase
{
    public override StrategyType strategyType => StrategyType.Element;
    public Element Element;
    public StrategyElement(TurretAttribute attribute, int quality, Element element, TurretContent turret) : base(attribute, quality, turret)
    {
        this.Element = element;
    }
}
