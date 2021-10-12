public enum ElementType
{
    //½ðÄ¾Ë®»ðÍÁ£¡
    GOLD, WOOD, WATER, FIRE, DUST, None
}

public abstract class Element
{
    public abstract string GetIntensifyText { get; }
    public abstract string GetSkillText { get; }
    public abstract string GetElementName { get; }

    public abstract void GetComIntensify(StrategyBase strategy, bool add = true);
}

public class Gold : Element
{
    public override string GetIntensifyText => StaticData.Instance.GoldAttackIntensify * 100 + GameMultiLang.GetTraduction("ATTACKUP");

    public override string GetSkillText => "\n" + (StaticData.Instance.GoldAttackIntensify + GameRes.TempGoldIntensify) * 100 + GameMultiLang.GetTraduction("ATTACKUP");

    public override string GetElementName => "A";

    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        strategy.ComAttackIntensify += add ? StaticData.Instance.GoldAttackIntensify : -StaticData.Instance.GoldAttackIntensify;
    }
}
public class Wood : Element
{
    public override string GetIntensifyText => StaticData.Instance.WoodSpeedIntensify * 100 + GameMultiLang.GetTraduction("SPEEDUP");
    public override string GetSkillText => "\n" + (StaticData.Instance.WoodSpeedIntensify + GameRes.TempWoodIntensify) * 100 + GameMultiLang.GetTraduction("SPEEDUP");
    public override string GetElementName => "B";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        strategy.ComSpeedIntensify += add ? StaticData.Instance.WoodSpeedIntensify : -StaticData.Instance.WoodSpeedIntensify;
    }
}
public class Water : Element
{
    public override string GetIntensifyText => StaticData.Instance.WaterSlowIntensify + GameMultiLang.GetTraduction("SLOWUP");
    public override string GetSkillText => "\n" + (StaticData.Instance.WaterSlowIntensify + GameRes.TempWaterIntensify) + GameMultiLang.GetTraduction("SLOWUP");
    public override string GetElementName => "C";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        strategy.ComSlowIntensify += add ? StaticData.Instance.WaterSlowIntensify : -StaticData.Instance.WaterSlowIntensify;
    }
}
public class Fire : Element
{
    public override string GetIntensifyText => StaticData.Instance.FireCriticalIntensify * 100 + GameMultiLang.GetTraduction("CRITICALUP");
    public override string GetSkillText => "\n" + (StaticData.Instance.FireCriticalIntensify + GameRes.TempFireIntensify) * 100 + GameMultiLang.GetTraduction("CRITICALUP");
    public override string GetElementName => "D";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        strategy.ComCriticalIntensify += add ? StaticData.Instance.FireCriticalIntensify : -StaticData.Instance.FireCriticalIntensify;
    }
}
public class Dust : Element
{
    public override string GetIntensifyText => StaticData.Instance.DustSputteringIntensify + GameMultiLang.GetTraduction("SPUTTERINGUP");
    public override string GetSkillText => "\n" + (StaticData.Instance.DustSputteringIntensify + GameRes.TempDustIntensify) + GameMultiLang.GetTraduction("SPUTTERINGUP");
    public override string GetElementName => "E";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        strategy.ComSputteringRangeIntensify += add ? StaticData.Instance.DustSputteringIntensify : -StaticData.Instance.DustSputteringIntensify;
    }
}



