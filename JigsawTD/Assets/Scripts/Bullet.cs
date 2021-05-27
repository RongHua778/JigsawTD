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

    public void Initialize(Turret turret, TargetPoint target = null, Vector2? pos = null)
    {
        this.turretParent = turret;
<<<<<<< Updated upstream
        this.Target = turret.Target[0];
        this.TargetPos = pos ?? turret.Target[0].Position;
=======
<<<<<<< Updated upstream
        this.Target = turret.Target;
        this.TargetPos = pos ?? turret.Target.Position;
=======
        this.Target = target;
        this.TargetPos = pos ?? target.Position;
>>>>>>> Stashed changes
>>>>>>> Stashed changes
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

    public float GetTargetDistance()
    {
        float distanceToTarget = ((Vector2)transform.position - TargetPos).magnitude;
        return distanceToTarget;
    }

    private void ReclaimBullet()
    {
        ObjectPool.Instance.UnSpawn(this.gameObject);
    }

    protected virtual void DealDamage()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.z -= 0.1f;
        Gizmos.DrawWireSphere(position, SputteringRange);
    }
}
