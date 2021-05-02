using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class GameBoard : MonoBehaviour
{
    List<GroundTile> groundTiles = new List<GroundTile>();
    List<TempTile> tempTileList = new List<TempTile>();

    Vector2Int size;
    List<GameTile> tiles;

    TileFactory tileFactory;
    GameTileContentFactory tileContentFactory;

    public List<GameTile> shortestPath = new List<GameTile>();

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }

    public static Path path;

    bool findPath = false;

    public bool FindPath { get => findPath; set => findPath = value; }

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
                foreach (TempTile temp in tempTileList)
                {
                    temp.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (TempTile temp in tempTileList)
                {
                    temp.gameObject.SetActive(false);
                }
            }
        }
    }


    private void Start()
    {
        GameEvents.Instance.onAddTiles += AddTile;
        GameEvents.Instance.onSeekPath += SeekPath;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onAddTiles -= AddTile;
        GameEvents.Instance.onSeekPath -= SeekPath;
    }

    public void Initialize(Vector2Int size, TileFactory tileFactory, GameTileContentFactory contentFactory)
    {
        this.size = size;
        this.tileFactory = tileFactory;
        tileContentFactory = contentFactory;
        GenerateGroundTiles();
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f) * StaticData.Instance.TileSize;
        tiles = new List<GameTile>();
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile;
                if (i==3)//SpawnPoint
                {
                    tile = this.tileFactory.GetBasicTile(BasicTileType.SpawnPoint);
                    SpawnPoint = tile;
                } 
                else if (i==5)//Destination
                {
                    tile = this.tileFactory.GetBasicTile(BasicTileType.Destination);
                    DestinationPoint = tile;
                }
                else
                {
                    tile = this.tileFactory.GetTile(0);
                }
                tiles.Add(tile);
                tile.gameObject.layer = LayerMask.NameToLayer("ConcreteTile");
                tile.transform.localPosition = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                CorrectTileCoord(tile);
                tile.Content = contentFactory.Get(GameTileContentType.Empty);
                GroundTile groundTile = groundTiles.Find(c => c.OffsetCoord == tile.OffsetCoord);
                if (groundTile != null)
                {
                    groundTile.gameObject.layer = 0;
                }
            }

        }



        //ToggleDestination(tiles[5]);
        //ToggleSpawnPoint(tiles[3]);

        SeekPath();
    }

    public LayerMask GameTileMask;
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            foreach (GameTile tile in tiles)
            {
                tile.HidePath();
            }
            shortestPath.Clear();
            for (int i = 0; i < path.vectorPath.Count; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(path.vectorPath[i], Vector3.forward, Mathf.Infinity, GameTileMask);
                GameTile tile = hit.collider.GetComponent<GameTile>();
                shortestPath.Add(tile);
            }
            for(int i = 1; i < shortestPath.Count; i++)
            {
                shortestPath[i - 1].NextTileOnPath = shortestPath[i];
                shortestPath[i - 1].ExitPoint = (shortestPath[i].transform.position + shortestPath[i - 1].transform.position) * 0.5f;
                shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
            }
            if (ShowPaths)
            {
                foreach (GameTile tile in shortestPath)
                {
                    tile.ShowPath();
                }
            }
            FindPath = true;
            Debug.Log("find path!");
        }
        else
        {
            path = p;
            foreach (GameTile tile in shortestPath)
            {
                tile.HidePath();
            }
            shortestPath.Clear();
            FindPath = false;
            Debug.LogError("NoPathFound");
        }
    }

    private void GenerateGroundTiles()
    {
        Vector2 offset = new Vector2((21 - 1) * 0.5f, (21 - 1) * 0.5f) * StaticData.Instance.TileSize;
        for (int i = 0, y = 0; y < 21; y++)
        {
            for (int x = 0; x < 21; x++, i++)
            {
                GroundTile groundTile = tileFactory.GetGroundTile();
                groundTile.transform.localPosition = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                CorrectTileCoord(groundTile);
                groundTiles.Add(groundTile);
            }
        }
    }


    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
            DestinationPoint = tile;
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.SpawnPoint);
            SpawnPoint = tile;
        }
    }


    public void GetShortestPath()
    {
        if (shortestPath.Count > 0)
        {
            foreach (GameTile tile in tiles)
            {
                tile.HidePath();
            }
            shortestPath.Clear();
        }
        GameTile findTile = SpawnPoint;
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
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Turret);
            SeekPath();
        }
        else if (tile.Content.Type == GameTileContentType.Turret)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            SeekPath();
        }
    }

    private void SeekPath()
    {
        Physics2D.SyncTransforms();
        AstarPath.active.Scan();
        var p = ABPath.Construct(SpawnPoint.transform.position, DestinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
    }



    private void AddTile(List<GameTile> newTiles)
    {
        foreach (GameTile tile in newTiles)
        {
            tile.SetPreviewing(false);
            tile.transform.SetParent(this.transform);
            tile.gameObject.layer = LayerMask.NameToLayer("ConcreteTile");
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            CorrectTileCoord(tile);

            var gameTile = tiles.Find(t => t.OffsetCoord == tile.OffsetCoord);
            if (gameTile != null)
            {
                tiles.Remove(gameTile);
                ObjectPool.Instance.UnSpawn(gameTile.gameObject);
            }
            var groundTile = groundTiles.Find(t => t.OffsetCoord == tile.OffsetCoord);
            if (groundTile != null)
            {
                groundTiles.Remove(groundTile);
                ObjectPool.Instance.UnSpawn(groundTile.gameObject);
            }
            tiles.Add(tile);

        }

        SeekPath();
    }

    Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    private void CorrectTileCoord(TileBase tile)
    {
        Vector2 coord = tile.transform.localPosition;
        float newX = coord.x / StaticData.Instance.TileSize;
        float newY = coord.y / StaticData.Instance.TileSize;
        tile.OffsetCoord = new Vector2(newX, newY);
    }



    public GameTile GetTile()
    {
        RaycastHit2D[] hits;
        hits = Physics2D.RaycastAll(MouseInWorldCoords(), Vector3.forward, Mathf.Infinity);
        GameTile hitTile = null;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.GetComponent<DraggingActions>() != null)//正在拖动方块
                return null;
            if (hit.collider.GetComponent<GameTile>() != null)
                hitTile = hit.collider.GetComponent<GameTile>();
        }
        return hitTile;
    }
}


