using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up,
    down,
    left,
    right
}

public static class DirectionExtensions
{
    public static readonly Vector2[] NormalizeDistance =
    {
        new Vector2(0,1),new Vector2(-1,0),new Vector2(1,0),new Vector2(0,-1)
    };

    public static Vector2 GetDirectionPos(int directionIndex)
    {
        return NormalizeDistance[directionIndex] * StaticData.Instance.TileSize;
    }

    public static Direction GetDirection(int directionIndex)
    {
        switch (directionIndex)
        {
            case 0: return Direction.up;
            case 1: return Direction.left;
            case 2: return Direction.right;
            case 3: return Direction.down;
        }
        return Direction.up;
    }

}




