using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armorer : Enemy
{
    [SerializeField] float armorIntensify;
    public float ArmorIntensify { get => armorIntensify; set => armorIntensify = value; }
    public float ArmorCoolDown { get => armorCoolDown; set => armorCoolDown = value; }

    [SerializeField] float armorCoolDown;
    public override EnemyType EnemyType => EnemyType.Armorer;

}
