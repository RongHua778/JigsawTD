using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public GameBoard Board = default;

    public LevelUIManager _levelUIManager = default;

    //地图每一边上方块的数量
    [SerializeField]
    Vector2Int _startSize, _groundSize = default;


    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;

    [SerializeField]
    TurretFactory _turretFactory = default;

    public TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();

    //把场上所有塔都放进去以判断合成规则的集合
    public List<Turret> turretsElements = new List<Turret>();

    [SerializeField, Range(0.1f, 10f)]
    float spawnSpeed = 100f;
    float spawnProgress;

    //计算点击选中
    [SerializeField] GameObject selection = default;
    static float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.3f; }
    public static GameTile SelectingTile = null;
    public EnemySpawner EnemySpawnHelper;

    //State
    private State state;
    public State State { get => state; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; set => _shapeFactory = value; }
    public TurretFactory TurretFactory { get => _turretFactory; set => _turretFactory = value; }

    public BuildingState buildingState;
    public WaveState waveState;

    public PlayerManager playerManager;
    private void OnDisable()
    {
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }
    void Start()
    {
        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;

        buildingState = new BuildingState(this);
        waveState = new WaveState(this);
        state = buildingState;
        StartCoroutine(state.EnterState());

        _enemyFactory.InitializeFactory();
        _tileFactory.InitializeFactory();
        Board.Initialize(_startSize, _groundSize, _tileFactory);

        EnemySpawnHelper = this.GetComponent<EnemySpawner>();
        EnemySpawnHelper.LevelInitialize(_enemyFactory);
    }

    private void TileClick()
    {
        IsPressingTile = true;
    }

    private void TileUp(GameTile tile)
    {
        if (!IsLongPress)
        {
            _levelUIManager.HideTips();
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
                {
                    ((TurretTile)tile).ShowTurretRange(true);
                }
                _levelUIManager.ShowTileTips(tile);
                SelectingTile = tile;
            }
        }
        IsPressingTile = false;
    }

    void Update()
    {
        EnemySpawnHelper.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        turrets.GameUpdate();
        nonEnemies.GameUpdate();

        if (Input.GetKeyDown(KeyCode.R) && StaticData.holdingShape != null)
        {
            StaticData.holdingShape.RotateShape();
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

    public void SpawnEnemy(EnemySequence sequence)
    {
        Enemy enemy = EnemySpawnHelper.SpawnEnemy(sequence.EnemyAttribute, sequence.Intensify);
        GameTile tile = Board.SpawnPoint;
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
    }

    public void TransitionToState(StateName stateName)
    {
        switch (stateName)
        {
            case StateName.BuildingState:
                StartCoroutine(this.state.ExitState(buildingState));
                break;
            case StateName.WaveState:
                StartCoroutine(this.state.ExitState(waveState));
                break;
            case StateName.WonState:
                break;
            case StateName.LoseState:
                break;
        }
    }

    public void EnterNewState(State newState)
    {
        this.state = newState;
        StartCoroutine(this.state.EnterState());
    }

}
