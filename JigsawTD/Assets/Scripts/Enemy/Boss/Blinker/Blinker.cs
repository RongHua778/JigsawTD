using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinker : Enemy
{
    int blink;
    [SerializeField] private ReusableObject holePrefab = default;
    bool transfering = false;
    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify,path);
        //EnemySkills = new List<Skill>();
        //EnemySkills.Add(GameManager.Instance.SkillFactory.GetSkill(EnemySkill.Blink, this));
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
        AnimatorStateInfo stateinfo = Anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.normalizedTime >= 0.98f/*&&stateinfo.normalizedTime<=1f*/)
        {
            PointIndex += 4;

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
            Anim.Play("Default");
        }
    }


    private void BlinkAfterAnim()
    {
        Vector3 targetPos;
        if (!transfering)
        {
            targetPos = PathPoints[Mathf.Min(PointIndex + 4,PathPoints.Count - 1)].PathPos;
            SpawnHoleOnPos(transform.position);
            SpawnHoleOnPos(targetPos);
            Anim.Play("Exit");
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
