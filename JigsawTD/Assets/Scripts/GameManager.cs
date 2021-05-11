using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    GameBoard _board = default;
    [SerializeField]
    LevelUIManager _levelUIManager = default;

    [SerializeField]
    Vector2Int _startSize, _groundSize = default;

    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;
    [SerializeField]
    TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();

    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 100f;
    float spawnProgress;

    //������ѡ��
    [SerializeField] GameObject selection = default;
    static float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.3f; }

    public static GameTile SelectingTile = null;


    private void OnDisable()
    {
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }
    void Start()
    {
        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;
        _tileFactory.InitializeFactory();
        _board.Initialize(_startSize, _groundSize, _tileFactory);
    }


    private TileShape GetRandomNewShape()
    {
        TileShape shape = _shapeFactory.GetRandomShape();
        shape.InitializeRandomShpe(_tileFactory);
        return shape;
    }

    private void TileClick()
    {
        IsPressingTile = true;
    }

    private void TileUp(GameTile tile)
    {
        if (!IsLongPress)
        {
            if (tile == SelectingTile)
            {
                if (SelectingTile.BasicTileType == BasicTileType.Turret)
                    ((TurretTile)SelectingTile).ShowTurretRange(false);
                SelectingTile = null;
                HideSelection();
            }
            else
            {
                if (SelectingTile != null)
                {
                    if (SelectingTile.BasicTileType == BasicTileType.Turret)
                        ((TurretTile)SelectingTile).ShowTurretRange(false);
                }
                if (tile.BasicTileType == BasicTileType.Turret)
                    ((TurretTile)tile).ShowTurretRange(true);
                SelectingTile = tile;
            }
        }
        IsPressingTile = false;
    }

    void Update()
    {
        if (GameBoard.FindPath)
        {
            spawnProgress += spawnSpeed * Time.deltaTime;
            while (spawnProgress >= 1f)
            {
                spawnProgress -= 1f;
                SpawnEnemy();
            }
        }
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        turrets.GameUpdate();
        nonEnemies.GameUpdate();

        if (Input.GetKeyDown(KeyCode.R) && StaticData.holdingShape != null)
        {
            StaticData.holdingShape.RotateShape();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _levelUIManager.DisplayShape(0, GetRandomNewShape());
            _levelUIManager.DisplayShape(1, GetRandomNewShape());
            _levelUIManager.DisplayShape(2, GetRandomNewShape());
            _levelUIManager.ShowSelections();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPaths = !_board.ShowPaths;
        }

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


    public void HideSelection()
    {
        selection.SetActive(false);
    }

    private void SpawnEnemy()
    {
        GameTile tile = _board.SpawnPoint;
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
    }

}
