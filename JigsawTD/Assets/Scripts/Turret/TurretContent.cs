using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class TurretContent : GameTileContent, IGameBehavior
{
    private float frostTime = 0;
    public bool Activated = true;

    public StrategyBase Strategy;//数值计算规则

    public override bool IsWalkable => false;
    public bool Dropped { get; set; }
    public List<TargetPoint> targetList = new List<TargetPoint>();

    private RangeIndicator rangeIndicator;
    protected List<RangeIndicator> rangeIndicators;
    protected List<RangeIndicator> currentRangetors;

    private Transform rangeParent;
    private float nextAttackTime;
    private Quaternion look_Rotation;


    protected RangeType RangeType;
    protected Bullet bulletPrefab;

    //**********美术，动画及音效
    protected Animator turretAnim;
    protected AudioSource audioSource;
    protected SpriteRenderer TurretBaseSprite;
    protected SpriteRenderer CannonSprite;
    protected AudioClip ShootClip;

    [SerializeField] protected ParticleSystem ShootEffect = default;
    //**********

    private List<TargetPoint> target = new List<TargetPoint>();
    public List<TargetPoint> Target { get => target; set => target = value; }

    protected Collider2D detectCollider;
    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    //控制sprite颜色
    protected List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    protected List<Color> originalColors = new List<Color>();

    private int currentRange = 0;//为检测范围变化时的Rangeindicator修改


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
        SpriteRenderer[] ss = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < ss.Length; i++)
        {
            sprites.Add(ss[i]);
            originalColors.Add(sprites[i].color);
        }
    }

    public virtual void InitializeTurret()
    {
        RangeType = Strategy.m_Att.RangeType;
        rotTrans.localRotation = Quaternion.identity;
        bulletPrefab = Strategy.m_Att.Bullet;
        ShootClip = Strategy.m_Att.ShootSound;
        SetGraphic();
        GenerateRange();
        Activated = true;
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


    //设置不同等级的美术资源
    public virtual void SetGraphic()
    {
        shootPoint.transform.localPosition = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].ShootPointOffset;
        CannonSprite.sprite = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].CannonSprite;
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


    public virtual void Frost(float time)
    {
        Activated = false;
        frostTime += time;
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
        if (frostTime > 0)
        {
            frostTime -= Time.deltaTime;
            if (frostTime <= 0)
                Activated = true;
        }
        if (Activated)
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
            if (Strategy.FinalTargetCount > Target.Count)
            {
                Target.Clear();
                List<int> returnInt = StaticData.SelectNoRepeat(targetList.Count, Strategy.FinalTargetCount);
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
        if (Strategy.FinalRange == currentRange)
            return;
        if (currentRangetors.Count > 0)
            RecycleRanges();
        List<Vector2Int> points = null;
        switch (RangeType)
        {
            case RangeType.Circle:
                points = StaticData.GetCirclePoints(Strategy.FinalRange);
                ((BoxCollider2D)detectCollider).size = Vector2.one * (2 * Strategy.FinalRange + 1) * Mathf.Cos(45 * Mathf.Deg2Rad);
                break;
            case RangeType.HalfCircle:
                points = StaticData.GetHalfCirclePoints(Strategy.FinalRange);
                detectCollider.transform.localScale = Vector2.one * (Strategy.FinalRange + 0.5f);
                break;
            case RangeType.Line:
                points = StaticData.GetLinePoints(Strategy.FinalRange);
                ((BoxCollider2D)detectCollider).size = new Vector2(1, Strategy.FinalRange);
                detectCollider.offset = new Vector2(0, 1 + 0.5f * (Strategy.FinalRange - 1));
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
        currentRange = Strategy.FinalRange;
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
        rotTrans.rotation = Quaternion.LerpUnclamped(rotTrans.rotation, look_Rotation, Strategy.RotSpeed * Time.deltaTime);
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
        if (Time.time - nextAttackTime > 1 / Strategy.FinalSpeed)
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
        ShootEffect.Play();
        PlayAudio(ShootClip, false);
        var targets = Target.GetEnumerator();
        while (targets.MoveNext())
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab) as Bullet;
            bullet.transform.position = shootPoint.position;
            bullet.transform.localScale += 0.05f * Strategy.Quality * Vector3.one;
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
            GameManager.Instance.ShowTurretTips(this.Strategy);
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
        Strategy = null;
        frostTime = 0;
        ShowRange(false);

    }

}
