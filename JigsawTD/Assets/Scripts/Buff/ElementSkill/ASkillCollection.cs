using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoubleAttack : ElementSkill
{
    //ËùÓÐ¹¥»÷ÌáÉýÐ§¹û·­±¶
    public override List<int> Elements => new List<int> { 0, 0, 0 };


    public override void Build()
    {
        base.Build();
        strategy.AllAttackIntensifyModify += 1;
    }
}

public class HitAttack : ElementSkill
{
    //Ã¿´Î¹¥»÷ÌáÉý3%¹¥»÷
    public override List<int> Elements => new List<int> { 0, 0, 1 };

    public override void Shoot(Bullet bullet = null)
    {
        strategy.TurnAttackIntensify += 0.03f * strategy.TimeModify;
    }
}

public class TimeAttack : ElementSkill
{
    //¹¥»÷Ã¿ÃëÌáÉý2%
    public override List<int> Elements => new List<int> { 0, 0, 2 };


    public override void StartTurn()
    {
        Duration += 999;
    }

    public override void Tick(float delta)
    {
        base.Tick(delta);
        strategy.TurnAttackIntensify += 0.02f * delta * strategy.TimeModify;
    }

    public override void EndTurn()
    {
        Duration = 0;
    }
}

public class FlutAttack : ElementSkill
{
    //¹¥»÷²¨¶¯
    public override List<int> Elements => new List<int> { 0, 0, 3 };


    float targetValue;
    float currentValue;
    float counter = 2;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 999;
    }
    public override void Tick(float delta)
    {
        base.Tick(delta);
        counter += delta;
        if (counter > 2f)
        {
            counter = 0;
            targetValue = Random.Range(0.25f, 2.5f);
            DOTween.To(() => currentValue, x => currentValue = x, targetValue, 2);
        }
        strategy.AttackAdjust = currentValue;
    }

    public override void EndTurn()
    {
        base.EndTurn();
        Duration = 0;
        strategy.AttackAdjust = 1;
        currentValue = 1;
        counter = 2;
    }
}
public class StartAttack : ElementSkill
{
    //¿ª¾Ö¹¥»÷
    public override List<int> Elements => new List<int> { 0, 0, 4 };

    float intensify = 0;
    public override void StartTurn()
    {
        base.StartTurn();
        Duration += 20;
        intensify = 1.5f * strategy.TimeModify;
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








