using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : GameBehavior
{
    List<Composition> compositions = new List<Composition>();
    public List<Composition> Compositions { get => compositions; set => compositions = value; }

    private GameObject rangeIndicator;
    private Transform rangeParent;
    private float nextAttackTime;
    private Quaternion look_Rotation;

    protected RangeType RangeType;
    [SerializeField] protected GameObject bulletPrefab = default;
    protected float _rotSpeed = 10f;
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    protected CompositeCollider2D detectCollider;
    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    [HideInInspector] public bool Dropped = false;

    public List<TargetPoint> targetList = new List<TargetPoint>();


    //[Header("美术资源设置")]
    private SpriteRenderer BaseSprite;
    private SpriteRenderer CannonSprite;
    //************

    [Header("TurretAttribute")]
    public int Level = 0;
    public TurretAttribute m_TurretAttribute = default;
    //塔的品质
    private int quality = 1;
    //塔的元素属性
    private Element element;
    //查看塔的状态（如是否购买了其蓝图，是否集齐了蓝图上面的配方）
    public TurretStatus Status { get; set; }
    public int Quality
    {
        get => quality;
        set
        {
            quality = value;
        }
    }
    public Element Element { get => element; set => element = value; }
    public virtual float AttackDamage { get => (m_TurretAttribute.TurretLevels[Quality - 1].AttackDamage + TurnAdditionalAttack) * (1 + AttackIntensify); }
    public virtual int AttackRange { get => m_TurretAttribute.TurretLevels[Quality - 1].AttackRange + RangeIntensify; }
    public int ForbidRange { get => m_TurretAttribute.TurretLevels[Quality - 1].ForbidRange; }
    public virtual float AttackSpeed { get => m_TurretAttribute.TurretLevels[Quality - 1].AttackSpeed * (1 + SpeedIntensify); }
    public float BulletSpeed { get => m_TurretAttribute.TurretLevels[Quality - 1].BulletSpeed; }
    public float SputteringRange { get => m_TurretAttribute.TurretLevels[Quality - 1].SputteringRange; }
    public float CriticalRate { get => m_TurretAttribute.TurretLevels[Quality - 1].CriticalRate; }


    //**************回合临时属性
    int turnAddtionalAttack = 0;
    public int TurnAdditionalAttack { get => turnAddtionalAttack; set => turnAddtionalAttack = value; }
    //*************

    //*************光环加成
    float attackIntensify;
    public float AttackIntensify { get => attackIntensify; set => attackIntensify = value; }
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
    public float SpeedIntensify { get => speedIntensify; set => speedIntensify = value; }
    //*************

    public List<TurretEffectInfo> AttackEffectInfos => m_TurretAttribute.TurretLevels[Level].AttackEffects;
    public List<TurretEffect> AttackEffects = new List<TurretEffect>();


    private void Awake()
    {
        rangeIndicator = Resources.Load<GameObject>("Prefabs/RangeIndicator");
        rangeParent = transform.Find("TurretRangeCol");
        detectCollider = rangeParent.GetComponent<CompositeCollider2D>();
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        BaseSprite = transform.root.Find("TileBase/TurretBase").GetComponent<SpriteRenderer>();
        CannonSprite = rotTrans.Find("Cannon").GetComponent<SpriteRenderer>();
        RangeType = m_TurretAttribute.RangeType;
        Element = m_TurretAttribute.element;
    }

    public virtual void InitializeTurret(GameTile tile, int quality)
    {
        GenerateRange();
        rotTrans.localRotation = Quaternion.identity;
        this.quality = quality;
        SetGraphic();
    }

    //设置不同等级的美术资源
    public void SetGraphic()
    {
        shootPoint.transform.localPosition = m_TurretAttribute.TurretLevels[quality - 1].ShootPointOffset;
        BaseSprite.sprite = m_TurretAttribute.TurretLevels[quality - 1].BaseSprite;
        CannonSprite.sprite = m_TurretAttribute.TurretLevels[quality - 1].CannonSprite;
    }

    public virtual void TriggerPoloEffect(bool value)
    {
        if (m_TurretAttribute.TurretLevels[Level].PoloEffects.Count > 0)
        {
            List<Vector2> poss = StaticData.GetCirclePoints(AttackRange, ForbidRange);
            foreach (var polo in m_TurretAttribute.TurretLevels[Level].PoloEffects)
            {
                switch (polo.EffectType)
                {
                    case PoloEffectType.RangeIntensify:
                        foreach (var pos in poss)
                        {
                            GroundTile groungTile = GameBoard.GetTile(pos + (Vector2)transform.position, StaticData.GetGroundLayer) as GroundTile;
                            groungTile.RangeIntensify += value ? (int)polo.KeyValue : -(int)polo.KeyValue;
                            groungTile.TriggerIntensify();
                        }
                        break;
                    case PoloEffectType.AttackIntensify:
                        foreach (var pos in poss)
                        {
                            GroundTile groungTile = GameBoard.GetTile(pos + (Vector2)transform.position, StaticData.GetGroundLayer) as GroundTile;
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
    }

    public virtual void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.Target == target)
            {
                this.Target = null;
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
        if (Target == null)
        {
            return false;
        }
        if (!Target.Enemy.gameObject.activeSelf)
        {
            targetList.Remove(Target);
            Target = null;
            return false;
        }
        return true;
    }
    private bool AcquireTarget()
    {
        if (targetList.Count <= 0)
            return false;
        else
        {
            Target = targetList[UnityEngine.Random.Range(0, targetList.Count - 1)];
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
        if (Target == null)
            return;
        var dir = Target.transform.position - rotTrans.position;
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
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = shootPoint.position;
        bullet.Initialize(this);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.z -= 0.1f;
        if (Target != null)
        {
            Gizmos.DrawLine(position, Target.transform.position);
        }
    }
    //根据特定参数生成配方
    private void GenerateComposition()
    {
        int[] tempLevel = StaticData.GetSomeRandoms(m_TurretAttribute.totalLevel, m_TurretAttribute.elementNumber);
        int[] tempElement = new int[m_TurretAttribute.elementNumber];
        for (int i = 0; i < m_TurretAttribute.elementNumber; i++)
        {
            Composition c = new Composition(tempLevel[i], tempElement[i]);
            Compositions.Add(c);
        }
    }

    public void ClearTurnIntensify()
    {
        TurnAdditionalAttack = 0;
    }

}
