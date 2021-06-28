using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : DestructableObject
{
    protected List<PathPoint> pathPoints = new List<PathPoint>();
    protected int PointIndex = 0;
    Direction direction;
    public Direction Direction { get => direction; set => direction = value; }
    DirectionChange directionChange;
    public virtual DirectionChange DirectionChange { get => directionChange; set => directionChange = value; }

    [SerializeField] protected Transform model = default;
    public PathPoint CurrentPoint;
    protected Vector3 positionFrom, positionTo;
    protected float progress;
    protected float directionAngleFrom, directionAngleTo;
    protected float pathOffset = 0;
    // Start is called before the first frame update

    protected float speed = 0.8f;
    private float adjust;
    private float progressFactor;

    public virtual float Speed { get => speed; set => speed = value; }
    public float Adjust { get => adjust; set => adjust = value; }
    public float ProgressFactor { get => progressFactor; set => progressFactor = value; }

    public override bool GameUpdate()
    {
        progress += Time.deltaTime * ProgressFactor;
        while (progress >= 1f)
        {
            if (PointIndex == pathPoints.Count - 1)
            {
                SpawnOn(0);
                return true;
            }
            progress = 0;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    public void SpawnOn(int pointIndex, List<PathPoint> path = null)
    {
        if (path != null)
        {
            pathPoints = path;
        }
        if (pathPoints.Count <= 1)
        {
            Debug.Log("没有路可走");
        }
        PointIndex = pointIndex;
        CurrentPoint = pathPoints[PointIndex];
        progress = 0f;
        PrepareIntro();
    }

    protected virtual void PrepareIntro()
    {
        positionFrom = CurrentPoint.PathPos;
        positionTo = CurrentPoint.ExitPoint;
        Direction = CurrentPoint.PathDirection;
        DirectionChange = DirectionChange.None;
        model.localPosition = new Vector3(pathOffset, 0);
        directionAngleFrom = directionAngleTo = Direction.GetAngle();
        transform.localRotation = CurrentPoint.PathDirection.GetRotation();
        Adjust = 2f;
        ProgressFactor = Adjust * Speed;
    }

    protected virtual void PrepareOutro()
    {
        positionFrom = positionTo;
        positionTo = CurrentPoint.PathPos;
        DirectionChange = DirectionChange.None;
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = Direction.GetRotation();
        Adjust = 2f;
        ProgressFactor = Adjust * Speed;
    }

    protected virtual void PrepareNextState()
    {
        PointIndex++;
        CurrentPoint = pathPoints[PointIndex];
        if (PointIndex >= pathPoints.Count - 1)
        {
            PrepareOutro();
            return;
        }
        positionFrom = positionTo;
        positionTo = CurrentPoint.ExitPoint;
        DirectionChange = Direction.GetDirectionChangeTo(CurrentPoint.PathDirection);
        Direction = CurrentPoint.PathDirection;
        directionAngleFrom = directionAngleTo;

        switch (DirectionChange)
        {
            case DirectionChange.None:
                PrepareForward();
                break;
            case DirectionChange.TurnRight:
                PrepareTurnRight();
                break;
            case DirectionChange.TurnLeft:
                PrepareTurnLeft();
                break;
            case DirectionChange.TurnAround:
                PrepareTurnAround();
                break;
        }

    }

    void PrepareForward()
    {
        transform.localRotation = Direction.GetRotation();
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        Adjust = 1f;
        ProgressFactor = Adjust * Speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        Adjust = 1 / (Mathf.PI * 0.5f * (0.5f - pathOffset));
        ProgressFactor = Adjust * Speed;
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        Adjust = 1 / (Mathf.PI * 0.5f * (0.5f + pathOffset));
        ProgressFactor = Adjust * Speed;
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localPosition = positionFrom;
        Adjust = 1 / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
        ProgressFactor = Adjust * Speed;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        model.localPosition = Vector3.zero;
    }
}
