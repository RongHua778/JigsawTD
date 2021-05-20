using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : GameBehavior
{
    protected RangeType RangeType;
    [SerializeField] protected GameObject bulletPrefab = default;
    public const int enemyLayerMask = 1 << 11;
    protected float _rotSpeed = 10f;
    private TargetPoint target;
    public TargetPoint Target { get => target; set => target = value; }
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    protected CompositeCollider2D detectCollider;
    GameObject rangeIndicator;
    Transform rangeParent;


    protected bool ShowingRange = false;
    protected Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    public bool Dropped = false;

    public List<TargetPoint> targetList = new List<TargetPoint>();

    float nextAttackTime;
    Quaternion look_Rotation;

    [Header("TurretAttribute")]
    public TurretAttribute m_TurretAttribute = default;
    public int Level = 0;
    public virtual float AttackDamage { get => m_TurretAttribute.TurretLevels[Level].AttackDamage *(1+ AttackIntensify); }
    public virtual int AttackRange { get => m_TurretAttribute.TurretLevels[Level].AttackRange + RangeIntensify; }
    public int ForbidRange { get => m_TurretAttribute.TurretLevels[Level].ForbidRange; }
    public virtual float AttackSpeed { get => m_TurretAttribute.TurretLevels[Level].AttackSpeed * (1 + SpeedIntensify); }
    public float BulletSpeed { get => m_TurretAttribute.TurretLevels[Level].BulletSpeed; }
    public float SputteringRange { get => m_TurretAttribute.TurretLevels[Level].SputteringRange; }
    public float CriticalRate { get => m_TurretAttribute.TurretLevels[Level].CriticalRate; }

    float attackIntensify;
    public float AttackIntensify { get => attackIntensify; set => attackIntensify = value; }
    int rangeIntensify;
    public int RangeIntensify { get => rangeIntensify; 
        set 
        { 
            rangeIntensify = value; 
            GenerateRange(); 
        } 
    }

    float speedIntensify;
    public float SpeedIntensify { get => speedIntensify; set => speedIntensify = value; }


    public List<AttackEffectInfo> AttackEffectInfos => m_TurretAttribute.TurretLevels[Level].AttackEffects;


    private void Awake()
    {
        rangeIndicator = Resources.Load<GameObject>("Prefabs/RangeIndicator");
        rangeParent = transform.Find("TurretRangeCol");
        detectCollider = rangeParent.GetComponent<CompositeCollider2D>();
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        RangeType = m_TurretAttribute.RangeType;
    }

    public virtual void InitializeTurret(GameTile tile)
    {
        GenerateRange();
        rotTrans.localRotation = Quaternion.identity;
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
}
