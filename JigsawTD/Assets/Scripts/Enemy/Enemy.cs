using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : PathFollower
{
    public abstract EnemyType EnemyType { get; }

    protected bool trapTriggered = false;
    public int ReachDamage { get; set; }
    public int TileStunCounter { get; set; }
    private float stunTime;
    public float StunTime
    {
        get => stunTime;
        set
        {
            stunTime = value;
            progressFactor = Speed * adjust;
        }
    }
    public float initialSpeed;
    public override float Speed { get => StunTime > 0 ? 0 : Mathf.Max(0.1f, speed * (1 - (SlowRate + PathSlow) / (SlowRate + PathSlow + 0.8f))); set => speed = value; }

    float slowRate;
    public float SlowRate
    {
        get => slowRate;
        set
        {
            slowRate = value;
            progressFactor = Speed * adjust;//子弹减速即时更新速度
            healthBar.ShowSlowIcon(slowRate > 0.01f);
        }
    }
    float pathSlow;
    public float PathSlow
    {
        get => pathSlow;
        set
        {
            pathSlow = value;
            progressFactor = Speed * adjust;//子弹减速即时更新速度
        }
    }
    int brokeShell;
    public int BrokeShell { get => brokeShell; set => brokeShell = value; }

    [Header("HealthSetting")]
    HealthBar healthBar;
    public override bool GameUpdate()
    {
        if (IsDie)
        {
            StopAllCoroutines();
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = model.transform.position;
            GameEvents.Instance.EnemyDie(this);
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        if (StunTime >= 0)
        {
            StunTime -= Time.deltaTime;
            if (StunTime < 0)
                progressFactor = Speed * adjust;
        }
        progress += Time.deltaTime * progressFactor;

        if (!trapTriggered && progress >= 0.5f)
        {
            TriigerTrap();
        }

        while (progress >= 1f)
        {
            if (PointIndex == pathPoints.Count - 1)
            {
                try
                {
                    StartCoroutine(ExitCor());
                }
                catch
                {
                    Debug.LogAssertion("线程丢失");
                }
                return false;
            }
            trapTriggered = false;
            progress = 0;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    protected void TriigerTrap()
    {
        CurrentTile.OnTilePass(this);
        trapTriggered = true;
        Buffable.TileTick();
    }

    protected IEnumerator ExitCor()
    {
        anim.SetTrigger("Exit");
        yield return new WaitForSeconds(0.5f);
        GameEvents.Instance.EnemyReach(this);
        ObjectPool.Instance.UnSpawn(this);
    }
    public void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        this.pathOffset = pathOffset;
        this.healthBar = healthBar;
        this.healthBar.followTrans = model;
        Buffable = this.GetComponent<BuffableEntity>();
        CurrentHealth = MaxHealth = Mathf.RoundToInt(attribute.Health * intensify);
        Speed = attribute.Speed;
        initialSpeed = attribute.Speed;
        DamageIntensify = attribute.Shell;
        ReachDamage = attribute.ReachDamage;
        Type = ObjectType.Enemy;
    }

    protected override void PrepareIntro()
    {
        base.PrepareIntro();
        anim.Play("Default");
        anim.SetTrigger("Enter");
    }

    public override void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        base.ApplyDamage(amount, out realDamage,isCritical);
        healthBar.FillAmount = CurrentHealth / MaxHealth;
        if (isCritical)
        {
            healthBar.ShowJumpDamage((int)realDamage);
        }
    }

    public override void OnUnSpawn()
    {
        ObjectPool.Instance.UnSpawn(healthBar);
        TargetDamageCounter = 0;
        TileStunCounter = 0;
        DamageIntensify = 0;
        PathSlow = 0;
        SlowRate = 0;
        BrokeShell = 0;
        StunTime = 0;
        CurrentTile = null;
        Buffable.RemoveAllBuffs();
    }
}
