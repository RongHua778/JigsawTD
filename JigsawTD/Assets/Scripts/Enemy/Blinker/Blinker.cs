using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blinker : Enemy
{
    WaveSystem ws;
    BoardSystem bs;
    [SerializeField] ReusableObject holePrefab = default;
    public override EnemyType EnemyType => EnemyType.Blinker;
    int blink = 3;
    bool transfering = false;
    public override void Awake()
    {
        base.Awake();
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }
    public override bool GameUpdate()
    {
        if (currentHealth / MaxHealth < 0.75f && blink >= 3)
        {
            BlinkAfterAnim();

        }
        else if (currentHealth / MaxHealth < 0.5f && blink >= 2)
        {
            BlinkAfterAnim();

        }
        else if (currentHealth / MaxHealth < 0.25f && blink >= 1)
        {
            BlinkAfterAnim();

        }
        return base.GameUpdate();
    }
    private bool reachEnd = false;
    private void Blink()
    {
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Exit") && stateinfo.normalizedTime >= 0.95f)
        {
            PointIndex += 4;

            //在终点前不会瞬移
            if (PointIndex < pathPoints.Count - 1)
            {
                CurrentPoint = pathPoints[PointIndex];
                transform.localPosition = pathPoints[PointIndex].PathPos;
                positionFrom = CurrentPoint.PathPos;
                positionTo = CurrentPoint.ExitPoint;
                Direction = CurrentPoint.PathDirection;
                DirectionChange = DirectionChange.None;
                model.localPosition = new Vector3(pathOffset, 0);
                directionAngleFrom = directionAngleTo = Direction.GetAngle();
                transform.localRotation = CurrentPoint.PathDirection.GetRotation();
                Progress = 0;
                blink -= 1;
            }
            else
            {
                PointIndex = pathPoints.Count - 1;
                CurrentPoint = pathPoints[PointIndex];
                transform.localPosition = pathPoints[PointIndex].PathPos;
                Progress = 1;
            }
            transfering = false;
            anim.Play("Default");
        }



    }


    private void BlinkAfterAnim()
    {
        Vector3 targetPos;
        if (!transfering)
        {

            targetPos = pathPoints[Mathf.Min(PointIndex + 4, pathPoints.Count - 1)].PathPos;
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

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        blink = 3;
        transfering = false;
    }
}
