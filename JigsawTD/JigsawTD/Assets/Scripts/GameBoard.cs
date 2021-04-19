using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Ground ground = default;

    [SerializeField]
    GameTile tilePrefab = default;
    Vector2Int size;
    List<GameTile> tiles;

    GameTileContentFactory tileContentFactory;
    Queue<GameTile> searchFrontier = new Queue<GameTile>();



    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        this.size = size;
        tileContentFactory = contentFactory;
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        tiles = new List<GameTile>();
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = Instantiate(tilePrefab);
                tiles.Add(tile);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector2(x - offset.x, y - offset.y);
                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                    tile.IsAlternative = !tile.IsAlternative;
                if (x > 0)
                    GameTile.MakeLeftRightNeighbours(tile, tiles[i - 1]);
                if (y > 0)
                    GameTile.MakeUpDownNeighbours(tile, tiles[i - size.x]);
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }

        }

        ToggleSpawnPoint(tiles[11]);
        ToggleDestination(tiles[13]);

    }

    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.SpawnPoint);
        }
    }

    public void ToggleTurret(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty || tile.Content.Type == GameTileContentType.Rock)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Turret);
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Turret)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
    }

    public void ToggleRock(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty || tile.Content.Type == GameTileContentType.Turret)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Rock);
            FindPaths();
        }
        else if (tile.Content.Type == GameTileContentType.Rock)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
    }

    bool FindPaths()
    {
        foreach (GameTile tile in tiles)
        {
            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                searchFrontier.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }
        if (searchFrontier.Count == 0)
            return false;
        while (searchFrontier.Count > 0)
        {
            GameTile tile = searchFrontier.Dequeue();
            if (tile != null)
            {
                if (tile.IsAlternative)
                {
                    searchFrontier.Enqueue(tile.GrowPathUp());
                    searchFrontier.Enqueue(tile.GrowPathDown());
                    searchFrontier.Enqueue(tile.GrowPathLeft());
                    searchFrontier.Enqueue(tile.GrowPathRight());
                }
                else
                {
                    searchFrontier.Enqueue(tile.GrowPathRight());
                    searchFrontier.Enqueue(tile.GrowPathLeft());
                    searchFrontier.Enqueue(tile.GrowPathDown());
                    searchFrontier.Enqueue(tile.GrowPathUp());
                }
            }
        }
        foreach (GameTile tile in tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }
        foreach (GameTile tile in tiles)
        {
            tile.ShowPath();
        }
        return true;
    }

    Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    public GameTile GetTile()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(MouseInWorldCoords(), Vector3.forward, Mathf.Infinity);
        if (hit.collider == null)
            return null;
        if (hit.collider.GetComponent<GameTile>() != null)
            return hit.collider.GetComponent<GameTile>();
        return null;
    }
}


