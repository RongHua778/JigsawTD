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
    float pressCounter = 0;
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
                selectingTile.Content.OnContentSelected(false);
                selectingTile = selectingTile == value ? null : value;
                GameManager.Instance.HideTips();
            }
            else
            {
                selectingTile = value;
            }
            if (selectingTile != null)
            {
                selectingTile.Content.OnContentSelected(true);
                selection.transform.position = selectingTile.transform.position;
            }
            selection.SetActive(selectingTile != null);
        }

    }
    #endregion
    //_groundsize是地图每一边上方块的数量
    //startSize是初始生成的有方块的大小

    public static Vector2Int _startSize = new Vector2Int(3, 3);
    public static Vector2Int _groundSize = new Vector2Int(25, 25);

    [SerializeField] PathFollower pathFollowerPrefab = default;
    List<PathFollower> pathFollowers = new List<PathFollower>();

    List<Vector2Int> tilePoss = new List<Vector2Int>();

    public List<GameTile> shortestPath = new List<GameTile>();

    public static Path path;

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }

    public static bool FindPath { get; set; }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        selection = transform.Find("Selection").gameObject;

        GameEvents.Instance.onSeekPath += SeekPath;
        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;

        SetGameBoard();
    }

    public override void Release()
    {
        GameEvents.Instance.onSeekPath -= SeekPath;
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }

    public override void GameUpdate()
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
            selection.transform.position = SelectingTile.transform.position;
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
            SelectingTile = tile;
        }
        IsPressingTile = false;
    }

    public void SetGameBoard()
    {
        Vector2Int sizeOffset = new Vector2Int((int)((_startSize.x - 1) * 0.5f), (int)((_startSize.y - 1) * 0.5f));
        StaticData.BoardOffset = new Vector2Int((int)((_groundSize.x - 1) * 0.5f), (int)((_groundSize.y - 1) * 0.5f));
        
        GenerateGroundTiles(_groundSize);
        Physics2D.SyncTransforms();//涉及物理检测前，需要调用
        AstarPath.active.Scan();

        GenerateStartTiles(_startSize, sizeOffset);
        GenerateTrapTiles(sizeOffset, _startSize);
        Physics2D.SyncTransforms();
        SeekPath();
    }

    private void GenerateStartTiles(Vector2Int size, Vector2Int offset)
    {
        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = null;
                Vector2Int pos = new Vector2Int(x, y) - offset;
                if (pos.x == 0 && pos.y != 0)
                    continue;
                if (pos.x == -1 && pos.y == 0)//SpawnPoint
                {
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.SpawnPoint);
                    SpawnPoint = tile;
                }
                else if (pos.x == 1 && pos.y == 0)//Destination
                {
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.Destination);
                    DestinationPoint = tile;
                }
                else//空格子
                {
                    tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
                }
                tile.transform.position = (Vector3Int)pos;
                tile.TileLanded();
                Physics2D.SyncTransforms();
            }
        }
    }

    private void SeekPath()
    {
        // Physics2D.SyncTransforms();
        //AstarPath.active.Scan();
        var p = ABPath.Construct(SpawnPoint.transform.position, DestinationPoint.transform.position, OnPathComplete);
        AstarPath.StartPath(p);
        AstarPath.BlockUntilCalculated(p);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            FindPath = true;
            if (path != null && p.vectorPath.SequenceEqual(path.vectorPath))
            {
                Debug.Log("Found Same Path");
                return;
            }
            path = p;
            GetPathTiles();
            ShowPath();
            Debug.Log("Find Path!");
        }
        else
        {
            path = p;
            HidePath();
            shortestPath.Clear();
            Debug.LogError("No Path Found");
            FindPath = false;
        }
    }

    public void GetPathTiles()
    {
        shortestPath.Clear();
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            //Collider2D col = StaticData.RaycastCollider(path.vectorPath[i], LayerMask.GetMask(StaticData.ConcreteTileMask));
            Collider2D col = StaticData.RaycastCollider(path.vectorPath[i], StaticData.PathLayer);
            GameTile tile = col.GetComponent<GameTile>();
            shortestPath.Add(tile);
        }
        for (int i = 1; i < shortestPath.Count; i++)
        {
            shortestPath[i - 1].NextTileOnPath = shortestPath[i];
            shortestPath[i].NextTileOnPath = null;
            shortestPath[i - 1].ExitPoint = (shortestPath[i].transform.position + shortestPath[i - 1].transform.position) * 0.5f;
            shortestPath[i - 1].PathDirection = DirectionExtensions.GetDirection(shortestPath[i - 1].transform.position, shortestPath[i - 1].ExitPoint);
        }
    }

    private void ShowPath()
    {
        HidePath();
        for (int i = 0; i < shortestPath.Count-1; i++)
        {
            PathFollower follower = ObjectPool.Instance.Spawn(pathFollowerPrefab) as PathFollower;
            follower.SpawnPoint = shortestPath[0];
            follower.SpawnOn(shortestPath[i]);
            //pathLine.ShowPath(new Vector3[] { (Vector2)path.vectorPath[i - 1], (Vector2)path.vectorPath[i] });
            pathFollowers.Add(follower);
        }

    }

    private void HidePath()
    {
        foreach (PathFollower pl in pathFollowers)
        {
            ObjectPool.Instance.UnSpawn(pl);
        }
        pathFollowers.Clear();
    }
    private void GenerateTrapTiles(Vector2Int offset, Vector2Int size)
    {
        List<Vector2Int> traps = new List<Vector2Int>();
        List<Vector2Int> tempPoss = tilePoss.ToList();
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int pos = new Vector2Int(x, y) - offset;
                tempPoss.Remove(pos);
            }
        }
        for (int i = 0; i < StaticData.trapN; i++)
        {
            int index = UnityEngine.Random.Range(0, tempPoss.Count);
            Vector2Int temp = tempPoss[index];
            traps.Add(temp);
            List<Vector2Int> neibor = StaticData.GetCirclePoints(5, 0);
            for (int k = 0; k < neibor.Count; k++)
            {
                neibor[k] = neibor[k] + tempPoss[index];
            }
            tempPoss = tempPoss.Except(neibor).ToList();
            tempPoss.Remove(temp);
        }
        foreach (Vector2Int pos in traps)
        {
            GameTile tile = ConstructHelper.GetRandomTrap();
            tile.transform.position = (Vector3Int)pos;
            tile.TileLanded();
        }
    }
    private void GenerateGroundTiles(Vector2Int groundSize)
    {
        for (int i = 0, y = 0; y < groundSize.y; y++)
        {
            for (int x = 0; x < groundSize.x; x++, i++)
            {
                GroundTile groundTile = ConstructHelper.GetGroundTile();
                Vector2Int pos = new Vector2Int(x, y) - StaticData.BoardOffset;
                groundTile.transform.position = (Vector3Int)pos;
                groundTile.transform.position += Vector3.forward * 0.1f;
                StaticData.CorrectTileCoord(groundTile);
                tilePoss.Add(pos);
            }
        }
    }





}


