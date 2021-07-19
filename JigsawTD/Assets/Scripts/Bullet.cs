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
    private List<TurretSkill> turretEffects;
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

    private float sputteringPercentage;
    public float SputteringPercentage { get => sputteringPercentage; set => sputteringPercentage = value; }

    private float criticalRate;
    public float CriticalRate { get => criticalRate; set => criticalRate = value; }

    private float criticalPercentage;
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }
    private float slowRate;
    public float SlowRate { get => slowRate; set => slowRate = value; }
    public ParticalControl SputteringEffect { get => sputteringEffect; set => sputteringEffect = value; }

    //用来判断是否击中护甲，如果击中则子弹被挡掉
    public bool hit;
    public bool isCritical;
    public int SputteredCount;
    private void Awake()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
        initScale = transform.localScale;
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        transform.localScale = initScale;
        SputteredCount = 0;
        isCritical = false;
        //hit = false;
    }

    public virtual void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        if (trailRenderer != null)
            trailRenderer.Clear();
        this.turretParent = turret;
        this.Target = target;
        this.TargetPos = pos ?? target.Position;

        this.Damage = turret.Strategy.FinalAttack;
        this.bulletSpeed = turret.Strategy.m_Att.BulletSpeed;
        this.SputteringRange = turret.Strategy.FinalSputteringRange;
        this.CriticalRate = turret.Strategy.FinalCriticalRate;
        this.CriticalPercentage = turret.Strategy.FinalCriticalPercentage;
        this.turretEffects = turret.Strategy.TurretSkills;
        this.SlowRate = turret.Strategy.FinalSlowRate;
        this.SputteringPercentage = turret.Strategy.FinalSputteringPercentage;
        TriggerShootEffect();
    }

    protected void TriggerShootEffect()
    {
        if (turretEffects.Count > 0)
        {
            foreach (TurretSkill effect in turretEffects)
            {
                effect.bullet = this;
                effect.Shoot();
            }
        }
    }

    public void TriggerPreHitEffect()
    {
        isCritical = UnityEngine.Random.value <= CriticalRate;//在命中敌人前判断是否暴击
        if (turretEffects.Count > 0)
        {
            foreach (TurretSkill effect in turretEffects)
            {
                effect.PreHit();
            }
        }
    }

    protected void TriggerHitEffect(Enemy target)
    {
        if (turretEffects.Count > 0)
        {
            foreach (TurretSkill effect in turretEffects)
            {
                effect.Hit(target);
            }
        }
        if (SlowRate > 0 && !target.IsDie)//技能可能会修改slowrate
        {
            BuffInfo info = new BuffInfo(EnemyBuffName.SlowDown, SlowRate, 2f);
            target.Buffable.AddBuff(info);
        }
    }

    public virtual bool GameUpdate()
    {
        //if (!hit)
        //{
            if (Target != null && (Target.Enemy.IsDie || !Target.gameObject.activeSelf))
            {
                TargetPos = Target.transform.position;
                Target = null;
            }
            RotateBullet(TargetPos);
            return MoveTowards(TargetPos);
        //}
        //return false;

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

    }
    public void DealRealDamage(IDamageable damageable, bool isSputtering=false)
    {
        float finalDamage = isCritical ? Damage * CriticalPercentage : Damage;
        if (isSputtering)
            finalDamage *= SputteringPercentage;
        float realDamage;
        damageable.ApplyDamage(finalDamage, out realDamage, isCritical);
        turretParent.Strategy.DamageAnalysis += (int)realDamage;
    }

    public void EnemyDamageProcess(IDamageable target,bool isSputtering=false)
    {
        TriggerHitEffect((Enemy)target);
        DealRealDamage(target,isSputtering);
    }
}
