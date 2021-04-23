using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    Ground ground = default;

    [SerializeField]
    GameTile tilePrefab = default;
    float tileSize = 1f;
    Vector2Int size;
    List<GameTile> tiles;

    GameTileContentFactory tileContentFactory;
    Queue<GameTile> searchFrontier = new Queue<GameTile>();
    List<GameTile> shortestPath = new List<GameTile>();
    List<GameTile> spawnPoints = new List<GameTile>();

    bool showPaths = true;
    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (showPaths)
            {
                foreach (GameTile tile in shortestPath)
                {
                    tile.ShowPath();
                }
            }
            else
            {
                foreach (GameTile tile in shortestPath)
                {
                    tile.HidePath();
                }
            }
        }
    }


    public void Initialize(Vector2Int size, GameTileContentFactory contentFactory)
    {
        this.size = size;
        tileContentFactory = contentFactory;
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f) * tileSize;
        tiles = new List<GameTile>();
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = Instantiate(tilePrefab);
                tiles.Add(tile);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector2(x, y) * tileSize - offset;
                CorrectTileCoord(tile);
                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                    tile.IsAlternative = !tile.IsAlternative;
                //if (x > 0)
                //    GameTile.MakeLeftRightNeighbours(tile, tiles[i - 1]);
                //if (y > 0)
                //    GameTile.MakeUpDownNeighbours(tile, tiles[i - size.x]);
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }

        }
        foreach (GameTile tile in tiles)
        {
            //tile.GetNeighbours(tiles);
            tile.GetNeighbours2(tiles);

        }

        ToggleDestination(tiles[40]);
        ToggleSpawnPoint(tiles[3]);


    }

    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.SpawnPoint);
            spawnPoints.Add(tile);
            FindPaths();
        }
    }

    public GameTile GetSpawnPoint(int index)
    {
        return spawnPoints[index];
    }

    public void GetShortestPath()
    {
        if (shortestPath.Count > 0)
        {
            foreach (GameTile tile in shortestPath)
            {
                tile.HidePath();
            }
            shortestPath.Clear();
        }
        GameTile findTile = spawnPoints[0];
        while (findTile != null)
        {
            shortestPath.Add(findTile);
            findTile = findTile.NextTileOnPath;
        }
        foreach (GameTile tile in shortestPath)
        {
            if (ShowPaths)
                tile.ShowPath();
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
                //if (tile.IsAlternative)
                //{
                //    searchFrontier.Enqueue(tile.GrowPathUp());
                //    searchFrontier.Enqueue(tile.GrowPathDown());
                //    searchFrontier.Enqueue(tile.GrowPathLeft());
                //    searchFrontier.Enqueue(tile.GrowPathRight());
                //}
                //else
                //{
                //    searchFrontier.Enqueue(tile.GrowPathRight());
                //    searchFrontier.Enqueue(tile.GrowPathLeft());
                //    searchFrontier.Enqueue(tile.GrowPathDown());
                //    searchFrontier.Enqueue(tile.GrowPathUp());
                //}
                if (tile.IsAlternative)
                {
                    for (int i = 0; i < tile.NeighbourTiles.Length; i++)
                    {
                        searchFrontier.Enqueue(tile.GrowPathTo(tile.NeighbourTiles[i]));
                    }
                }
                else
                {
                    for (int i = tile.NeighbourTiles.Length - 1; i >= 0; i--)
                    {
                        searchFrontier.Enqueue(tile.GrowPathTo(tile.NeighbourTiles[i]));
                    }
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
        GetShortestPath();
        return true;
    }

    Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void CorrectTileCoord(GameTile tile)
    {
        Vector2 coord = tile.transform.localPosition;
        //coord = new Vector2(coord.x + tileSize / 2, coord.y + tileSize / 2);
        float newX = coord.x / tileSize;
        float newY = coord.y / tileSize;
        tile.OffsetCoord = new Vector2(newX, newY);
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


