using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }
    [SerializeField]
    Transform arrow = default;
    int distance;
    GameTile left, down, up, right, nextOnPath;

    public GameTile[] NeighbourTiles = new GameTile[4];//0=up,1=left,2=right,3=down
    public GameTile NextTileOnPath => nextOnPath;

    bool isAlternative = true;
    public bool IsAlternative
    {
        get
        {
            bool alter = ((int)OffsetCoord.y & 1) == 0;
            if (((int)OffsetCoord.x & 1) == 0)
            {
                return alter ? isAlternative : !isAlternative;
            }
            else
            {
                return alter ? !isAlternative : isAlternative;
            }

        }
        //set;
    }
    public bool HasPath => distance != int.MaxValue;

    Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    GameTileContent content;
    public GameTileContent Content
    {
        get => content;
        set
        {
            Debug.Assert(value != null, "Null Content Assigned");
            if (content != null)
                content.Recycle();
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }


    static Quaternion
        upRotation = Quaternion.Euler(0f, 0f, 0f),
        downRoatation = Quaternion.Euler(0f, 0f, 180f),
        leftRotation = Quaternion.Euler(0f, 0f, 270f),
        rightRoatation = Quaternion.Euler(0f, 0f, 90f);


    //public static void MakeUpDownNeighbours(GameTile up, GameTile down)
    //{
    //    //Debug.Assert(up.down == null && down.up == null, "Redefined Neighbours!");
    //    up.down = down;
    //    down.up = up;
    //}

    //public static void MakeLeftRightNeighbours(GameTile left, GameTile right)
    //{
    //    //Debug.Assert(left.right == null && right.left == null, "Redefined Neighbours!");
    //    left.right = right;
    //    right.left = left;
    //}


    //public void GetNeighbours(List<GameTile> tiles)
    //{
    //    foreach (Vector2 direction in directions)
    //    {
    //        var neighbour = tiles.Find(t => t.OffsetCoord == OffsetCoord + direction);
    //        if (neighbour == null)
    //            continue;
    //        if (neighbour.OffsetCoord.x < OffsetCoord.x)
    //            MakeLeftRightNeighbours(this, neighbour);
    //        else if (neighbour.OffsetCoord.x > OffsetCoord.x)
    //            MakeLeftRightNeighbours(neighbour, this);
    //        else if (neighbour.OffsetCoord.y < OffsetCoord.y)
    //            MakeUpDownNeighbours(this, neighbour);
    //        else if (neighbour.OffsetCoord.y > OffsetCoord.y)
    //            MakeUpDownNeighbours(neighbour, this);
    //    }

    //}
    public void GetNeighbours2(List<GameTile> tiles)
    {
        for (int i = 0; i < NeighbourTiles.Length; i++)
        {
            if (NeighbourTiles[i] == null)
            {
                var neighbour = tiles.Find(t => t.OffsetCoord == OffsetCoord + DirectionExtensions.NormalizeDistance[i]);
                if (neighbour != null)
                {
                    Debug.Assert(neighbour.NeighbourTiles[3 - i] == null, "Already Assign Neighbour!");
                    neighbour.NeighbourTiles[3 - i] = this;
                    NeighbourTiles[i] = neighbour;
                }
            }
        }
    }

    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
    }

    //public GameTile GrowPathUp() => GrowPathTo(up);
    //public GameTile GrowPathDown() => GrowPathTo(down);
    //public GameTile GrowPathLeft() => GrowPathTo(left);
    //public GameTile GrowPathRight() => GrowPathTo(right);

    public GameTile GrowPathTo(GameTile neighbour, int directionIndex)
    {
        Debug.Assert(HasPath, "No Path!");
        if (neighbour == null || neighbour.HasPath)
            return null;
        neighbour.distance = distance + 1;
        neighbour.nextOnPath = this;
        neighbour.PathDirection = DirectionExtensions.GetDirection(3 - directionIndex);
        return (neighbour.Content.Type != GameTileContentType.Turret && neighbour.Content.Type != GameTileContentType.Rock) ? neighbour : null;
    }

    public void ShowPath()
    {
        if (Content.Type == GameTileContentType.SpawnPoint || Content.Type == GameTileContentType.Destination)
        {
            arrow.gameObject.SetActive(false);
            return;
        }
        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == NeighbourTiles[0] ? upRotation :
            nextOnPath == NeighbourTiles[2] ? leftRotation :
            nextOnPath == NeighbourTiles[1] ? rightRoatation :
            downRoatation;
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }


}
