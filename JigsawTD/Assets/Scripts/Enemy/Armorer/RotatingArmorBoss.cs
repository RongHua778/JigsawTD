using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmorBoss : Armorer
{
    public float turningSpeed;
    public override EnemyType EnemyType => EnemyType.BossRotatingArmor;
    private void Update()
    {

    }

}
