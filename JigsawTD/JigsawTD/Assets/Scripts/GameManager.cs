using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameBoard _board = default;

    [SerializeField]
    Vector2Int _startSize = default;

    [SerializeField]
    GameTileContentFactory _contentFactory = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;
    [SerializeField]
    TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    EnemyCollection enemies = new EnemyCollection();
    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 100f;
    float spawnProgress;


    [SerializeField]
    LevelUIManager _levelUIManager = default;

    public static DraggingShape holdingShape;

    Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onGenerateShape += PreSpawnShape;

        _tileFactory.InitializeFactory();

        _board.Initialize(_startSize, _tileFactory, _contentFactory);
        _board.ShowTempTile = true;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onGenerateShape -= PreSpawnShape;
    }

    private void PreSpawnShape(ShapeType shapeType, List<TileInfo> infoList)
    {
        TileShape shape = _shapeFactory.GetShape(shapeType);
        shape.InitializeRandomShpe(_tileFactory);
        _levelUIManager.DisplayShape(0, shape);
    }

    private TileShape GetRandomNewShape()
    {
        TileShape shape = _shapeFactory.GetRandomShape();
        shape.InitializeRandomShpe(_tileFactory);
        return shape;
    }

    // Update is called once per frame
    void Update()
    {
        spawnProgress += spawnSpeed * Time.deltaTime;
        while (spawnProgress >= 1f)
        {
            spawnProgress -= 1f;
            SpawnEnemy();
        }
        enemies.GameUpdate();


        if (Input.GetKeyDown(KeyCode.R)&&holdingShape!=null)
        {
            holdingShape.RotateShape();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            _levelUIManager.DisplayShape(0, GetRandomNewShape());
            _levelUIManager.DisplayShape(1, GetRandomNewShape());
            _levelUIManager.DisplayShape(2, GetRandomNewShape());
            _levelUIManager.ShowSelections();
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameTile tile = _board.GetTile();
            if (tile != null)
            {
                _board.ToggleTurret(tile);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameTile tile = _board.GetTile();
            if (tile != null)
            {
                _board.ToggleRock(tile);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            _board.ShowPaths = !_board.ShowPaths;
            _board.ShowTempTile = !_board.ShowTempTile;
        }
    }

    private void SpawnEnemy()
    {
        GameTile spawnPoint = _board.GetSpawnPoint(0);
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(spawnPoint);
        enemies.Add(enemy);
    }
}
