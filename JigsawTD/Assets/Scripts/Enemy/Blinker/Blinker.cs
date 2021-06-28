using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : Enemy
{
    WaveSystem ws;
    BoardSystem bs;
    public override EnemyType EnemyType => EnemyType.Blinker;
    int blink=3;
    public override void Awake()
    {
        base.Awake();
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }
    public override bool GameUpdate()
    {
        if (currentHealth / MaxHealth < 0.75f&&blink>=3)
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

    private IEnumerator Blink()
    {
        yield return null;
        AnimatorStateInfo stateinfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateinfo.IsName("Exit")&&stateinfo.normalizedTime >= 0.95f)
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
                progress = 0;
                blink -= 1;
            }
            anim.Play("Default") ;
        }
    }

   private void BlinkAfterAnim()
    {
        anim.Play("Exit");
        StartCoroutine(Blink());
    }
}
