using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;
using PathCreation;

public class GameBoard : MonoBehaviour
{
    int[,] traps = new int[25, 25];

    [SerializeField] PathLine pathLinePrefab = default;
    List<PathLine> pathLines = new List<PathLine>();

    //private PathCreator pathCreator;
    //public MeshRenderer pathMeshHolder;

    TileFactory tileFactory;

    //地板上的空tile
    List<GroundTile> groundTiles = new List<GroundTile>();
    //生成的陷阱
    List<TrapTile> trapTiles = new List<TrapTile>();


    List<GameTile> tiles = new List<GameTile>();
    static List<GameTile> shortestPath = new List<GameTile>();

    public static Path path;

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }

    public static bool FindPath { get => path.vectorPath.Count > 0; }

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

        //pathCreator = this.GetComponent<PathCreator>();
        //pathMeshHolder.sortingLayerName = "UI";
    }

    private void OnDisable()
    {
        GameEvents.Instance.onAddTiles -= RePlaceTiles;
        GameEvents.Instance.onSeekPath -= SeekPath;
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
                    tile = this.tileFactory.GetBasicTile();
                }
                //RemoveGroundTileOnTile(tile, pos);
                AddGameTile(tile, pos);
            }
        }
        GenerateTrapTiles(groundSize,StaticData.trapN);
        SeekPath();
    }

    private void AddGameTile(GameTile tile, Vector2 pos)
    {
        GroundTile groundTile = GetTile(pos, StaticData.GetGroundLayer) as GroundTile;
        if (groundTile != null)
        {
            groundTile.TileAbrove = tile;
            groundTile.gameObject.layer = LayerMask.NameToLayer(StaticData.TempGroundMask);
        }
        tile.gameObject.layer = LayerMask.NameToLayer(StaticData.ConcreteTileMask);
        tile.transform.localPosition = pos;
        CorrectTileCoord(tile);
        tiles.Add(tile);
        if (tile.BasicTileType == BasicTileType.Turret)
        {
            //turret的tile加入一个list
            GameManager.Instance.turrets.Add(((TurretTile)tile).turret);
            //turret加入一个list以检测合成逻辑
            //GameManager.Instance.turretsElements.Add(tile.GetComponentInChildren<Turret>());
        }
        groundTile.TriggerIntensify();
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
                //Debug.Log("Found Same Path");
                return;
            }
            path = p;
            ShowPath(path);
            //Debug.Log("Find Path!");
        }
        else
        {
            path = p;
            foreach (PathLine pl in pathLines)
            {
                ObjectPool.Instance.UnSpawn(pl.gameObject);
            }
            shortestPath.Clear();
            //Debug.LogError("No Path Found");
        }
    }

    public void GetPathTiles()
    {
        shortestPath.Clear();
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            GameTile tile = GetTile(path.vectorPath[i], StaticData.PathLayer) as GameTile;
            shortestPath.Add(tile);
        }
        for (int i = 1; i < shortestPath.Count; i++)
        {
            shortestPath[i - 1].NextTileOnPath = shortestPath[i];
            shortestPath[i - 1].ExitPoint = (shortestPath[i].transform.position + shortestPath[i - 1].transform.position) * 0.5f;
            shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
        }
    }

    private void ShowPath(Path path)
    {
        foreach (PathLine pl in pathLines)
        {
            ObjectPool.Instance.UnSpawn(pl.gameObject);
        }
        //for (int i = 1; i < shortestPath.Count; i++)
        //{
        //    shortestPath[i - 1].NextTileOnPath = shortestPath[i];
        //    shortestPath[i - 1].ExitPoint = (shortestPath[i].transform.position + shortestPath[i - 1].transform.position) * 0.5f;
        //    shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
        //    PathLine pathLine = ObjectPool.Instance.Spawn(pathLinePrefab.gameObject).GetComponent<PathLine>();
        //    pathLine.ShowPath(new Vector3[] { (Vector2)shortestPath[i - 1].transform.position, (Vector2)shortestPath[i].transform.position });
        //    pathLines.Add(pathLine);
        //}
        for (int i = 1; i < path.vectorPath.Count; i++)
        {
            PathLine pathLine = ObjectPool.Instance.Spawn(pathLinePrefab.gameObject).GetComponent<PathLine>();
            pathLine.ShowPath(new Vector3[] { (Vector2)path.vectorPath[i - 1], (Vector2)path.vectorPath[i] });
            pathLines.Add(pathLine);
        }

    }
    private void GenerateTrapTiles(Vector2Int groundSize,int trapN)
    {
        List<int> X=new List<int>();
        List<int> Y=new List<int>();

        for(int i = 0; i < groundSize.x; i++)
        {
            X.Add(i);
        }
        for (int i = 0; i < groundSize.y; i++)
        {
            Y.Add(i);
        }
        for (int i = 0; i < trapN; i++)
        {
            int x = UnityEngine.Random.Range(0,X.Count);
            int y = UnityEngine.Random.Range(0,Y.Count);
            //*****以后需要根据需求改动
            while ((x <=14&&x>=12)&&(y<=14&&y<=14))
            {
                x = UnityEngine.Random.Range(0, X.Count);
                y = UnityEngine.Random.Range(0, Y.Count);
            }
            traps[x, y] = 1;
            X.Remove(x);
            Y.Remove(y);
        }
        for (int i = 0, y = 0; y < groundSize.y; y++)
        {
            for (int x = 0; x < groundSize.x; x++, i++)
            {
                //在这里判断哪里有陷阱
                if (traps[x, y] == 1)
                {
                    TrapTile trapTile = tileFactory.GetRandomTrap();
                    AddGameTile(trapTile, new Vector2(x-(groundSize.x-1)/2, y - (groundSize.y - 1) / 2));
                }
            }
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
                    groundTile.transform.localPosition += Vector3.forward * 0.1f;
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
            tile.SetPreviewing(false);
            AddGameTile(tile, pos);
        }
        foreach (GameTile tile in newTiles)
        {
            tile.TileDroped();
        }
        GameEvents.Instance.CheckBluePrint();
    }

    private void RemoveGameTileOnTile(Vector3 pos)
    {
        GameTile gameTile = GetTile(pos, LayerMask.GetMask(StaticData.ConcreteTileMask)) as GameTile;
        if (gameTile != null)
        {
            tiles.Remove(gameTile);
            if (GameManager.SelectingTile == gameTile)
                GameManager.SelectingTile = null;
            ObjectPool.Instance.UnSpawn(gameTile.gameObject);
        }
    }

    //private void RemoveGroundTileOnTile(GameTile tile, Vector3 pos)
    //{
    //    GroundTile groundTile = GetTile(pos, LayerMask.GetMask(StaticData.GroundTileMask)) as GroundTile;
    //    if (groundTile != null)
    //    {
    //        //groundTiles.Remove(groundTile);
    //        groundTile.TileAbrove = tile;
    //        groundTile.gameObject.layer = LayerMask.NameToLayer(StaticData.TempGroundMask);
    //        //ObjectPool.Instance.UnSpawn(groundTile.gameObject);
    //    }
    //}

    private void CorrectTileCoord(TileBase tile)
    {
        Vector2 coord = tile.transform.localPosition;
        float newX = coord.x / StaticData.Instance.TileSize;
        float newY = coord.y / StaticData.Instance.TileSize;
        tile.OffsetCoord = new Vector2(newX, newY);
    }



    public static TileBase GetTile(Vector3 origin, LayerMask layer)
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


