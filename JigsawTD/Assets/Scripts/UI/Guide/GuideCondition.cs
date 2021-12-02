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
