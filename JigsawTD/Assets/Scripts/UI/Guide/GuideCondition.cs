using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class GuideCondition
{
    public abstract bool Judge();

}

public class HasGold : GuideCondition
{
    public int Amount;
    public override bool Judge()
    {
        return GameRes.Coin >= Amount;
    }
}

public class FirstTime : GuideCondition
{
    public string trigger;
    public int value;
    public override bool Judge()
    {
        return PlayerPrefs.GetInt(trigger, 0) != value;
    }
}
