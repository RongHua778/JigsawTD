using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ElementSkillInfo
{
    public List<int> Elements;
}
public abstract class ElementSkill : TurretSkill
{
    public virtual List<int> Elements { get; set; }

    public override void Build()
    {
        base.Build();
        strategy.GetComIntensify(Elements);
    }
}




