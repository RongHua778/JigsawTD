using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    protected RangeType RangeType;
    [SerializeField] protected GameObject bulletPrefab = default;
    public const int enemyLayerMask = 1 << 11;
    protected float _rotSpeed = 10f;
    protected TargetPoint target;
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    protected CompositeCollider2D detectCollider;
    GameObject rangeIndicator;
    Transform rangeParent;
    Transform rotTrans;
    protected Transform shootPoint;
    protected float CheckAngle = 10f;

    List<TargetPoint> targetList = new List<TargetPoint>();

    float nextAttackTime;
    Quaternion look_Rotation;

    [Header("TurretAttribute")]
    public TurretAttribute m_TurretAttribute = default;
    protected int Level = 0;
    public float AttackDamage { get => m_TurretAttribute.TurretLevels[Level].AttackDamage; }
    public int AttackRange { get => m_TurretAttribute.TurretLevels[Level].AttackRange; }
    public float AttackSpeed { get => m_TurretAttribute.TurretLevels[Level].AttackSpeed; }
    public float BulletSpeed { get => m_TurretAttribute.TurretLevels[Level].BulletSpeed; }

    private void Awake()
    {
        rangeIndicator = Resources.Load<GameObject>("Prefabs/RangeIndicator");
        rangeParent = transform.Find("TurretRangeCol");
        detectCollider = rangeParent.GetComponent<CompositeCollider2D>();
        rotTrans = transform.Find("RotPoint");
        shootPoint = rotTrans.Find("ShootPoint");
        RangeType = m_TurretAttribute.RangeType;
    }

    public virtual void InitializeTurret()
    {
        GenerateRange();
    }

    public void AddTarget(TargetPoint target)
    {
        targetList.Add(target);
    }

    public void RemoveTarget(TargetPoint target)
    {
        if (targetList.Contains(target))
        {
            if (this.target == target)
            {
                this.target = null;
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

    public virtual void GameUpdate()
    {
        if (TrackTarget() || AcquireTarget())
        {
            RotateTowards();
            FireProjectile();
        }
    }

    private bool TrackTarget()
    {
        if (target == null)
        {
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
            target = targetList[UnityEngine.Random.Range(0, targetList.Count - 1)];
            return false;
        }
    }

    public void ShowRange(bool show)
    {
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
                points = StaticData.GetCirclePoints(AttackRange);
                break;
            case RangeType.HalfCircle:
                break;
            case RangeType.Line:
                points = StaticData.GetLinePoints(AttackRange);
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
    }

    protected virtual void RotateTowards()
    {
        if (target == null)
            return;
        var dir = target.transform.position - rotTrans.position;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        look_Rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rotTrans.rotation = Quaternion.Lerp(rotTrans.rotation, look_Rotation, _rotSpeed * Time.deltaTime);
    }

    private bool AngleCheck()
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
            if (target != null && AngleCheck())
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
        bullet.Initialize(target,target.Position, AttackDamage, BulletSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.z -= 0.1f;
        if (target != null)
        {
            Gizmos.DrawLine(position, target.transform.position);
        }
    }
}
