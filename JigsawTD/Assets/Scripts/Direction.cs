using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up, right, down, left
}
public enum DirectionChange
{
    None, TurnRight, TurnLeft, TurnAround
}

public static class DirectionExtensions
{
    public static readonly Vector2[] NormalizeDistance =
    {
        new Vector2(0,1),new Vector2(1,0),new Vector2(0,-1),new Vector2(-1,0)
    };


    static Quaternion[] Rotations =
    {
        Quaternion.Euler(0f, 0f, 0f),
        Quaternion.Euler(0f, 0f, 270f),
        Quaternion.Euler(0f, 0f, 180f),
        Quaternion.Euler(0f, 0f, 90f)
    };

    static Vector3[] halfVectors =
{
        Vector3.up * 0.5f,
        Vector3.right * 0.5f,
        Vector3.down * 0.5f,
        Vector3.left * 0.5f
    };

    public static Vector3 GetHalfVector(this Direction direction)
    {
        return halfVectors[(int)direction];
    }
    public static DirectionChange GetDirectionChangeTo(this Direction current, Direction next)
    {
        if (current == next)
        {
            return DirectionChange.None;
        }
        else if (current + 1 == next || current - 3 == next)
        {
            return DirectionChange.TurnRight;
        }
        else if (current - 1 == next || current + 3 == next)
        {
            return DirectionChange.TurnLeft;
        }
        return DirectionChange.TurnAround;
    }

    public static float GetAngle(this Direction direction)
    {
        switch (direction)
        {
            case Direction.up:
                return 0f;
            case Direction.right:
                return 270f;
            case Direction.down:
                return 180f;
            case Direction.left:
                return 90f;
        }
        return 0f;
    }

    public static Vector2 GetDirectionPos(this Direction direction)
    {
        return NormalizeDistance[(int)direction] * StaticData.Instance.TileSize;
    }

    public static Quaternion GetRandomRotation()
    {
        return Rotations[Random.Range(0, Rotations.Length)];
    }

    public static Quaternion GetRotation(this Direction direction)
    {
        return Rotations[(int)direction];
    }

    public static Direction GetDirection(int index)
    {
        return (Direction)index;
    }


}




