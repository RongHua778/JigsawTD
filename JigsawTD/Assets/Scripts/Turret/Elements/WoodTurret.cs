using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTurret : Turret
{
    private Animator anim;
    public override float SputteringRange => 0;
    public override void InitializeTurret()
    {
        base.InitializeTurret();
        anim = this.GetComponent<Animator>();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }

    public override bool GameUpdate()
    {
        if (Target.Count == 0)
        {
            anim.SetBool("Attacking", false);
        }
        else
        {
            anim.SetBool("Attacking", true);
        }
        return base.GameUpdate();

    }
    protected override void Shoot()
    {
        foreach (var target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            float offset = Random.Range(-0.03f, 0.03f);
            bullet.transform.position = shootPoint.position + offset * shootPoint.right;
            Vector2 dir = bullet.transform.position - transform.position;
            Vector2 pos = (Vector2)shootPoint.position + dir.normalized * AttackRange;
            bullet.Initialize(this, target, pos);
        }
    }



}
