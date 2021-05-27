using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public GameBoard Board = default;


    public BluePrintShop _bluePrintShop = default;

    //_groundsize�ǵ�ͼÿһ���Ϸ��������
    //startSize�ǳ�ʼ���ɵ��з���Ĵ�С
    [SerializeField]
    Vector2Int _startSize, _groundSize = default;


    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;
    [SerializeField]
    TurretFactory _turretFactory = default;
    [SerializeField]
    public TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();

    //�ѳ������������Ž�ȥ���жϺϳɹ���ļ���
    //public List<Turret> turretsElements = new List<Turret>();

    //[SerializeField, Range(0.1f, 10f)]
    //float spawnSpeed = 100f;
    //float spawnProgress;

    //������ѡ��
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
        _bluePrintFacotry.InitializeFactory();

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
            LevelUIManager.Instance.HideTips();
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
                LevelUIManager.Instance.ShowTileTips(tile);
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

    public Blueprint GetSingleBluePrint(TurretAttribute attribute)
    {
        Blueprint bluePrint = _bluePrintFacotry.GetComposedTurret(attribute);
        return bluePrint;
    }

    public TileShape GenerateRandomBasicShape()
    {
        TileShape shape = _shapeFactory.GetBasicShape();
        GameTile randomElementTile = _tileFactory.GetRandomElementTile();
        shape.InitializeShape(randomElementTile, _tileFactory);
        return shape;
    }

    public TileShape GenerateDShape(TurretAttribute compositeAttribute)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = _tileFactory.GetCompositeTurretTile(compositeAttribute);
        shape.InitializeShape(tile);
        return shape;
    }

}
