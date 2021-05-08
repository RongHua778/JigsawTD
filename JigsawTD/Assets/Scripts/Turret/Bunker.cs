using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bunker : Turret
{
    private void Awake()
    {
        AttackDamage = 5f;
        AttackSpeed = 1.5f;
        AttackRange = 1;
    }

}
