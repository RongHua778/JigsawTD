using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;
using PathCreation;

public class BoardSystem : IGameSystem
{
    //计算点击选中
    #region 选择Tile高亮
    static GameObject selection;
    static float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.3f; }
    private static GameTile selectingTile;
    public static GameTile SelectingTile
    {
        get => selectingTile;
        set
        {
            if (selectingTile != null)
            {
                if (selectingTile.BasicTileType == BasicTileType.Turret)
                {
                    ((TurretTile)selectingTile).ShowTurretRange(false);
                }
                selectingTile = selectingTile == value ? null : value;
            }
            else
            {
                selectingTile = value;
            }
            if (selectingTile != null)
            {
                if (selectingTile.BasicTileType == BasicTileType.Turret)
                {
                    ((TurretTile)selectingTile).ShowTurretRange(true);
                }
                LevelUIManager.Instance.ShowTileTips(selectingTile);
                selection.transform.position = selectingTile.transform.position;
            }
            selection.SetActive(selectingTile != null);
        }

    }
    #endregion

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
    public List<GameTile> shortestPath = new List<GameTile>();

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


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        selection = transform.Find("Selection").gameObject;

        GameEvents.Instance.onAddTiles += RePlaceTiles;
        GameEvents.Instance.onSeekPath += SeekPath;
        GameEvents.Instance.onRemoveGameTile += RemoveGameTile;

        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;
    }

    public override void Release()
    {
        GameEvents.Instance.onAddTiles -= RePlaceTiles;
        GameEvents.Instance.onSeekPath -= SeekPath;
        GameEvents.Instance.onRemoveGameTile -= RemoveGameTile;

        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }



    public void GameUpdate()
    {


        if (IsPressingTile && Input.GetMouseButton(0))
        {
            pressCounter += Time.deltaTime;
        }
        else
        {
            pressCounter = 0;
        }
        if (SelectingTile != null)
        {
            selection.SetActive(true);
            selection.transform.position = SelectingTile.transform.position;
        }
        else
        {
            selection.SetActive(false);
        }
    }
    private void TileClick()
    {
        IsPressingTile = true;
    }

    private void TileUp(GameTile tile)
    {
        if (!IsLongPress)
        {
            LevelUIManager.Instance.HideTips();
            SelectingTile = tile;
        }
        IsPressingTile = false;
    }

    public void SetGameBoard(Vector2Int size, Vector2Int groundSize, TileFactory tileFactory)
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
                if (pos.x == 0 && pos.y != 0)
                    continue;
                if (pos.x == -1 && pos.y == 0)//SpawnPoint
                {
                    tile = this.tileFactory.GetImportantTile(BasicTileType.SpawnPoint);
                    SpawnPoint = tile;
                }
                else if (pos.x == 1 && pos.y == 0)//Destination
                {
                    tile = this.tileFactory.GetImportantTile(BasicTileType.Destination);
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
        GenerateTrapTiles(groundSize, StaticData.trapN, tileFactory);
        SeekPath();
        ShowPath(path);
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
        for (int i = 1; i < path.vectorPath.Count; i++)
        {
            PathLine pathLine = ObjectPool.Instance.Spawn(pathLinePrefab.gameObject).GetComponent<PathLine>();
            pathLine.ShowPath(new Vector3[] { (Vector2)path.vectorPath[i - 1], (Vector2)path.vectorPath[i] });
            pathLines.Add(pathLine);
        }

    }
    private void GenerateTrapTiles(Vector2Int groundSize, int trapN, TileFactory t)
    {

        List<Vector2> tiles = new List<Vector2>();
        List<Vector2> basicPoss = new List<Vector2>();

        List<Vector2> traps = new List<Vector2>();
        for (int i = 1; i < groundSize.x - 1; i++)
        {
            for (int j = 1; j < groundSize.y - 1; j++)
            {
                //避免陷阱刷到初始的方块上
                if (!(i >= 10 && i <= 14 && j >= 10 && j <= 14))
                {
                    tiles.Add(new Vector2(i, j));
                    basicPoss.Add(new Vector2(i, j));
                }
            }
        }
        for (int i = 0; i < trapN; i++)
        {
            int index = UnityEngine.Random.Range(0, tiles.Count);
            Vector2 temp = tiles[index];
            traps.Add(temp);
            List<Vector2> neibor = StaticData.GetCirclePoints(5, 0);
            for (int k = 0; k < neibor.Count; k++)
            {
                neibor[k] = neibor[k] + tiles[index];
            }
            tiles = tiles.Except(neibor).ToList();
            tiles.Remove(temp);
            basicPoss.Remove(temp);
        }

        for (int j = 0; j < StaticData.basicN; j++)
        {
            int index = UnityEngine.Random.Range(0, basicPoss.Count);
            Vector2 pos = basicPoss[index];
            BasicTile tile = t.GetBasicTile();

            AddGameTile(tile, new Vector2(pos.x - (groundSize.x - 1) / 2,
                pos.y - (groundSize.y - 1) / 2));
            //tile.gameObject.layer = LayerMask.NameToLayer(StaticData.TrapTileMask);
        }

        foreach (Vector2 trap in traps)
        {
            TrapTile tile = t.GetRandomTrap();
            AddGameTile(tile, new Vector2(trap.x - (groundSize.x - 1) / 2,
                trap.y - (groundSize.y - 1) / 2));
            tile.gameObject.layer = LayerMask.NameToLayer(StaticData.ConcreteTileMask);
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
            tile.SetBackToParent();
            Vector2 pos = tile.transform.position;
            Collider2D col = StaticData.RaycastCollider(pos, LayerMask.GetMask(StaticData.ConcreteTileMask));
            tile.SetPreviewing(false);
            if (col != null)
            {
                if (tile.TileContent == null)
                    ObjectPool.Instance.UnSpawn(tile.gameObject);
                else
                {
                    RemoveGameTileOnPos(pos);
                    AddGameTile(tile, pos);
                }
                //if (col.GetComponentInParent<TurretTile>() || col.GetComponentInParent<TrapTile>())
                //{
                //    ObjectPool.Instance.UnSpawn(tile.gameObject);
                //    continue;
                //}
                //else
                //{
                //    RemoveGameTileOnPos(pos);
                //}
            }
            else
            {
                AddGameTile(tile, pos);
            }
        }
        foreach (GameTile tile in newTiles)
        {
            tile.TileDroped();
        }
        GameEvents.Instance.CheckBluePrint();
    }

    private void RemoveGameTileOnPos(Vector3 pos)
    {
        GameTile gameTile = GetTile(pos, LayerMask.GetMask(StaticData.ConcreteTileMask)) as GameTile;
        if (gameTile != null)
        {
            tiles.Remove(gameTile);
            if (SelectingTile == gameTile)
                SelectingTile = null;
            ObjectPool.Instance.UnSpawn(gameTile.gameObject);
        }
        GroundTile groundTile = GetTile(pos, StaticData.GetGroundLayer) as GroundTile;
        if (groundTile != null)
        {
            groundTile.gameObject.layer = LayerMask.NameToLayer(StaticData.GroundTileMask);
        }
    }

    private void RemoveGameTile(GameTile tile)
    {
        if (tiles.Contains(tile))
        {
            tiles.Remove(tile);
            if (SelectingTile == tile)
                SelectingTile = null;
            ObjectPool.Instance.UnSpawn(tile.gameObject);

            GroundTile groundTile = GetTile(tile.transform.position, StaticData.GetGroundLayer) as GroundTile;
            if (groundTile != null)
            {
                groundTile.gameObject.layer = LayerMask.NameToLayer(StaticData.GroundTileMask);
            }
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


