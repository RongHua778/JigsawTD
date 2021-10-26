using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froster : Enemy
{
    public override string ExplosionEffect => "EnemyExplosionBlue";

    public float freezeRange;
    public float freezeTime;


    protected override void OnDie()
    {
        base.OnDie();
        StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);// FrostTurrets();

    }

}
