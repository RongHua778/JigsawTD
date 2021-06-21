using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmorBoss : Enemy
{
    public override EnemyType EnemyType => EnemyType.BossRotatingArmor;

    public float ArmorIntensify { get => armorIntensify; set => armorIntensify = value; }
    public float ArmorCoolDown { get => armorCoolDown; set => armorCoolDown = value; }

    [SerializeField] float armorIntensify;
    [SerializeField] float armorCoolDown;
}
