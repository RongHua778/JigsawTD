using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDetect : MonoBehaviour
{
    Turret Turret; 
    private void Awake()
    {
        Turret = this.transform.root.GetComponentInChildren<Turret>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TargetPoint target = collision.GetComponent<TargetPoint>();
            foreach(var effect in Turret.TurretEffects)
            {
                effect.EnemyEnter();
            }
            Turret.AddTarget(target);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TargetPoint target = collision.GetComponent<TargetPoint>();
            foreach (var effect in Turret.TurretEffects)
            {
                effect.EnemyExit();
            }
            Turret.RemoveTarget(target);
        }
    }

}
