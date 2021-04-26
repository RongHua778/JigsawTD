using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameTile : MonoBehaviour
{
    public int TileID;
    SpriteRenderer tileTypeSr;
    Transform arrow;

    Direction pathDirection;
    public Direction PathDirection { get => pathDirection; set => pathDirection = value; }
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

    private void Awake()
    {
        tileTypeSr = transform.Find("TileType").GetComponent<SpriteRenderer>();
        arrow = transform.Find("PathArrow");
    }

    public void SetPreviewing(bool isPreviewing)
    {
        if (isPreviewing)
        {
            tileTypeSr.sortingOrder = 5;
            GetComponent<SpriteRenderer>().sortingOrder = 4;
            GetComponent<Collider2D>().enabled = false;

        }
        else
        {
            tileTypeSr.sortingOrder = 2;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            GetComponent<Collider2D>().enabled = true;
        }
    }

    public virtual void OnTilePass()
    {

    }

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
        arrow.rotation = DirectionExtensions.GetRotation((int)PathDirection);
    }

    public void HidePath()
    {
        arrow.gameObject.SetActive(false);
    }


}
