using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;

public class GameBoard : MonoBehaviour
{
    [SerializeField] PathLine pathLinePrefab = default;
    
    List<Turret> updatingTurret = new List<Turret>();
    TileFactory tileFactory;

    List<PathLine> pathLines = new List<PathLine>();
    List<GroundTile> groundTiles = new List<GroundTile>();
    List<GameTile> tiles = new List<GameTile>();
    static List<GameTile> shortestPath = new List<GameTile>();

    public static Path path;

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }

    public static bool FindPath { get => shortestPath.Count >= 1;}

    bool showPaths = true;
    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (!showPaths)
            {
                foreach (PathLine line in pathLines)
                {
                    line.HidePath();
                }
            }
            else
            {
                foreach (PathLine line in pathLines)
                {
                    line.ShowPath();
                }
            }
        }
    }


    private void Start()
    {
        GameEvents.Instance.onAddTiles += RePlaceTiles;
        GameEvents.Instance.onSeekPath += SeekPath;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onAddTiles -= RePlaceTiles;
        GameEvents.Instance.onSeekPath -= SeekPath;
    }

    public void GameUpdate()
    {
        for(int i = 0; i < updatingTurret.Count; i++)
        {
            updatingTurret[i].GameUpdate();
        }
    }

    public void Initialize(Vector2Int size, Vector2Int groundSize, TileFactory tileFactory)
    {
        this.tileFactory = tileFactory;
        GenerateGroundTiles(groundSize);
        Physics2D.SyncTransforms();
        Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f) * StaticData.Instance.TileSize;
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile;
                Vector2 pos = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                if (pos.x == -1 && pos.y == 0)//SpawnPoint
                {
                    tile = this.tileFactory.GetBasicTile(BasicTileType.SpawnPoint);
                    SpawnPoint = tile;
                }
                else if (pos.x == 1 && pos.y == 0)//Destination
                {
                    tile = this.tileFactory.GetBasicTile(BasicTileType.Destination);
                    DestinationPoint = tile;
                }
                else
                {
                    tile = this.tileFactory.GetTile(0);
                }
                RemoveGroundTileOnTile(pos);
                AddGameTile(tile, pos);
            }
        }
        SeekPath();
    }

    private void AddGameTile(GameTile tile, Vector2 pos)
    {
        tile.gameObject.layer = LayerMask.NameToLayer(StaticData.ConcreteTileMask);
        tile.transform.localPosition = pos;
        CorrectTileCoord(tile);
        tiles.Add(tile);
        if (tile.TileTurret != null)
            updatingTurret.Add(tile.TileTurret);
    }

    private void SeekPath()
    {
        Physics2D.SyncTransforms();
        AstarPath.active.Scan();
        var p = ABPath.Construct(SpawnPoint.transform.position, DestinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
        AstarPath.BlockUntilCalculated(p);
    }
    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            if (path != null && p.vectorPath.SequenceEqual(path.vectorPath))
            {
                Debug.Log("Found Same Path");
                return;
            }
            path = p;
            shortestPath.Clear();
            for (int i = 0; i < path.vectorPath.Count; i++)
            {
                GameTile tile = GetTile(path.vectorPath[i], StaticData.PathLayer) as GameTile;
                shortestPath.Add(tile);
            }
            GetTilePath();
            Debug.Log("Find Path!");
        }
        else
        {
            path = p;
            foreach (PathLine pl in pathLines)
            {
                ObjectPool.Instance.UnSpawn(pl.gameObject);
            }
            shortestPath.Clear();
            Debug.LogError("No Path Found");
        }
    }

    private void GetTilePath()
    {
        foreach (PathLine pl in pathLines)
        {
            ObjectPool.Instance.UnSpawn(pl.gameObject);
        }
        for (int i = 1; i < shortestPath.Count; i++)
        {
            shortestPath[i - 1].NextTileOnPath = shortestPath[i];
            shortestPath[i - 1].ExitPoint = (shortestPath[i].transform.position + shortestPath[i - 1].transform.position) * 0.5f;
            shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
            PathLine pathLine = ObjectPool.Instance.Spawn(pathLinePrefab.gameObject).GetComponent<PathLine>();
            pathLine.ShowPath(new Vector3[] { (Vector2)shortestPath[i - 1].transform.position, (Vector2)shortestPath[i].transform.position });
            pathLines.Add(pathLine);
        }
    }

    private void GenerateGroundTiles(Vector2Int groundSize)
    {
        Vector2 offset = new Vector2((groundSize.x - 1) * 0.5f, (groundSize.y - 1) * 0.5f) * StaticData.Instance.TileSize;
        for (int i = 0, y = 0; y < groundSize.y; y++)
        {
            for (int x = 0; x < groundSize.x; x++, i++)
            {
                GroundTile groundTile = tileFactory.GetGroundTile();
                groundTile.transform.localPosition = new Vector2(x, y) * StaticData.Instance.TileSize - offset;
                CorrectTileCoord(groundTile);
                groundTiles.Add(groundTile);
            }
        }
    }

    private void RePlaceTiles(List<GameTile> newTiles)
    {
        foreach (GameTile tile in newTiles)
        {
            tile.GetComponent<ReusableObject>().SetBackToParent();
            Vector3 pos = new Vector3(tile.transform.position.x, tile.transform.position.y, 0);
            RemoveGameTileOnTile(pos);
            RemoveGroundTileOnTile(pos);
            tile.SetPreviewing(false);
            AddGameTile(tile, pos);
            tile.TileDroped();
        }
    }

    private void RemoveGameTileOnTile(Vector3 pos)
    {
        GameTile gameTile = GetTile(pos, LayerMask.GetMask(StaticData.ConcreteTileMask)) as GameTile;
        if (gameTile != null)
        {
            tiles.Remove(gameTile);
            ObjectPool.Instance.UnSpawn(gameTile.gameObject);
        }
    }

    private void RemoveGroundTileOnTile(Vector3 pos)
    {
        GroundTile groundTile = GetTile(pos, LayerMask.GetMask(StaticData.GroundTileMask)) as GroundTile;
        if (groundTile != null)
        {
            groundTiles.Remove(groundTile);
            ObjectPool.Instance.UnSpawn(groundTile.gameObject);
        }
    }

    private void CorrectTileCoord(TileBase tile)
    {
        Vector2 coord = tile.transform.localPosition;
        float newX = coord.x / StaticData.Instance.TileSize;
        float newY = coord.y / StaticData.Instance.TileSize;
        tile.OffsetCoord = new Vector2(newX, newY);
    }



    public TileBase GetTile(Vector3 origin, LayerMask layer)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, layer);
        if (hit.collider != null)
        {
            TileBase hitTile = hit.collider.GetComponent<TileBase>();
            if (hitTile != null)
            {
                return hitTile;
            }
        }
        return null;
    }
}


