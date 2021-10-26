using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BulletType
{
    Ground, Target, Penetrate,Self
}
public abstract class Bullet : ReusableObject, IGameBehavior
{
    [SerializeField] private ParticalControl sputteringEffect = default;
    public abstract BulletType BulletType { get; }
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }

    private TurretContent turretParent;
    private List<TurretSkill> turretEffects;

    private Vector2 targetPos;
    protected virtual Vector2 TargetPos
    {
        get => targetPos;
        set => targetPos = value;
    }

    protected readonly float minDistanceToDealDamage = .1f;

    private float bulletSpeed;
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

    public bool isCritical;//是否暴击
    public int SputteredCount;//溅射目标数

    public override void OnSpawn()
    {
        base.OnSpawn();
        GameManager.Instance.nonEnemies.Add(this);
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        SputteredCount = 0;
        isCritical = false;
    }

    public virtual void Initialize(TurretContent turret, TargetPoint target = null, Vector2? pos = null)
    {
        this.turretParent = turret;
        this.Target = target;
        this.TargetPos = pos ?? target.Position;

        SetAttribute(turret);
        TriggerShootEffect();
    }

    protected void SetAttribute(TurretContent turret)
    {
        this.turretParent = turret;
        this.Damage = turret.Strategy.FinalAttack;
        this.bulletSpeed = turret.Strategy.m_Att.BulletSpeed;
        this.SputteringRange = turret.Strategy.FinalSputteringRange;
        this.CriticalRate = turret.Strategy.FinalCriticalRate;
        this.CriticalPercentage = turret.Strategy.FinalCriticalPercentage;
        this.turretEffects = turret.Strategy.TurretSkills;
        this.SlowRate = turret.Strategy.FinalSlowRate;
        this.SputteringPercentage = turret.Strategy.FinalSputteringPercentage;
    }

    protected void TriggerShootEffect()
    {
        foreach (TurretSkill effect in turretEffects)
        {
            effect.Shoot(this);
        }

    }

    public void TriggerPreHitEffect()//子弹命中前触发,溅射发生前
    {
        isCritical = UnityEngine.Random.value <= CriticalRate;//在命中敌人前判断是否暴击
        foreach (TurretSkill effect in turretEffects)
        {
            effect.PreHit(this);
        }
    }

    protected void TriggerHitEffect(IDamageable target)
    {
        foreach (TurretSkill effect in turretEffects)
        {
            effect.Hit(target, this);
        }
        if (SlowRate > 0 && target.DamageStrategy.IsEnemy)//技能可能会修改slowrate
        {
            target.DamageStrategy.ApplyBuff(EnemyBuffName.SlowDown, SlowRate, 2f);
        }
    }

    public virtual bool GameUpdate()
    {
        if (Target != null && !Target.gameObject.activeSelf)
        {
            TargetPos = Target.transform.position;
            Target = null;
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
        if ((distanceToTarget < minDistanceToDealDamage))
        {
            TriggerDamage();
            return false;
        }
        return true;
    }

    public float GetTargetDistance()
    {
        float distanceToTarget = ((Vector2)turretParent.transform.position - TargetPos).magnitude;
        return distanceToTarget;
    }

    public void ReclaimBullet()
    {
        ObjectPool.Instance.UnSpawn(this);
    }

    public virtual void TriggerDamage()
    {
        ReclaimBullet();
    }
    public void DealRealDamage(IDamageable target, bool isSputtering = false)
    {
        float finalDamage = isCritical ? Damage * CriticalPercentage : Damage;
        if (isSputtering)
            finalDamage *= SputteringPercentage;
        float realDamage;
        target.DamageStrategy.ApplyDamage(finalDamage, out realDamage, isCritical);
        turretParent.Strategy.DamageAnalysis += (int)realDamage;//防御塔伤害统计
    }

    public void DamageProcess(IDamageable target, bool isSputtering = false)
    {
        TriggerHitEffect(target);
        DealRealDamage(target, isSputtering);
    }


}
