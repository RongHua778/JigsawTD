using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public GameBoard Board = default;
    public BluePrintShop _bluePrintShop = default;

    //游戏速度
    private float gameSpeed = 1;
    public float GameSpeed
    {
        get => gameSpeed;
        set
        {
            if (value > 3)
            {
                gameSpeed = 1;
            }
            else
            {
                gameSpeed = value;
            }
            Time.timeScale = gameSpeed;
        }
    }
    //关卡难度
    private int difficulty = 2;
    public int Difficulty { get => difficulty; set => difficulty = value; }

    //_groundsize是地图每一边上方块的数量
    //startSize是初始生成的有方块的大小
    [SerializeField]
    Vector2Int _startSize, _groundSize = default;
    public Vector2Int GroundSize { get => _groundSize; set => _groundSize = value; }

    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;
    [SerializeField]
    TurretAttributeFactory _turretFactory = default;
    [SerializeField]
    TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();


    //计算点击选中
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
    public EnemySpawner EnemySpawnHelper;

    //State
    private State state;
    public State State { get => state; }

    public BuildingState buildingState;
    public WaveState waveState;

    private void OnDisable()
    {
        GameSpeed = 1;
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }
    void Start()
    {
        Sound.Instance.BgVolume = 0.3f;

        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;

        selection = transform.Find("Selection").gameObject;

        buildingState = new BuildingState(this);
        waveState = new WaveState(this);
        state = buildingState;
        StartCoroutine(state.EnterState());

        _enemyFactory.InitializeFactory();
        _tileFactory.InitializeFactory();
        // _bluePrintFacotry.InitializeFactory();
        _turretFactory.InitializeFacotory();

        Board.Initialize(_startSize, GroundSize, _tileFactory);

        EnemySpawnHelper = this.GetComponent<EnemySpawner>();
        EnemySpawnHelper.LevelInitialize(_enemyFactory,GameManager.Instance.difficulty);
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


    //***************工厂中介者服务区
    public Blueprint GetSingleBluePrint(TurretAttribute attribute)
    {
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        return bluePrint;
    }

    //生成随机形状，配置随机元素塔
    public TileShape GenerateRandomBasicShape()
    {
        TileShape shape = _shapeFactory.GetBasicShape();
        GameTile randomElementTile = _tileFactory.GetRandomElementTile();
        shape.InitializeShape(randomElementTile, _tileFactory);
        return shape;
    }

    //消耗素材，生成合成塔及DShape供放置
    public TileShape GenerateCompositeShape(Blueprint bluePrint)
    {
        TileShape shape = _shapeFactory.GetDShape();
        TurretTile tile = _tileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
        //将蓝图赋值给合成塔turret
        Turret turret = tile.turret;
        ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
        shape.InitializeShape(tile);
        return shape;
    }


    //测试用，基于元素，等级，获取对应元素塔
    public void GetTestElement(int quality, int element)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = _tileFactory.GetBasicTurret(quality, element);
        shape.InitializeShape(tile);
    }

    //获取基本元素塔，基于元素类型，用于配置UI等
    public TurretAttribute GetElementAttribute(Element element)
    {
        return _turretFactory.GetElementsAttributes(element);
    }

    //根据玩家等级概率获取对应随机配方
    public TurretAttribute GetRandomCompositeAttributeByLevel()
    {
        return _turretFactory.GetRandomCompositionTurretByLevel();
    }

    //获取一个随机的配方
    public TurretAttribute GetRandomCompositeAttribute()
    {
        return _turretFactory.GetRandomCompositionTurret();
    }

    //测试用，根据名字生成一个合成塔
    public void GetCompositeAttributeByName(string name)
    {
        TileShape shape = _shapeFactory.GetDShape();
        TurretAttribute attribute = _turretFactory.TestGetCompositeByName(name);
        GameTile tile = _tileFactory.GetCompositeTurretTile(attribute);
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        Turret turret = ((TurretTile)tile).turret;
        ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
        shape.InitializeShape(tile);
    }

    //测试用，根据名字生成一个陷阱
    public void GetTrapByName(string name)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = _tileFactory.GetTrapByName(name);
        shape.InitializeShape(tile);
    }
}
