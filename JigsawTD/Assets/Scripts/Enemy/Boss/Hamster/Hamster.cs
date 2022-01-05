using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : Enemy
{

    public static bool isFirstHamster = false;
    public static int HamsterCount;
    public static float HamsterDamageIntensify;

    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset,intensify);
        if (!isFirstHamster)
        {
            HamsterCount = attribute.InitCount;
            isFirstHamster = true;
        }
    }
    protected override void SetStrategy()
    {
        DamageStrategy = new HamsterStrategy(this);
    }

    protected override void OnDie()
    {
        base.OnDie();
        HamsterCount--;
        if (HamsterCount == 0)
        {
            isFirstHamster = false;
        }
    }

}
