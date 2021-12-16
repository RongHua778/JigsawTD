using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoubleAttack : ElementSkill
{
    //ËùÓÐ¹¥»÷ÌáÉýÐ§¹û·­±¶
    public override List<int> Elements => new List<int> { 0, 0, 0 };

    public override void StartTurn()
    {
        strategy.TurnFixRange += strategy.TotalElementCount / 3;
        strategy.Turret.GenerateRange();
    }

    public override void EndTurn()
    {
        strategy.Turret.GenerateRange();
    }
}

public class HitAttack : ElementSkill
{
    //Ã¿´Î¹¥»÷ÌáÉý1%¹¥»÷
    public override List<int> Elements => new List<int> { 1, 1, 0 };
    public override float KeyValue => 0.01f * strategy.GoldCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.GOLD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.GOLD;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnAttackIntensify += KeyValue;
    }
}

public class TimeAttack : ElementSkill
{
    //¹¥»÷Ã¿ÃëÌáÉý0.5%
    public override List<int> Elements => new List<int> { 2, 2, 0 };
    public override float KeyValue => 0.005f * strategy.GoldCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.GOLD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.GOLD;

    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnAttackIntensify += KeyValue * delta;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class LongAttack : ElementSkill
{
    //¹¥»÷²¨¶¯
    public override List<int> Elements => new List<int> { 3, 3, 0 };
    public override float KeyValue => 0.05f * strategy.GoldCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.GOLD].Colorized((KeyValue*100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.GOLD;
    float intensifyValue;
    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnAttackIntensify -= intensifyValue;
        float distance = bullet.GetTargetDistance();
        if (distance > 3)
        {
            intensifyValue = bullet.GetTargetDistance() * KeyValue;
            strategy.TurnAttackIntensify += intensifyValue;
            bullet.Damage += strategy.BaseAttack * intensifyValue;
        }
        else
        {
            intensifyValue = 0;
        }
    }

    public override void EndTurn()
    {
        intensifyValue = 0;
    }

    //float targetValue;
    //float currentValue;
    //float counter = 2;
    //public override void StartTurn()
    //{
    //    base.StartTurn();
    //    Duration += 999;
    //}
    //public override void Tick(float delta)
    //{
    //    base.Tick(delta);
    //    counter += delta;
    //    if (counter > 2f)
    //    {
    //        counter = 0;
    //        targetValue = Random.Range(0.25f, 2.5f);
    //        DOTween.To(() => currentValue, x => currentValue = x, targetValue, 2);
    //    }
    //    strategy.AttackAdjust = currentValue;
    //}

    //public override void EndTurn()
    //{
    //    base.EndTurn();
    //    Duration = 0;
    //    strategy.AttackAdjust = 1;
    //    currentValue = 1;
    //    counter = 2;
    //}
}
public class StartAttack : ElementSkill
{
    //¿ª¾Ö¹¥»÷
    public override List<int> Elements => new List<int> { 4, 4, 0 };
    public override float KeyValue => 0.25f * strategy.GoldCount;
    public override string DisplayValue => StaticData.ElementDIC[ElementType.GOLD].Colorized((KeyValue * 100).ToString() + "%");
    public override ElementType IntensifyElement => ElementType.GOLD;
    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = KeyValue;
        strategy.TurnAttackIntensify += intensify;
    }

    public override void TickEnd()
    {
        base.TickEnd();
        strategy.TurnAttackIntensify -= intensify;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
    }
}








