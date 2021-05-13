using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Turret
{
    public override float AttackSpeed => base.AttackSpeed * (1 + SpeedIntensify);
    float speedIntensify;
    float SpeedIntensify { get => speedIntensify; set => speedIntensify = Mathf.Min(3, value); }
    float SpeedPerSecond = .5f;
    [SerializeField] LineRenderer lineSR = default;

    public override bool GameUpdate()
    {
        base.GameUpdate();
        SpeedControl();
        return true;
    }

    public override void InitializeTurret()
    {
        base.InitializeTurret();
        CheckAngle = 10f;
        lineSR.positionCount = 2;

    }

    public override void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.Target == target)
            {
                SpeedIntensify = 0;
                this.Target = null;
            }
            targetList.Remove(target);
        }
    }
    private void SpeedControl()
    {
        if (Target != null && AngleCheck())
        {
            lineSR.enabled = true;
            lineSR.SetPosition(0, transform.position);
            lineSR.SetPosition(1, Target.Position);
            SpeedIntensify += SpeedPerSecond * Time.deltaTime;
        }
        else
        {
            lineSR.enabled = false;
        }

    }

    protected override void Shoot()
    {
        Target.Enemy.ApplyDamage(AttackDamage);
    }
}
