using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkTrap : TrapContent
{

    public override void OnPassOnce(Enemy enemy)
    {
        enemy.PointIndex -= 4*(int)enemy.TrapIntentify;
        if (enemy.PointIndex < 0)
        {
            enemy.PointIndex = 0;
        }
        enemy.CurrentPoint = enemy.PathPoints[enemy.PointIndex];
        enemy.transform.localPosition = enemy.PathPoints[enemy.PointIndex].PathPos;
        enemy.PositionFrom = enemy.CurrentPoint.PathPos;
        enemy.PositionTo = enemy.CurrentPoint.ExitPoint;
        enemy.Direction = enemy.CurrentPoint.PathDirection;
        enemy.DirectionChange = DirectionChange.None;
        enemy.model.localPosition = new Vector3(enemy.PathOffset, 0);
        enemy.DirectionAngleFrom = enemy.DirectionAngleTo = enemy.Direction.GetAngle();
        enemy.transform.localRotation = enemy.CurrentPoint.PathDirection.GetRotation();
        enemy.Progress = 0.5f;
        base.OnPassOnce(enemy);
    }

}
