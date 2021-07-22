using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BlinkSkill : Skill
{
    int blink;
    ReusableObject holePrefab = default;
    bool transfering = false;
    public BlinkSkill(Enemy enemy, int blink, ReusableObject holePrefab)
    {
        this.enemy = enemy;
        this.blink = blink;
        this.holePrefab = holePrefab;
    }

    public override void OnBorn()
    {
        base.OnBorn();
        transfering = false;
        blink = 3;
    }
    //在enemy的gameupdate中调用
    public override void OnGameUpdating()
    {
        if (enemy.CurrentHealth / enemy.MaxHealth < 0.75f && blink >= 3)
        {
            BlinkAfterAnim();

        }
        else if (enemy.CurrentHealth / enemy.MaxHealth < 0.5f && blink >= 2)
        {
            BlinkAfterAnim();

        }
        else if (enemy.CurrentHealth / enemy.MaxHealth < 0.25f && blink >= 1)
        {
            BlinkAfterAnim();

        }
    }

    void Blink()
    {
        AnimatorStateInfo stateinfo = enemy.Anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.normalizedTime >= 0.98f/*&&stateinfo.normalizedTime<=1f*/)
        {
            enemy.PointIndex += 4;

            //在终点前不会瞬移
            if (enemy.PointIndex < enemy.PathPoints.Count - 1)
            {
                enemy.CurrentPoint = enemy.PathPoints[enemy.PointIndex];
                enemy.transform.localPosition = enemy.PathPoints[enemy.PointIndex].PathPos;
                enemy.PositionFrom = enemy.CurrentPoint.PathPos;
                enemy.PositionTo = enemy.CurrentPoint.ExitPoint;
                enemy.Direction = enemy.CurrentPoint.PathDirection;
                enemy.DirectionChange = DirectionChange.None;
                enemy.model.localPosition = new Vector3(enemy.PathOffset, 0);
                enemy.DirectionAngleFrom = enemy.DirectionAngleTo = enemy.Direction.GetAngle();
                enemy.transform.localRotation = enemy.CurrentPoint.PathDirection.GetRotation();
                enemy.Progress = 0;
            }
            else
            {
                enemy.PointIndex = enemy.PathPoints.Count - 1;
                enemy.CurrentPoint = enemy.PathPoints[enemy.PointIndex];
                enemy.transform.localPosition = enemy.PathPoints[enemy.PointIndex].PathPos;
                enemy.Progress = 1;
            }
            transfering = false;
            blink -= 1;
            enemy.Anim.Play("Default");
        }
    }


    private void BlinkAfterAnim()
    {
        Vector3 targetPos;
        if (!transfering)
        {
            targetPos = enemy.PathPoints[Mathf.Min(enemy.PointIndex + 4, enemy.PathPoints.Count - 1)].PathPos;
            SpawnHoleOnPos(enemy.transform.position);
            SpawnHoleOnPos(targetPos);
            enemy.Anim.Play("Exit");
            enemy.StunTime += 0.5f;
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
