using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Ground, Target, Penetrate
}
public abstract class Bullet : GameBehavior
{
    protected const int enemyLayerMask = 1 << 11;
    public abstract BulletType BulletType { get; }
    protected TargetPoint target;
    Vector2 targetPos;
    public GameObject turretParent;
    protected Vector2 TargetPos
    {
        get => BulletType != BulletType.Target ? targetPos : target.Position;
        set => targetPos = value;
    }
    protected float bulletSpeed;
    protected readonly float minDistanceToDealDamage = .1f;
    protected float damage;
    protected float sputteringRange;
    protected float criticalRate;
    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public void Initialize(TargetPoint target, Vector2 targetPos, float damage, float bulletSpeed, float sputteringRange = 0, float criticalRate = 0)
    {
        this.target = target;
        this.TargetPos = targetPos;
        this.damage = damage;
        this.bulletSpeed = bulletSpeed;
        this.sputteringRange = sputteringRange;
        this.criticalRate = criticalRate;
    }

    public override bool GameUpdate()
    {
        if (target.Enemy.IsDie || !target.Enemy.gameObject.activeSelf)
        {
            ReclaimBullet();
            return false;
        }
        RotateBullet(TargetPos);
        return MoveTowards(TargetPos);
    }

    protected void RotateBullet(Vector2 pos)
    {
        Vector2 targetPos = pos - (Vector2)transform.position;
        float angle = Vector3.SignedAngle(transform.up, targetPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }

    protected bool MoveTowards(Vector2 pos)
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
        
    }


}
