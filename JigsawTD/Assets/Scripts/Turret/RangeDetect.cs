using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetect : MonoBehaviour
{
    TurretContent Turret;
    private void Awake()
    {
        Turret = this.transform.root.GetComponentInChildren<TurretContent>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        //foreach (var effect in Turret.TurretEffects)
        //{
        //    effect.EnemyEnter();
        //}
        Turret.AddTarget(target);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TargetPoint target = collision.GetComponent<TargetPoint>();
        //foreach (var effect in Turret.TurretEffects)
        //{
        //    effect.EnemyExit();
        //}
        Turret.RemoveTarget(target);
    }

}
