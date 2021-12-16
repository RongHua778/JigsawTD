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
        foreach (var element in Elements)
        {
            switch (element)
            {
                case 0:
                    strategy.BaseGoldCount += 1;
                    break;
                case 1:
                    strategy.BaseWoodCount += 1;
                    break;
                case 2:
                    strategy.BaseWaterCount += 1;
                    break;
                case 3:
                    strategy.BaseFireCount += 1;
                    break;
                case 4:
                    strategy.BaseDustCount += 1;
                    break;
            }
        }
        //strategy.GetComIntensify(Elements);
    }
}




