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
    public override string EventDes => "̽�ⲿ�ӷ���һС�����б������ĵ�������ˣ��Ƿ�׷����";

    public override string EventChoice1Des => "ȫ��׷����������5�غ�����5�������ߣ�������5�غϺ󣬻��300���+1������Ԫ��";
    public override string EventChoice2Des => "�ػ�ɧ�ţ�������3�غ�����3�������ߣ�������3�غϺ󣬻��150���";
    public override string EventChoice3Des => "��������";
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
