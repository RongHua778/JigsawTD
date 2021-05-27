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
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }
    Vector2 targetPos;
    [HideInInspector] public Turret turretParent;
    private List<TurretEffect> turretEffects;
    protected Vector2 TargetPos
    {
        get => BulletType != BulletType.Target ? targetPos : Target.Position;
        set => targetPos = value;
    }


    protected float bulletSpeed;
    protected readonly float minDistanceToDealDamage = .1f;
    private float damage;
    public float Damage { get => damage; set => damage = value; }
    private float sputteringRange;
    public float SputteringRange { get => sputteringRange; set => sputteringRange = value; }

    private float criticalRate;
    public float CriticalRate { get => criticalRate; set => criticalRate = value; }
    private float slowRate;
    public float SlowRate { get => slowRate; set => slowRate = value; }

    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
    }

    public void Initialize(Turret turret, Vector2? pos = null)
    {
        this.turretParent = turret;
        this.Target = turret.Target[0];
        this.TargetPos = pos ?? turret.Target[0].Position;
        this.Damage = turret.AttackDamage;
        this.bulletSpeed = turret.BulletSpeed;
        this.SputteringRange = turret.SputteringRange;
        this.CriticalRate = turret.CriticalRate;
        this.turretEffects = turret.TurretEffects;
        this.SlowRate = turret.SlowRate;
        TriggerShootEffect();
    }

    protected void TriggerShootEffect()
    {
        if (turretEffects.Count > 0)
        {
            foreach (TurretEffect effect in turretEffects)
            {
                effect.bullet = this;
                effect.Shoot();
            }
        }
    }

    protected void TriggerHitEffect(Enemy target)
    {
        if (SlowRate > 0)
        {
            BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, SlowRate, 2f);
            target.Buffable.AddBuff(info);
        }
        if (turretEffects.Count > 0)
        {
            foreach (TurretEffect effect in turretEffects)
            {
                effect.bullet = this;
                effect.Hit(target);
            }
        }
    }

    public override bool GameUpdate()
    {
        if (Target.Enemy.IsDie || !Target.Enemy.gameObject.activeSelf)
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
