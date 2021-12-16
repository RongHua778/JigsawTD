public enum ElementType
{
    //½ðÄ¾Ë®»ðÍÁ£¡
    GOLD, WOOD, WATER, FIRE, DUST, None
}

public abstract class Element
{
    public abstract string GetIntensifyText(int amount);
    public abstract string GetSkillText { get; }
    public abstract string GetElementName { get; }
    public abstract string GetExtraInfo { get; }
    public abstract string ElementColor { get; }
    public string Colorized(string text)
    {
        return "<color=" + ElementColor + ">" + text + "</color>";
    }


    public abstract void GetComIntensify(StrategyBase strategy, bool add = true);
}
public class None : Element
{
    public override string GetSkillText => throw new System.NotImplementedException();

    public override string GetElementName => throw new System.NotImplementedException();
    public override string GetExtraInfo => "";

    public override string ElementColor => throw new System.NotImplementedException();

    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        throw new System.NotImplementedException();
    }

    public override string GetIntensifyText(int amount)
    {
        throw new System.NotImplementedException();
    }
}
public class Gold : Element
{
    public override string GetIntensifyText(int amount)
    {
        string txt;
        txt = "+" + amount * StaticData.Instance.GoldAttackIntensify * 100 + GameMultiLang.GetTraduction("ATTACKUP");
        return txt;
    }
    public override string GetExtraInfo => string.Format(GameMultiLang.GetTraduction("FOLLOW"), 0);
    public override string GetSkillText => "\n" + (StaticData.Instance.GoldAttackIntensify + GameRes.TempGoldIntensify) * 100 + GameMultiLang.GetTraduction("ATTACKUP");

    public override string GetElementName => "A";

    public override string ElementColor => "#FFE766";

    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        //strategy.ElementAttackIntensify += add ? StaticData.Instance.GoldAttackIntensify : -StaticData.Instance.GoldAttackIntensify;
    }

}
public class Wood : Element
{
    public override string GetIntensifyText(int amount)
    {
        string txt;
        txt = "+" + amount * StaticData.Instance.WoodFirerateIntensify * 100 + GameMultiLang.GetTraduction("SPEEDUP");
        return txt;
    }
    public override string ElementColor => "#62C751";
    public override string GetExtraInfo => string.Format(GameMultiLang.GetTraduction("FOLLOW"), 1);
    public override string GetSkillText => "\n" + (StaticData.Instance.WoodFirerateIntensify + GameRes.TempWoodIntensify) * 100 + GameMultiLang.GetTraduction("SPEEDUP");
    public override string GetElementName => "B";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        //strategy.ElementFirerateIntensify += add ? StaticData.Instance.WoodFirerateIntensify : -StaticData.Instance.WoodFirerateIntensify;
    }

}
public class Water : Element
{
    public override string GetIntensifyText(int amount)
    {
        string txt;
        txt = "+" + amount * StaticData.Instance.WaterSlowIntensify + GameMultiLang.GetTraduction("SLOWUP");
        return txt;
    }
    public override string ElementColor => "#00B7FF";
    public override string GetExtraInfo => string.Format(GameMultiLang.GetTraduction("FOLLOW"), 2);
    public override string GetSkillText => "\n" + (StaticData.Instance.WaterSlowIntensify + GameRes.TempWaterIntensify) + GameMultiLang.GetTraduction("SLOWUP");
    public override string GetElementName => "C";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        //strategy.ElementSlowIntensify += add ? StaticData.Instance.WaterSlowIntensify : -StaticData.Instance.WaterSlowIntensify;
    }
}
public class Fire : Element
{
    public override string GetIntensifyText(int amount)
    {
        string txt;
        txt = "+" + amount * StaticData.Instance.FireCritIntensify * 100 + GameMultiLang.GetTraduction("CRITICALUP");
        return txt;
    }
    public override string ElementColor => "#F7173A";

    public override string GetExtraInfo => string.Format(GameMultiLang.GetTraduction("FOLLOW"), 3);
    public override string GetSkillText => "\n" + (StaticData.Instance.FireCritIntensify + GameRes.TempFireIntensify) * 100 + GameMultiLang.GetTraduction("CRITICALUP");
    public override string GetElementName => "D";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        //strategy.ElementCritIntensify += add ? StaticData.Instance.FireCritIntensify : -StaticData.Instance.FireCritIntensify;
    }
}
public class Dust : Element
{
    public override string GetIntensifyText(int amount)
    {
        string txt;
        txt = "+" + amount * StaticData.Instance.DustSplashIntensify + GameMultiLang.GetTraduction("SPUTTERINGUP");
        return txt;
    }
    public override string ElementColor => "#E84BA3";
    public override string GetExtraInfo =>string.Format(GameMultiLang.GetTraduction("FOLLOW"),4);
    public override string GetSkillText => "\n" + (StaticData.Instance.DustSplashIntensify + GameRes.TempDustIntensify) + GameMultiLang.GetTraduction("SPUTTERINGUP");
    public override string GetElementName => "E";
    public override void GetComIntensify(StrategyBase strategy, bool add = true)
    {
        //strategy.ElementSplashIntensify += add ? StaticData.Instance.DustSplashIntensify : -StaticData.Instance.DustSplashIntensify;
    }
}



