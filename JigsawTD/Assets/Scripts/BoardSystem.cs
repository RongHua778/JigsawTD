using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using System.Linq;


[Serializable]
public struct PathPoint
{
    public Vector2 PathPos;
    public Direction PathDirection;
    public Vector2 ExitPoint;
    public PathPoint(Vector2 pos, Direction dir, Vector2 exit)
    {
        PathPos = pos;
        PathDirection = dir;
        ExitPoint = exit;
    }
}

public class BoardSystem : IGameSystem
{
    //计算点击选中
    #region 选择Tile高亮
    static GameObject selection;
    float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.2f; }
    private Camera mainCam;
    private float moveDis;
    private Vector3 startPos;


    private static TileBase selectingTile;
    public static TileBase SelectingTile
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
                selection.transform.position = selectingTile.transform.position;
                selectingTile.Content.OnContentSelected(true);
            }
            selection.SetActive(selectingTile != null);
        }

    }
    #endregion

    public static Vector2Int _startSize = new Vector2Int(3, 3); //初始大小
    public static Vector2Int _groundSize = new Vector2Int(25, 25); //地图大小

    [SerializeField] PathFollower PathFollowerPrefab = default;
    private GameBehaviorCollection followers = new GameBehaviorCollection();

    List<Vector2Int> tilePoss = new List<Vector2Int>();

    public List<GameTile> shortestPath = new List<GameTile>();

    public List<PathPoint> shortestPoints = new List<PathPoint>();

    public static Path path;

    GameTile spawnPoint;
    public GameTile SpawnPoint { get => spawnPoint; set => spawnPoint = value; }
    GameTile destinationPoint;
    public GameTile DestinationPoint { get => destinationPoint; set => destinationPoint = value; }

    //买一块地板多少钱
    int buyOneGroundMoney = 20;
    public int BuyOneGroundMoney
    {
        get => buyOneGroundMoney;
        set
        {
            buyOneGroundMoney = value;
        }
    }

    //买一块地板多少钱
    int switchTrapCost = 50;
    public int SwitchTrapCost
    {
        get => switchTrapCost;
        set
        {
            switchTrapCost = value;
        }
    }

    public static bool FindPath { get; set; }

    public override void Initialize()
    {
        selection = transform.Find("Selection").gameObject;
        mainCam = Camera.main;
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
        followers.GameUpdate();
        if (IsPressingTile && Input.GetMouseButton(0))
        {
            pressCounter += Time.deltaTime;
        }
        else
        {
            pressCounter = 0;
        }
    }
    private void TileClick()
    {
        IsPressingTile = true;
        startPos = Input.mousePosition;
    }

    private void TileUp(TileBase tile)
    {
        moveDis = Vector2.SqrMagnitude(Input.mousePosition - startPos);
        if (moveDis < 0.5f)
        {
            SelectingTile = tile;
        }
        IsPressingTile = false;
    }


    private void OnApplicationFocus(bool focus)
    {
        if (FindPath)
            ShowPath();
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
        ShowPath();
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
                    tile = ConstructHelper.GetSpawnPoint();
                    SpawnPoint = tile;
                }
                else if (pos.x == 1 && pos.y == 0)//Destination
                {
                    tile = ConstructHelper.GetDestinationPoint();
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
                //Debug.Log("Found Same Path");
                return;
            }
            path = p;
            ShowPath();
            //Debug.Log("Find Path!");
        }
        else
        {
            path = p;
            HidePath();
            shortestPath.Clear();
            //Debug.LogError("No Path Found");
            FindPath = false;
        }
    }

    public void GetPathPoints()
    {
        shortestPoints.Clear();
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            Direction dir = Direction.up;
            if (i < path.vectorPath.Count - 1)
                dir = DirectionExtensions.GetDirection(path.vectorPath[i], path.vectorPath[i + 1]);
            PathPoint point = new PathPoint(path.vectorPath[i], dir, path.vectorPath[i] + dir.GetHalfVector());
            shortestPoints.Add(point);
        }
    }


    private void ShowPath()
    {
        GetPathPoints();
        HidePath();
        for (int i = 0; i < shortestPoints.Count - 1; i++)
        {
            PathFollower follower = ObjectPool.Instance.Spawn(PathFollowerPrefab) as PathFollower;
            follower.SpawnOn(i, shortestPoints);
            followers.Add(follower);
        }
    }


    private void HidePath()
    {
        foreach (PathFollower pl in followers.behaviors)
        {
            ObjectPool.Instance.UnSpawn(pl);
        }
        followers.behaviors.Clear();
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
        for (int i = 0; i < StaticData.Instance.trapN; i++)
        {
            int index = UnityEngine.Random.Range(0, tempPoss.Count);
            Vector2Int temp = tempPoss[index];
            traps.Add(temp);
            List<Vector2Int> neibor = StaticData.GetCirclePoints(5);
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
            //tile.SetRandomRotation();
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

    public void BuyOneEmptyTile()
    {
        if (GameManager.Instance.OperationState.StateName == StateName.WaveState)
        {
            GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBATTLESTATE"));
            return;
        }
        if (StaticData.GetNodeWalkable(SelectingTile))
        {
            GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("ALREADYGROUND"));
            return;
        }
        if (StaticData.FreeGroundTileCount > 0)
        {
            StaticData.FreeGroundTileCount--;
        }
        else if (!GameManager.Instance.ConsumeMoney(BuyOneGroundMoney))
        {
            return;
        }
        else
        {
            BuyOneGroundMoney += 20;
        }
        GameTile tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
        tile.transform.position = SelectingTile.transform.position;
        tile.TileLanded();
        Physics2D.SyncTransforms();
        if (DraggingShape.PickingShape != null)
        {
            DraggingShape.PickingShape.ShapeFindPath();//只有用这个才找的到路
        }
        else
        {
            SeekPath();
        }
        GameManager.Instance.HideTips();
    }


    public void CheckPathTrap()//检查是否有陷阱加入到路线中
    {
        foreach (var pathPoint in shortestPoints)
        {
            Collider2D col = StaticData.RaycastCollider(pathPoint.PathPos, LayerMask.GetMask(StaticData.TrapMask));
            if (col != null)
            {
                TrapContent trap = col.GetComponent<TrapContent>();
                trap.RevealTrap();
            }
        }
    }

    public void SwitchTrap(TrapContent trap)
    {
        SwitchTrapCost += 50;
        Vector2 pos = trap.m_GameTile.transform.position;
        ObjectPool.Instance.UnSpawn(trap.m_GameTile);
        GameTile tile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
        tile.transform.position = pos;
        tile.TileLanded();
        ConstructHelper.GetTrapByName(trap.TrapAttribute.Name);

    }


}


