using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public const int enemyLayerMask = 1 << 11;


    protected float _rotSpeed = 5f;
    protected TargetPoint target;
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    [SerializeField] protected CompositeCollider2D detectCollider = default;
    [SerializeField] protected GameObject bulletPrefab = default;
    [SerializeField] GameObject rangeIndicator = default;
    [SerializeField] Transform rangeParent = default;
    [SerializeField] Transform rotTrans = default;
    [SerializeField] Transform shootPoint = default;

    List<TargetPoint> targetList = new List<TargetPoint>();

    float nextAttackTime;
    Quaternion look_Rotation;

    [Header("TurretAttribute")]
    float attackDamage = 1f;
    int attackRange = 1;
    float attackSpeed = 1f;
    float bulletSpeed = 1f;
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }



    public void InitializeTurret()
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
        List<Vector2> points = StaticData.GetRangePoints(AttackRange, transform.position);
        foreach (Vector2 point in points)
        {
            GameObject rangeObj = ObjectPool.Instance.Spawn(rangeIndicator);
            rangeObj.transform.SetParent(rangeParent);
            rangeObj.transform.position = point;
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
        if (angleCheck < 10)
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
        bullet.Initialize(target, AttackDamage);
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
