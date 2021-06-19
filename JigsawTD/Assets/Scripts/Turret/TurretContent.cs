using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TurretContent : GameTileContent, IGameBehavior
{
    //**********塔是否被激活，如果未激活则不会动
    private bool activated;
    private float inActivatedTime;
    //**********
    public override bool IsWalkable => false;
    public bool Dropped { get; set; }
    [HideInInspector] public TurretAttribute m_TurretAttribute;
    public List<TargetPoint> targetList = new List<TargetPoint>();

    private RangeIndicator rangeIndicator;
    protected List<RangeIndicator> rangeIndicators;
    protected List<RangeIndicator> currentRangetors;

    private Transform rangeParent;
    private float nextAttackTime;
    private Quaternion look_Rotation;
    protected float _rotSpeed = 10f;

    protected RangeType RangeType;
    protected Bullet bulletPrefab;

    //**********美术，动画及音效
    protected Animator turretAnim;
    protected AudioSource audioSource;
    protected SpriteRenderer TurretBaseSprite;
    protected SpriteRenderer CannonSprite;
    protected AudioClip ShootClip;
    //**********

    private List<TargetPoint> target = new List<TargetPoint>();
    public List<TargetPoint> Target { get => target; set => target = value; }

    protected Collider2D detectCollider;
    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    //品质
    private int quality = 0;
    public int Quality
    {
        get => quality;
        set
        {
            quality = value;
            SetGraphic();
            GenerateRange();
            GetTurretEffects();
        }
    }
    private int currentRange = 0;//为检测范围变化时的Rangeindicator修改



    //公开属性
    private int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }

    public virtual float AttackDamage { get => (m_TurretAttribute.TurretLevels[Quality - 1].AttackDamage) * (1 + BaseAttackIntensify) * (1 + AttackIntensify); }
    public virtual int AttackRange { get => m_TurretAttribute.TurretLevels[Quality - 1].AttackRange + RangeIntensify; }
    public int ForbidRange { get => m_TurretAttribute.TurretLevels[Quality - 1].ForbidRange; }
    public virtual float AttackSpeed { get => (m_TurretAttribute.TurretLevels[Quality - 1].AttackSpeed) * (1 + SpeedIntensify); }
    public float BulletSpeed { get => m_TurretAttribute.BulletSpeed; }
    public virtual float SputteringRange { get => m_TurretAttribute.TurretLevels[Quality - 1].SputteringRange + SputteringIntensify; }
    public float CriticalRate { get => m_TurretAttribute.TurretLevels[Quality - 1].CriticalRate + CriticalIntensify; }
    public float SlowRate { get => m_TurretAttribute.TurretLevels[Quality - 1].SlowRate + SlowIntensify; }

    //隐藏属性
    float sputteringRate = 0.5f;//溅射伤害率
    public float SputteringRate { get => sputteringRate; set => sputteringRate = value; }

    float criticalPercentage = 1.5f;//暴击伤害率
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }

    int targetCount = 1;//目标数
    public int TargetCount { get => targetCount; set => targetCount = value; }



    //攻击力加成
    public virtual float AttackIntensify { get => TileAttackIntensify + TurnAttackIntensify; }//最终攻击加成

    float turnAttackIntensify;//回合临时加成
    public float TurnAttackIntensify { get => turnAttackIntensify; set => turnAttackIntensify = value; }

    float tileAttackIntensify;//地形加成
    public float TileAttackIntensify { get => tileAttackIntensify; set => tileAttackIntensify = value; }

    float baseAttackIntensify;//基础修正，只加成基础攻击
    public float BaseAttackIntensify { get => baseAttackIntensify; set => baseAttackIntensify = value; }
    //********************

    //范围加成
    public int RangeIntensify { get => TileRangeIntensify; }//最终范围加成
    int tileRangeIntensify;//地形加成
    public int TileRangeIntensify { get => tileRangeIntensify; set => tileRangeIntensify = value; }
    //********************

    //攻速加成
    public virtual float SpeedIntensify { get => TurnSpeedIntensify; }

    float turnSpeedIntensify;
    public float TurnSpeedIntensify { get => turnSpeedIntensify; set => turnSpeedIntensify = value; }
    //*********************

    //暴击率加成
    public virtual float CriticalIntensify { get; }
    //减速效果加成
    public virtual float SlowIntensify { get; }
    //溅射范围加成
    public virtual float SputteringIntensify { get; }


    private List<TurretEffectInfo> TurretEffectInfos => m_TurretAttribute.TurretLevels[Quality - 1].TurretEffects;

    public List<TurretEffect> TurretEffects = new List<TurretEffect>();


    private void Awake()
    {
        rangeIndicators = new List<RangeIndicator>();
        currentRangetors = new List<RangeIndicator>();
        rangeIndicator = Resources.Load<RangeIndicator>("Prefabs/RangeIndicator");
        rangeParent = transform.Find("TurretRangeCol");
        detectCollider = rangeParent.GetComponent<Collider2D>();
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        TurretBaseSprite = transform.Find("TurretBase").GetComponent<SpriteRenderer>();
        CannonSprite = rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();
        turretAnim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    }

    public void InitializeTurret(TurretAttribute attribute, int quality)
    {
        m_TurretAttribute = attribute;
        RangeType = m_TurretAttribute.RangeType;
        Quality = quality;
        rotTrans.localRotation = Quaternion.identity;
        bulletPrefab = m_TurretAttribute.Bullet;
        ShootClip = m_TurretAttribute.ShootSound;
        Activate();
    }

    //在激活的时候调用
    public virtual void Activate()
    {
        SpriteRenderer s = GetComponentInChildren<SpriteRenderer>();
        s.material.color = Color.white;
        activated = true;
    }

    //在不激活的时候调用
    public virtual void InActivate(float time) 
    {
        inActivatedTime = time;
        activated = false;
        SpriteRenderer s = GetComponentInChildren<SpriteRenderer>();
        s.material.color = Color.blue;
        Invoke("Activate", inActivatedTime); 
    }


    protected virtual void PlayAudio(AudioClip clip, bool isAuto)
    {
        audioSource.volume = StaticData.Instance.EnvrionmentBaseVolume;
        audioSource.clip = clip;
        if (isAuto)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void GetTurretEffects()//获取并激活效果
    {
        TurretEffects.Clear();
        foreach (TurretEffectInfo info in TurretEffectInfos)
        {
            TurretEffect effect = TurretEffectFactory.GetEffect((int)info.EffectName);
            effect.turret = this;
            effect.KeyValue = info.KeyValue;
            TurretEffects.Add(effect);
            effect.Build();
        }
    }

    //设置不同等级的美术资源
    public virtual void SetGraphic()
    {
        shootPoint.transform.localPosition = m_TurretAttribute.TurretLevels[Quality - 1].ShootPointOffset;
        TurretBaseSprite.sprite = m_TurretAttribute.TurretLevels[Quality - 1].BaseSprite;
        CannonSprite.sprite = m_TurretAttribute.TurretLevels[Quality - 1].CannonSprite;
    }

    //public virtual void TriggerPoloEffect(bool value)
    //{
    //    if (m_TurretAttribute.TurretLevels[Quality - 1].PoloEffects.Count > 0)
    //    {
    //        List<Vector2> poss = StaticData.GetCirclePoints(AttackRange, ForbidRange);
    //        foreach (var polo in m_TurretAttribute.TurretLevels[Quality - 1].PoloEffects)
    //        {
    //            switch (polo.EffectType)
    //            {
    //                case PoloEffectType.RangeIntensify:
    //                    foreach (var pos in poss)
    //                    {
    //                        GroundTile groungTile = StaticData.RaycastCollider(pos + (Vector2)transform.position, StaticData.GetGroundLayer).GetComponent<GroundTile>();
    //                        groungTile.RangeIntensify += value ? (int)polo.KeyValue : -(int)polo.KeyValue;
    //                        groungTile.TriggerIntensify();
    //                    }
    //                    break;
    //                case PoloEffectType.AttackIntensify:
    //                    foreach (var pos in poss)
    //                    {
    //                        GroundTile groungTile = StaticData.RaycastCollider(pos + (Vector2)transform.position, StaticData.GetGroundLayer).GetComponent<GroundTile>();
    //                        groungTile.AttackIntensify += value ? polo.KeyValue : -polo.KeyValue;
    //                        groungTile.TriggerIntensify();
    //                    }
    //                    break;
    //            }
    //        }
    //    }
    //}

    public void AddTarget(TargetPoint target)
    {
        targetList.Add(target);
        AcquireTarget();
    }

    public virtual void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.Target.Contains(target))
            {
                this.Target.Remove(target);
                //this.Target = null;
            }
            targetList.Remove(target);
        }
    }

    //在塔被激活后每一帧都会调用的方法
    public virtual void OnActivating()
    {
        if (TrackTarget() || AcquireTarget())
        {
            RotateTowards();
            FireProjectile();
        }
    }

    public virtual bool GameUpdate()
    {
        if (!Dropped)
            return false;
        if (activated)
        {
            OnActivating();
        }

        return true;
    }

    private bool TrackTarget()
    {
        if (Target.Count == 0)
        {
            return false;
        }
        for (int i = 0; i < Target.Count; i++)
        {
            if (Target[i].Enemy.IsDie)
            {
                targetList.Remove(Target[i]);
                Target.Remove(Target[i]);
            }
        }
        if (Target.Count == 0)
            return false;

        return true;
    }
    private bool AcquireTarget()
    {
        if (targetList.Count <= 0)
            return false;
        else
        {
            if (TargetCount > Target.Count)
            {
                Target.Clear();
                List<int> returnInt = StaticData.SelectNoRepeat(targetList.Count, TargetCount);
                var ints = returnInt.GetEnumerator();
                while (ints.MoveNext())
                {
                    Target.Add(targetList[ints.Current]);
                }
            }
            else if (Target.Count <= 0)
            {
                Target.Add(targetList[UnityEngine.Random.Range(0, targetList.Count - 1)]);
            }

            return false;
        }
    }

    public void ShowRange(bool show)
    {
        ShowingRange = show;
        var ranges = currentRangetors.GetEnumerator();
        while (ranges.MoveNext())
        {
            ranges.Current.ShowSprite(show);
        }
    }
    public void GenerateRange()
    {
        int m = rangeIndicators.Count;
        if (AttackRange == currentRange)
            return;
        if (currentRangetors.Count > 0)
            RecycleRanges();
        List<Vector2Int> points = null;
        switch (RangeType)
        {
            case RangeType.Circle:
                points = StaticData.GetCirclePoints(AttackRange);
                ((BoxCollider2D)detectCollider).size = Vector2.one * (2 * AttackRange + 1) * Mathf.Cos(45 * Mathf.Deg2Rad);
                break;
            case RangeType.HalfCircle:
                points = StaticData.GetHalfCirclePoints(AttackRange);
                detectCollider.transform.localScale = Vector2.one * (AttackRange + 0.5f);
                break;
            case RangeType.Line:
                points = StaticData.GetLinePoints(AttackRange);
                ((BoxCollider2D)detectCollider).size = new Vector2(1, AttackRange);
                detectCollider.offset = new Vector2(0, 1 + 0.5f * (AttackRange - 1));
                break;
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (i >= m)
            {
                RangeIndicator rangeIndecator = Instantiate(rangeIndicator, transform);
                rangeIndecator.transform.localPosition = (Vector3Int)points[i];
                rangeIndicators.Add(rangeIndecator);
                currentRangetors.Add(rangeIndecator);
            }
            else
            {
                rangeIndicators[i].transform.localPosition = (Vector3Int)points[i];
                currentRangetors.Add(rangeIndicators[i]);
            }
        }
        currentRange = AttackRange;
        ShowRange(ShowingRange);
    }

    public void RecycleRanges()
    {
        var ranges = currentRangetors.GetEnumerator();
        while (ranges.MoveNext())
        {
            ranges.Current.ShowSprite(false);
        }
        currentRangetors.Clear();
    }

    protected virtual void RotateTowards()
    {
        if (Target.Count == 0)
            return;
        var dir = Target[0].transform.position - rotTrans.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotTrans.rotation = Quaternion.Lerp(rotTrans.rotation, look_Rotation, _rotSpeed * Time.deltaTime);
    }

    protected bool AngleCheck()
    {
        var angleCheck = Quaternion.Angle(rotTrans.rotation, look_Rotation);
        if (angleCheck < CheckAngle)
        {
            return true;
        }
        return false;
    }

    protected virtual void FireProjectile()
    {
        if (Time.time - nextAttackTime > 1 / AttackSpeed)
        {
            if (Target != null && AngleCheck())
            {
                Shoot();
            }
            else
            {
                return;
            }
            nextAttackTime = Time.time;
        }
    }

    protected virtual void Shoot()
    {
        turretAnim.SetTrigger("Attack");
        PlayAudio(ShootClip, false);
        var targets = Target.GetEnumerator();
        while (targets.MoveNext())
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as Bullet;
            bullet.transform.position = shootPoint.position;
            bullet.Initialize(this, targets.Current);
        }
    }

    //content类重载*************

    public override void ContentLanded()
    {
        base.ContentLanded();
        Collider2D col = StaticData.RaycastCollider(transform.position, LayerMask.GetMask(StaticData.ConcreteTileMask));
        ContentLandedCheck(col);

        Dropped = true;
        StaticData.SetNodeWalkable(m_GameTile, false, false);


    }

    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        ShowRange(value);
        if (value)
        {
            GameManager.Instance.ShowTurretTips(this);
        }

    }
    public override void CorretRotation()
    {
        base.CorretRotation();
        TurretBaseSprite.transform.rotation = Quaternion.identity;
    }

    protected override void ContentLandedCheck(Collider2D col)
    {
        if (col != null)
        {
            GameTile tile = col.GetComponent<GameTile>();
            ObjectPool.Instance.UnSpawn(tile);
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        if (Dropped)
            StaticData.SetNodeWalkable(m_GameTile, false, true);
        Dropped = false;
        targetList.Clear();
        ClearIntensify();
        ShowRange(false);
        ClearTurnIntensify();
    }

    private void ClearIntensify()
    {
        TileAttackIntensify = 0;
        BaseAttackIntensify = 0;
        TileRangeIntensify = 0;
        CriticalPercentage = 1.5f;
        TargetCount = 1;
        DamageAnalysis = 0;
    }

    public void ClearTurnIntensify()
    {
        TurnSpeedIntensify = 0;
        TurnAttackIntensify = 0;
    }

}
