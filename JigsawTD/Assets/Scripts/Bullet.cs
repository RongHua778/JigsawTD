using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Ground, Target, Penetrate
}
public abstract class Bullet : ReusableObject, IGameBehavior
{
    [SerializeField] private ParticalControl sputteringEffect = default;
    //[SerializeField] protected ParticalControl HitEffect = default;
    TrailRenderer trailRenderer;
    protected const int enemyLayerMask = 1 << 11;
    public abstract BulletType BulletType { get; }
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }
    Vector2 targetPos;
    [HideInInspector] public TurretContent turretParent;
    private List<TurretEffect> turretEffects;
    protected virtual Vector2 TargetPos
    {
        get => targetPos;
        set => targetPos = value;
    }

    private Vector3 initScale;
    protected float bulletSpeed;
    protected readonly float minDistanceToDealDamage = .1f;
    private float damage;
    public float Damage { get => damage; set => damage = value; }
    private float sputteringRange;
    public float SputteringRange { get => sputteringRange; set => sputteringRange = value; }

    private float sputteringRate;
    public float SputteringRate { get => sputteringRate; set => sputteringRate = value; }

    private float criticalRate;
    public float CriticalRate { get => criticalRate; set => criticalRate = value; }

    private float criticalPercentage;//溢出的暴击率转为暴击伤害
    public float CriticalPercentage { get => criticalPercentage + Mathf.Max(0, CriticalRate - 1); set => criticalPercentage = value; }
    private float slowRate;
    public float SlowRate { get => slowRate; set => slowRate = value; }
    public ParticalControl SputteringEffect { get => sputteringEffect; set => sputteringEffect = value; }

    //用来判断是否击中护甲，如果击中则子弹被挡掉
    public bool hit;
    private void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        initScale = transform.localScale;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
        hit = false;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        transform.localScale = initScale;
    }

    public virtual void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        if (trailRenderer != null)
            trailRenderer.Clear();
        this.turretParent = turret;
        this.Target = target;
        this.TargetPos = pos ?? target.Position;
        //this.Damage = turret.AttackDamage;
        //this.bulletSpeed = turret.BulletSpeed;
        //this.SputteringRange = turret.SputteringRange;
        //this.CriticalRate = turret.CriticalRate;
        //this.CriticalPercentage = turret.CriticalPercentage;
        //this.turretEffects = turret.TurretEffects;
        //this.SlowRate = turret.SlowRate;
        //this.SputteringRate = turret.SputteringRate;

        this.Damage = turret.Strategy.AttackDamage;
        this.bulletSpeed = turret.Strategy.BulletSpeed;
        this.SputteringRange = turret.Strategy.SputteringRange;
        this.CriticalRate = turret.Strategy.CriticalRate;
        this.CriticalPercentage = turret.Strategy.CriticalPercentage;
        this.turretEffects = turret.Strategy.TurretEffects;
        this.SlowRate = turret.Strategy.SlowRate;
        this.SputteringRate = turret.Strategy.SputteringRate;

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

    public virtual bool GameUpdate()
    {
        if (!hit)
        {
            if (Target != null && (Target.Object.IsDie || !Target.Object.gameObject.activeSelf))
            {
                TargetPos = Target.transform.position;
                Target = null;
            }
            RotateBullet(TargetPos);
            return MoveTowards(TargetPos);
        }
        return false;

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
        if ((distanceToTarget < minDistanceToDealDamage))
        {
            TriggerDamage();
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

    public void ReclaimBullet()
    {
        ObjectPool.Instance.UnSpawn(this);
    }

    public virtual void TriggerDamage()
    {
        //if ( HitEffect!= null)
        //{
        //    ParticalControl effect = ObjectPool.Instance.Spawn(HitEffect) as ParticalControl;
        //    effect.transform.position = transform.position;
        //    effect.PlayEffect();
        //}
    }
    protected void DealRealDamage(Enemy enemy, float damage)
    {
        bool isCritical = UnityEngine.Random.value <= CriticalRate; ;
        float finalDamage = isCritical ? damage * CriticalPercentage : damage;
        TriggerHitEffect(enemy);
        float realDamage;
        enemy.ApplyDamage(finalDamage, out realDamage, isCritical);
        turretParent.Strategy.DamageAnalysis += (int)realDamage;
    }

}
