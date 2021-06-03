using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretType
{
    ElementTurret, CompositeTurret
}
public abstract class Turret : GameBehavior
{
    public virtual TurretType TurretType => TurretType.ElementTurret;

    public TurretTile m_TurretTile;
    private GameObject rangeIndicator;
    private Transform rangeParent;
    private float nextAttackTime;
    private Quaternion look_Rotation;


    //**********动画及音效
    protected Animator turretAnim;
    protected AudioSource audioSource;


    protected RangeType RangeType;
    protected GameObject bulletPrefab = default;
    protected float _rotSpeed = 10f;
    private List<TargetPoint> target = new List<TargetPoint>();
    public List<TargetPoint> Target { get => target; set => target = value; }
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    protected CompositeCollider2D detectCollider;
    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    [HideInInspector] public bool Dropped = false;

    public List<TargetPoint> targetList = new List<TargetPoint>();


    //[Header("美术资源设置")]
    protected SpriteRenderer BaseSprite;
    protected SpriteRenderer CannonSprite;
    protected AudioClip ShootClip;
    //************

    //[Header("TurretAttribute")]
    //private int level = 0;
    //public int Level { get => level; set => level = value; }

    [HideInInspector] public TurretAttribute m_TurretAttribute = default;
    private int damageAnalysis;
    public int DamageAnalysis { get => damageAnalysis; set => damageAnalysis = value; }
    //塔的品质
    private int quality = 1;
    public int Quality
    {
        get => quality; set
        {
            quality = value;
            SetGraphic();
        }
    }
    //塔的元素属性
    private Element element;
    public Element Element { get => element; set => element = value; }

    public virtual float AttackDamage { get => (m_TurretAttribute.TurretLevels[Quality - 1].AttackDamage + TurnAdditionalAttack) * (1 + AttackIntensify); }
    public virtual int AttackRange { get => m_TurretAttribute.TurretLevels[Quality - 1].AttackRange + RangeIntensify; }
    public int ForbidRange { get => m_TurretAttribute.TurretLevels[Quality - 1].ForbidRange; }
    public virtual float AttackSpeed { get => (m_TurretAttribute.TurretLevels[Quality - 1].AttackSpeed + turnAdditionalSpeed) * (1 + SpeedIntensify); }
    public float BulletSpeed { get => m_TurretAttribute.BulletSpeed; }
    public virtual float SputteringRange { get => m_TurretAttribute.TurretLevels[Quality - 1].SputteringRange + SputteringIntensify; }
    public float CriticalRate { get => m_TurretAttribute.TurretLevels[Quality - 1].CriticalRate + CriticalIntensify; }
    public float SlowRate { get => m_TurretAttribute.TurretLevels[Quality - 1].SlowRate + SlowIntensify; }

    float criticalPercentage = 1.5f;
    public float CriticalPercentage { get => criticalPercentage; set => criticalPercentage = value; }

    int targetCount = 1;
    public int TargetCount { get => targetCount; set => targetCount = value; }


    //**************回合临时属性
    int turnAddtionalAttack = 0;
    public int TurnAdditionalAttack { get => turnAddtionalAttack; set => turnAddtionalAttack = value; }
    float turnAdditionalSpeed = 0;
    public float TurnAdditionalSpeed { get => turnAdditionalSpeed; set => turnAdditionalSpeed = value; }

    //*************



    //*************光环加成
    float attackIntensify;
    public virtual float AttackIntensify { get => attackIntensify; set => attackIntensify = value; }
    int rangeIntensify;
    public int RangeIntensify
    {
        get => rangeIntensify;
        set
        {
            rangeIntensify = value;
            GenerateRange();
        }
    }
    float speedIntensify;
    public virtual float SpeedIntensify { get => speedIntensify; set => speedIntensify = value; }

    float criticalIntensify;
    public virtual float CriticalIntensify { get => criticalIntensify; set => criticalIntensify = value; }
    float slowIntensify;
    public virtual float SlowIntensify { get => slowIntensify; set => slowIntensify = value; }
    float sputteringIntensify;
    public virtual float SputteringIntensify { get => sputteringIntensify; set => sputteringIntensify = value; }

    //*************

    public List<TurretEffectInfo> TurretEffectInfos => m_TurretAttribute.TurretLevels[Quality - 1].TurretEffects;


    public List<TurretEffect> TurretEffects = new List<TurretEffect>();


    private void Awake()
    {
        rangeIndicator = Resources.Load<GameObject>("Prefabs/RangeIndicator");
        rangeParent = transform.Find("TurretRangeCol");
        detectCollider = rangeParent.GetComponent<CompositeCollider2D>();
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        BaseSprite = transform.root.Find("TileBase/TurretBase").GetComponent<SpriteRenderer>();
        CannonSprite = rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();
        turretAnim = this.GetComponent<Animator>();
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //audioSource.spatialBlend = 1;
    }

    public virtual void InitializeTurret()
    {
        rotTrans.localRotation = Quaternion.identity;
        RangeType = m_TurretAttribute.RangeType;
        Element = m_TurretAttribute.element;
        bulletPrefab = m_TurretAttribute.Bullet;
        ShootClip = m_TurretAttribute.ShootSound;
        SetGraphic();
        GenerateRange();
        GetTurretEffects();
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
        shootPoint.transform.localPosition = m_TurretAttribute.TurretLevels[quality - 1].ShootPointOffset;
        BaseSprite.sprite = m_TurretAttribute.TurretLevels[quality - 1].BaseSprite;
        CannonSprite.sprite = m_TurretAttribute.TurretLevels[quality - 1].CannonSprite;
    }

    public virtual void TriggerPoloEffect(bool value)
    {
        if (m_TurretAttribute.TurretLevels[Quality - 1].PoloEffects.Count > 0)
        {
            List<Vector2> poss = StaticData.GetCirclePoints(AttackRange, ForbidRange);
            foreach (var polo in m_TurretAttribute.TurretLevels[Quality - 1].PoloEffects)
            {
                switch (polo.EffectType)
                {
                    case PoloEffectType.RangeIntensify:
                        foreach (var pos in poss)
                        {
                            GroundTile groungTile = StaticData.RaycastCollider(pos + (Vector2)transform.position, StaticData.GetGroundLayer).GetComponent<GroundTile>();
                            groungTile.RangeIntensify += value ? (int)polo.KeyValue : -(int)polo.KeyValue;
                            groungTile.TriggerIntensify();
                        }
                        break;
                    case PoloEffectType.AttackIntensify:
                        foreach (var pos in poss)
                        {
                            GroundTile groungTile = StaticData.RaycastCollider(pos + (Vector2)transform.position, StaticData.GetGroundLayer).GetComponent<GroundTile>();
                            groungTile.AttackIntensify += value ? polo.KeyValue : -polo.KeyValue;
                            groungTile.TriggerIntensify();
                        }
                        break;
                }
            }
        }
    }

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

    public void RecycleRanges()
    {
        foreach (var range in rangeIndicators)
        {
            ObjectPool.Instance.UnSpawn(range.gameObject);
        }
        rangeIndicators.Clear();
    }

    public override bool GameUpdate()
    {
        if (!Dropped)
            return false;
        if (TrackTarget() || AcquireTarget())
        {
            RotateTowards();
            FireProjectile();
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
            if (!Target[i].Enemy.gameObject.activeSelf)
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
                foreach (int i in returnInt)
                {
                    Target.Add(targetList[i]);
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
        foreach (var indicator in rangeIndicators)
        {
            indicator.ShowSprite(show);
        }
    }
    private void GenerateRange()
    {
        if (rangeIndicators.Count > 0)
        {
            RecycleRanges();
        }
        List<Vector2> points = null;
        switch (RangeType)
        {
            case RangeType.Circle:
                points = StaticData.GetCirclePoints(AttackRange, ForbidRange);
                break;
            case RangeType.HalfCircle:
                points = StaticData.GetHalfCirclePoints(AttackRange, ForbidRange);
                break;
            case RangeType.Line:
                points = StaticData.GetLinePoints(AttackRange, ForbidRange);
                break;
        }
        foreach (Vector2 point in points)
        {
            GameObject rangeObj = ObjectPool.Instance.Spawn(rangeIndicator);
            rangeObj.transform.SetParent(rangeParent);
            rangeObj.transform.localPosition = point;
            rangeIndicators.Add(rangeObj.GetComponent<RangeIndicator>());
        }
        detectCollider.GenerateGeometry();
        ShowRange(ShowingRange);
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
        foreach (TargetPoint target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            bullet.transform.position = shootPoint.position;
            bullet.Initialize(this, target);
        }

    }

    public void ClearTurret()
    {
        if (Dropped)
        {
            TriggerPoloEffect(false);
        }
        AttackIntensify = 0;
        Quality = 1;
        Element = Element.Gold;
        RangeIntensify = 0;
        SpeedIntensify = 0;
        targetList.Clear();
        CriticalPercentage = 1.5f;
        Dropped = false;
        TargetCount = 1;
        DamageAnalysis = 0;
        GameManager.Instance.turrets.behaviors.Remove(this);//从列表移除
        ClearTurnIntensify();
        RecycleRanges();
    }

    public void ClearTurnIntensify()
    {
        TurnAdditionalAttack = 0;
        TurnAdditionalSpeed = 0;
    }

}
