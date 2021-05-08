using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Ground, Target
}
public abstract class Bullet : GameBehavior
{
    public abstract BulletType BulletType { get;}
    protected TargetPoint target;
    protected Vector2 targetPos;
    protected float bulletSpeed;
    protected readonly float minDistanceToDealDamage = .1f;
    protected float damage;
    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public void Initialize(TargetPoint target,float damage)
    {
        this.target = target;
        targetPos = target.Position;
        this.damage = damage;

    }

    public override bool GameUpdate()
    {
        if (target.Enemy.IsDie || !target.Enemy.gameObject.activeSelf)
        {
            ReclaimBullet();
            return false;
        }
        switch (BulletType)
        {
            case BulletType.Ground:
                return MoveTowards(targetPos);
            case BulletType.Target:
                return MoveTowards(target.Position);
        }
        return true;
    }

    private bool MoveTowards(Vector2 pos)
    {
        transform.position = Vector2.MoveTowards(transform.position,
            pos, bulletSpeed * Time.deltaTime);
        float distanceToTarget = ((Vector2)transform.position - pos).magnitude;
        if (distanceToTarget < minDistanceToDealDamage)
        {
            DealDamage();
            ReclaimBullet();
            return false;
        }
        return true;
    }

    private void ReclaimBullet()
    {
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    protected virtual void DealDamage()
    {
        target.Enemy.ApplyDamage(damage);
    }


}
