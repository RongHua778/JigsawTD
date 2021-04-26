using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    GameObject tempTilePrefab = default;
    List<GameObject> tempTileList = new List<GameObject>();

    Vector2Int size;
    List<GameTile> tiles;

    TileFactory tileFactory;
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
    bool showTempTile = false;
    public bool ShowTempTile
    {
        get => showTempTile;
        set
        {
            showTempTile = value;
            if (showTempTile)
            {
                foreach (GameObject temp in tempTileList)
                {
                    temp.SetActive(true);
                }
            }
            else
            {
                foreach (GameObject temp in tempTileList)
                {
                    temp.SetActive(false);
                }
            }
        }
    }

    private void Start()
    {
        GameEvents.Instance.onAddTiles += AddTile;
    }

    public void Initialize(Vector2Int size, TileFactory tileFactory, GameTileContentFactory contentFactory)
    {
        this.size = size;
        this.tileFactory = tileFactory;
        tileContentFactory = contentFactory;
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f) * StaticData.Instance.TileSize;
        tiles = new List<GameTile>();
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = this.tileFactory.GetTile(0);
                tiles.Add(tile);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                CorrectTileCoord(tile);
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
            }

        }
        foreach (GameTile tile in tiles)
        {
            tile.GetNeighbours2(tiles);

        }
        foreach (GameTile tile in tiles)
        {
            GenerateTempTile(tile);
        }


        ToggleDestination(tiles[5]);
        ToggleSpawnPoint(tiles[3]);


    }

    private void GenerateTempTile(GameTile tile)
    {
        for (int i = 0; i < tile.NeighbourTiles.Length; i++)
        {
            if (tile.NeighbourTiles[i] == null)
            {
                GameObject tempTile = ObjectPool.Instance.Spawn(tempTilePrefab);
                tempTile.transform.position = (Vector2)tile.transform.position + DirectionExtensions.GetDirectionPos(i);
                tempTileList.Add(tempTile);
            }
        }
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

    private void AddTile(List<GameTile> newTiles)
    {
        foreach (GameTile tile in newTiles)
        {
            tiles.Add(tile);
            tile.SetPreviewing(false);
            tile.transform.SetParent(this.transform);
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            CorrectTileCoord(tile);
            var tempTile = tempTileList.Find(t => t.transform.position == tile.transform.position);
            if (tempTile != null)
            {
                tempTileList.Remove(tempTile);
                ObjectPool.Instance.UnSpawn(tempTile);
            }
        }
        foreach (GameTile tile in newTiles)
        {
            tile.GetNeighbours2(tiles);
        }
        foreach (GameTile tile in newTiles)
        {
            GenerateTempTile(tile);
        }
        FindPaths();
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
                    for (int i = 0; i < tile.NeighbourTiles.Length; i++)
                    {
                        searchFrontier.Enqueue(tile.GrowPathTo(tile.NeighbourTiles[i], i));
                    }
                }
                else
                {
                    for (int i = tile.NeighbourTiles.Length - 1; i >= 0; i--)
                    {
                        searchFrontier.Enqueue(tile.GrowPathTo(tile.NeighbourTiles[i], i));
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
        float newX = coord.x / StaticData.Instance.TileSize;
        float newY = coord.y / StaticData.Instance.TileSize;
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


