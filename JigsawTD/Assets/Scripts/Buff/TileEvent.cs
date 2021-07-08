using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileEventType
{
    NormalEenemy, EliteEnemy, RandomEvent
}
public abstract class TileEvent
{
    public TileEventType EventType;
    public virtual string EventDes { get; }
    public virtual string EventChoice1Des { get; }
    public virtual string EventChoice2Des { get; }

    public virtual string EventChoice3Des { get; }
    public int TurnRemain;

    public virtual void BtnClick1()
    {

    }

    public virtual void BtnClick2()
    {

    }

    public virtual void TriggerReward()
    {

    }

    public virtual void TriggerEventEffect()
    {

    }
}

public class ExplosionEnemy : TileEvent
{
    public override string EventDes => "探测部队发现一小波带有冰冻核心的游离敌人，是否追击？";

    public override string EventChoice1Des => "全力追击：接下来5回合增加5个冰爆者；奖励：5回合后，获得300金币+1个万能元素";
    public override string EventChoice2Des => "迂回骚扰：接下来3回合增加3个冰爆者；奖励：3回合后，获得150金币";
    public override string EventChoice3Des => "无视它们";
    private int amount;
    public override void BtnClick1()
    {
        TurnRemain = 5;
        amount = 5;
        TriggerEventEffect();
    }

    public override void BtnClick2()
    {
        TurnRemain = 3;
        amount = 3;
        TriggerEventEffect();

    }

    public override void TriggerReward()
    {
        if (amount == 5)
        {
            GameManager.Instance.GainMoney(300);
            GameManager.Instance.GetPerfectElement(1);
        }
        else
        {
            GameManager.Instance.GainMoney(150);
        }
    }
    public override void TriggerEventEffect()
    {
        GameManager.Instance.AddSequence(EnemyType.Froster, amount);
    }
}
