using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K1Turret : CompositeTurret
{
    [SerializeField] ParticleSystem shootMuzzle = default;
    protected override void Shoot()
    {
        base.Shoot();
        shootMuzzle.Play();
    }
}
