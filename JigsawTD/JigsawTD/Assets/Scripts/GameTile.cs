using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    Transform arrow = default;
    int distance;
    GameTile left, down, up, right, nextOnPath;
    public GameTile NextTileOnPath => nextOnPath;
    public bool IsAlternative { get; set; }
    public bool HasPath => distance != int.MaxValue;
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


    public static void MakeUpDownNeighbours(GameTile up, GameTile down)
    {
        Debug.Assert(up.down == null && down.up == null, "Redefined Neighbours!");
        up.down = down;
        down.up = up;
    }

    public static void MakeLeftRightNeighbours(GameTile left, GameTile right)
    {
        Debug.Assert(left.right == null && right.left == null, "Redefined Neighbours!");
        left.right = right;
        right.left = left;
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

    public GameTile GrowPathUp() => GrowPathTo(up);
    public GameTile GrowPathDown() => GrowPathTo(down);
    public GameTile GrowPathLeft() => GrowPathTo(left);
    public GameTile GrowPathRight() => GrowPathTo(right);

    GameTile GrowPathTo(GameTile neighbour)
    {
        Debug.Assert(HasPath, "No Path!");
        if (neighbour == null || neighbour.HasPath)
            return null;
        neighbour.distance = distance + 1;
        neighbour.nextOnPath = this;
        return neighbour.Content.Type != GameTileContentType.Turret || neighbour.Content.Type != GameTileContentType.Rock ? neighbour : null;
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
            nextOnPath == up ? upRotation :
            nextOnPath == left ? leftRotation :
            nextOnPath == right ? rightRoatation :
            downRoatation;
    }
}
