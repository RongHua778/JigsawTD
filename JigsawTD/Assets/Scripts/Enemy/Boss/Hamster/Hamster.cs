using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hamster : Enemy
{
    public static bool isFirstHamster = false;
    public static int HamsterCount;
    public static float HamsterDamageIntensify;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> shortpath)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify, shortpath);
        if (!isFirstHamster)
        {
            HamsterCount = attribute.InitCount;
            isFirstHamster = true;
        }
    }
    protected override void SetStrategy()
    {
        DamageStrategy = new HamsterStrategy(model, this);
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
