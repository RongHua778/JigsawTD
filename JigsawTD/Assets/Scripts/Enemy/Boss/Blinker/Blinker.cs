using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinker : Boss
{
    public override string ExplosionEffect => "BossExplosionPurple";


    int blink;
    [SerializeField] private ReusableObject holePrefab = default;
    bool transfering = false;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset,intensify);
        transfering = false;
        blink = 3;
    }

    protected override void OnEnemyUpdate()
    {
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.75f && blink >= 3)
        {
            BlinkAfterAnim();

        }
        else if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.5f && blink >= 2)
        {
            BlinkAfterAnim();

        }
        else if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth < 0.25f && blink >= 1)
        {
            BlinkAfterAnim();

        }
    }

    void Blink()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.normalizedTime >= 0.98f/*&&stateinfo.normalizedTime<=1f*/)
        {
            PointIndex += 3;

            //在终点前不会瞬移
            if (PointIndex < PathPoints.Count - 1)
            {
                CurrentPoint = PathPoints[PointIndex];
                transform.localPosition = PathPoints[PointIndex].PathPos;
                PositionFrom = CurrentPoint.PathPos;
                PositionTo = CurrentPoint.ExitPoint;
                Direction = CurrentPoint.PathDirection;
                DirectionChange = DirectionChange.None;
                model.localPosition = new Vector3(PathOffset, 0);
                DirectionAngleFrom = DirectionAngleTo = Direction.GetAngle();
                transform.localRotation = CurrentPoint.PathDirection.GetRotation();
                Progress = 0;
            }
            else
            {
                PointIndex = PathPoints.Count - 1;
                CurrentPoint = PathPoints[PointIndex];
                transform.localPosition = PathPoints[PointIndex].PathPos;
                Progress = 1;
            }
            transfering = false;
            blink -= 1;
            anim.Play("Default");
        }
    }


    private void BlinkAfterAnim()
    {
        Vector3 targetPos;
        if (!transfering)
        {
            targetPos = PathPoints[Mathf.Min(PointIndex + 3,PathPoints.Count - 1)].PathPos;
            SpawnHoleOnPos(transform.position);
            SpawnHoleOnPos(targetPos);
            anim.Play("Exit");
            StunTime += 0.5f;
            transfering = true;
        }
        Blink();
    }

    private void SpawnHoleOnPos(Vector3 pos)
    {
        ReusableObject hole = ObjectPool.Instance.Spawn(holePrefab);
        hole.transform.position = pos;
        hole.transform.localScale = Vector3.one * 0.1f;
        hole.transform.DOScale(Vector3.one * 0.3f, 1f);
        hole.UnspawnAfterTime(1f);
    }

}
