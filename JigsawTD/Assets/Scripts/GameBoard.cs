using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    TempTile tempTilePrefab = default;
    [SerializeField]
    GroundTile baseTilePrefab = default;

    List<GroundTile> groundTiles = new List<GroundTile>();
    List<TempTile> tempTileList = new List<TempTile>();

    Vector2Int size;
    List<GameTile> tiles;

    TileFactory tileFactory;
    GameTileContentFactory tileContentFactory;

    public static List<GameTile> shortestPath = new List<GameTile>();

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
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
        GenerateGroundTiles();
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
                GroundTile groundTile = groundTiles.Find(c => c.OffsetCoord == tile.OffsetCoord);
                if (groundTile != null)
                {
                    groundTile.gameObject.layer = LayerMask.NameToLayer("Default");
                }
            }

        }

        foreach (GameTile tile in tiles)
        {
            GenerateTempTile(tile);
        }


        ToggleDestination(tiles[5]);
        ToggleSpawnPoint(tiles[3]);

        SeekPath();
    }

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
                GameTile tile = tiles.Find(c => c.OffsetCoord == (Vector2)path.vectorPath[i]);
                shortestPath.Add(tile);
                if (shortestPath.Count > 1)
                {
                    shortestPath[i - 1].NextTileOnPath = tile;
                    shortestPath[i - 1].ExitPoint = (tile.transform.position + shortestPath[i - 1].transform.position) * 0.5f;
                    shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
                }
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
                GroundTile groundTile = Instantiate(baseTilePrefab);
                groundTile.transform.SetParent(transform, false);
                groundTile.transform.localPosition = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                CorrectTileCoord(groundTile);
                groundTiles.Add(groundTile);
            }
        }
    }

    private void GenerateTempTile(GameTile tile)
    {
        foreach (Vector2 dir in DirectionExtensions.NormalizeDistance)
        {
            Vector2 pos = (Vector2)tile.transform.position + dir;
            var temp = tiles.Find(t => (Vector2)t.transform.position == pos);
            var temp2 = tempTileList.Find(t => (Vector2)t.transform.position == pos);
            if (temp == null && temp2 == null)
            {
                GameObject tempTile = ObjectPool.Instance.Spawn(tempTilePrefab.gameObject);
                tempTile.transform.position = pos;
                CorrectTileCoord(tempTile.GetComponent<TempTile>());
                tempTileList.Add(tempTile.GetComponent<TempTile>());
            }
        }

    }

    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = tileContentFactory.Get(GameTileContentType.Destination);
            destinationPoint = tile;
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
        var p = ABPath.Construct(SpawnPoint.transform.position, destinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
    }



    private void AddTile(List<GameTile> newTiles)
    {
        foreach (GameTile tile in newTiles)
        {
            
            tile.SetPreviewing(false);
            tile.transform.SetParent(this.transform);
            tile.Content = tileContentFactory.Get(GameTileContentType.Empty);
            CorrectTileCoord(tile);

            var tempTile = tempTileList.Find(t => t.OffsetCoord == tile.OffsetCoord);
            if (tempTile != null)
            {
                tempTileList.Remove(tempTile);
                ObjectPool.Instance.UnSpawn(tempTile.gameObject);
            }
            var gameTile = tiles.Find(t => t.OffsetCoord == tile.OffsetCoord);
            if (gameTile != null)
            {
                tiles.Remove(gameTile);
                Destroy(gameTile.gameObject);
            }
            var groundTile= groundTiles.Find(t => t.OffsetCoord == tile.OffsetCoord);
            if (groundTile != null)
            {
                groundTiles.Remove(groundTile);
                Destroy(groundTile.gameObject);
            }
            tiles.Add(tile);

        }
        foreach (GameTile tile in newTiles)
        {
            GenerateTempTile(tile);
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


