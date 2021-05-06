using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    public const int enemyLayerMask = 1 << 11;

    protected float _rotSpeed = 15f;
    protected TargetPoint target;
    protected List<RangeIndicator> rangeIndicators = new List<RangeIndicator>();
    [SerializeField] protected CompositeCollider2D detectCollider;
    [SerializeField] GameObject rangeIndicator = default;
    [SerializeField] Transform rangeParent = default;

    ContactFilter2D filter = new ContactFilter2D();
    static Collider2D[] targetBuffers = new Collider2D[10];


    float nextAttackTime;

    [Header("TurretAttribute")]
    float attackDamage;
    int attackRange = 1;
    float attackSpeed;
    float bulletSpeed;
    public float AttackDamage { get => attackDamage; set => attackDamage = value; }
    public int AttackRange { get => attackRange; set => attackRange = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }

    private void Awake()
    {
        filter.useTriggers = true;
        filter.SetLayerMask(enemyLayerMask);
    }

    public void InitializeTurret()
    {
        GenerateRange();
    }

    public void RecycleRanges()
    {
        foreach(var range in rangeIndicators)
        {
            ObjectPool.Instance.UnSpawn(range.gameObject);
        }
        rangeIndicators.Clear();
    }

    public virtual void GameUpdate()
    {
        if (TrackTarget() || AcquireTarget())
        {

        }
    }

    private bool TrackTarget()
    {
        if (target == null)
        {
            return false;
        }
        Vector2 a = transform.position;
        Vector2 b = target.transform.position;
        if ((a - b).magnitude > AttackRange+0.5f)
        {
            target = null;
            return false;
        }
        
        return true;
    }
    private bool AcquireTarget()
    {
        int hits = Physics2D.OverlapCollider(detectCollider, filter, targetBuffers);
        if (hits > 0)
        {
            Debug.Log("hit");
            target = targetBuffers[0].GetComponent<TargetPoint>();
            return true;
        }
        target = null;
        return false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        position.z -= 0.1f;
        Gizmos.DrawWireSphere(position, AttackRange);
        if (target != null)
        {
            Gizmos.DrawLine(position, target.transform.position);
        }
    }
}
