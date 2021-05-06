using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    EnemyCollection enemies = new EnemyCollection();

    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 100f;
    float spawnProgress;





    // Start is called before the first frame update
    void Start()
    {
        _tileFactory.InitializeFactory();
        _board.Initialize(_startSize,_groundSize, _tileFactory);
    }


    private TileShape GetRandomNewShape()
    {
        TileShape shape = _shapeFactory.GetRandomShape();
        shape.InitializeRandomShpe(_tileFactory);
        return shape;
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
        _board.GameUpdate();

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

    }

    private void SpawnEnemy()
    {
        GameTile tile = _board.SpawnPoint;
        Enemy enemy = _enemyFactory.Get();
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
    }
}
