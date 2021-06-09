using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowerOld : ReusableObject,IGameBehavior
{
    //public List<PathPoint> pathPoints = new List<PathPoint>();

    Direction direction;
    public Direction Direction { get => direction; set => direction = value; }
    DirectionChange directionChange;
    public virtual DirectionChange DirectionChange { get => directionChange; set => directionChange = value; }

    public GameTile SpawnPoint { get; set; }
    [SerializeField] protected Transform model = default;
    [HideInInspector] public GameTile tileFrom, tileTo;
    protected Vector3 positionFrom, positionTo;
    protected float progress, progressFactor, adjust;
    protected float directionAngleFrom, directionAngleTo;
    protected float pathOffset;
    // Start is called before the first frame update

    protected float speed = 0.8f;
    public virtual float Speed { get => speed; set => speed = value; }
    public virtual bool GameUpdate()
    {
        progress += Time.deltaTime * progressFactor;
        while (progress >= 1f)
        {
            if (tileTo == null)
            {
                //SpawnOn(SpawnPoint);
                return true;
            }
            progress = 0;
            //PrepareNextState();
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

    //public void SpawnOn(GameTile tile)
    //{
    //    Debug.Assert(tile.NextTileOnPath != null, "No where to go", this);
    //    tileFrom = tile;
    //    tileTo = tileFrom.NextTileOnPath;
    //    progress = 0f;
    //    PrepareIntro();
    //}

    //protected virtual void PrepareIntro()
    //{
    //    positionFrom = tileFrom.transform.position;
    //    positionTo = tileFrom.ExitPoint;
    //    Direction = tileFrom.PathDirection;
    //    DirectionChange = DirectionChange.None;
    //    model.localPosition = new Vector3(pathOffset, 0);
    //    directionAngleFrom = directionAngleTo = Direction.GetAngle();
    //    transform.localRotation = tileFrom.PathDirection.GetRotation();
    //    adjust = 2f;
    //    progressFactor = adjust * Speed;
    //}

    protected virtual void PrepareOutro()
    {
        positionTo = tileFrom.transform.position;
        DirectionChange = DirectionChange.None;
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localRotation = Direction.GetRotation();
        adjust = 2f;
        progressFactor = adjust * Speed;
    }

    //protected virtual void PrepareNextState()
    //{
    //    tileFrom = tileTo;
    //    tileTo = tileTo.NextTileOnPath;
    //    positionFrom = positionTo;
    //    if (tileTo == null)
    //    {
    //        PrepareOutro();
    //        return;
    //    }
    //    positionTo = tileFrom.ExitPoint;
    //    DirectionChange = Direction.GetDirectionChangeTo(tileFrom.PathDirection);
    //    Direction = tileFrom.PathDirection;
    //    directionAngleFrom = directionAngleTo;

    //    switch (DirectionChange)
    //    {
    //        case DirectionChange.None:
    //            PrepareForward();
    //            break;
    //        case DirectionChange.TurnRight:
    //            PrepareTurnRight();
    //            break;
    //        case DirectionChange.TurnLeft:
    //            PrepareTurnLeft();
    //            break;
    //        case DirectionChange.TurnAround:
    //            PrepareTurnAround();
    //            break;
    //    }

    //}

    void PrepareForward()
    {
        transform.localRotation = Direction.GetRotation();
        directionAngleTo = Direction.GetAngle();
        model.localPosition = new Vector3(pathOffset, 0f);
        adjust = 1f;
        progressFactor = adjust * Speed;
    }

    void PrepareTurnRight()
    {
        directionAngleTo = directionAngleFrom - 90f;
        model.localPosition = new Vector3(pathOffset - 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        adjust = 1 / (Mathf.PI * 0.5f * (0.5f - pathOffset));
        progressFactor = adjust * Speed;
    }
    void PrepareTurnLeft()
    {
        directionAngleTo = directionAngleFrom + 90f;
        model.localPosition = new Vector3(pathOffset + 0.5f, 0f);
        transform.localPosition = positionFrom + Direction.GetHalfVector();
        adjust = 1 / (Mathf.PI * 0.5f * (0.5f + pathOffset));
        progressFactor = adjust * Speed;
    }
    void PrepareTurnAround()
    {
        directionAngleTo = directionAngleFrom + (pathOffset < 0f ? 180f : -180f);
        model.localPosition = new Vector3(pathOffset, 0);
        transform.localPosition = positionFrom;
        adjust = 1 / (Mathf.PI * Mathf.Max(Mathf.Abs(pathOffset), 0.2f));
        progressFactor = adjust * Speed;
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        model.localPosition = Vector3.zero;
    }
}
