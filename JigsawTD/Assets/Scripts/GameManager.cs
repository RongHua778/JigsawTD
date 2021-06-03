using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //版图系统
    [SerializeField] private BoardSystem m_BoardSystem = default;

    //建造系统
    [SerializeField] private BuildingSystem m_BuildingSystem = default;


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



    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;
    [SerializeField]
    TurretAttributeFactory _turretFactory = default;


    [SerializeField] TileFactory _tileFactory = default;
    public TileFactory TileFactory { get => _tileFactory; }

    [SerializeField] GameTileContentFactory _contentFactory = default;
    public GameTileContentFactory ContentFactory { get => _contentFactory; }

    [SerializeField] TileShapeFactory _shapeFactory = default;
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }


    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();



    public EnemySpawner EnemySpawnHelper;

    //*********战斗中流程State
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; }


    private BuildingState buildingState;
    private WaveState waveState;
    //************


    //初始化设定
    public void Initinal()
    {
        //基本参数设置
        GameSpeed = 1;
        Difficulty = Game.Instance.Difficulty;

        //初始化工厂
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();

        //初始化系统
        m_BoardSystem.Initialize(this);//版图系统
        m_BuildingSystem.Initialize(this);//形状系统

        

        //_enemyFactory.InitializeFactory();
        //_tileFactory.InitializeFactory();
        //// _bluePrintFacotry.InitializeFactory();
        //_turretFactory.InitializeFacotory();

        //m_BoardSystem.SetGameBoard(_startSize, GroundSize, _tileFactory);

        //EnemySpawnHelper = this.GetComponent<EnemySpawner>();
        //EnemySpawnHelper.LevelInitialize(_enemyFactory, GameManager.Instance.difficulty);
        //_bluePrintShop.RefreshShop(0);

        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this);
        EnterNewState(buildingState);
    }

    //释放游戏系统
    public void Release()
    {
        GameSpeed = 1;
        m_BoardSystem.Release();
    }



    public void GameUpdate()
    {
        m_BoardSystem.GameUpdate();
        EnemySpawnHelper.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        turrets.GameUpdate();
        nonEnemies.GameUpdate();

        if (Input.GetKeyDown(KeyCode.R) && StaticData.holdingShape != null)
        {
            StaticData.holdingShape.RotateShape();
        }

    }



    public void SpawnEnemy(EnemySequence sequence)
    {
        Enemy enemy = EnemySpawnHelper.SpawnEnemy(sequence.EnemyAttribute, sequence.Intensify);
        GameTile tile = m_BoardSystem.SpawnPoint;
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
    }

    public void TransitionToState(StateName stateName)
    {
        switch (stateName)
        {
            case StateName.BuildingState:
                StartCoroutine(OperationState.ExitState(buildingState));
                break;
            case StateName.WaveState:
                StartCoroutine(OperationState.ExitState(waveState));
                break;
            case StateName.WonState:
                break;
            case StateName.LoseState:
                break;
        }
    }

    public void EnterNewState(BattleOperationState newState)
    {
        this.operationState = newState;
        StartCoroutine(this.operationState.EnterState());
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
        TileShape shape = _shapeFactory.GetRandomShape();
        GameTile randomElementTile = TileFactory.GetRandomElementTile();
        shape.InitializeShape(randomElementTile);
        return shape;
    }

    //消耗素材，生成合成塔及DShape供放置
    public TileShape GenerateCompositeShape(Blueprint bluePrint)
    {
        TileShape shape = _shapeFactory.GetDShape();
        TurretTile tile = TileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
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
        GameTile tile = TileFactory.GetBasicTurret(quality, element);
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
        GameTile tile = TileFactory.GetCompositeTurretTile(attribute);
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        Turret turret = ((TurretTile)tile).turret;
        ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
        shape.InitializeShape(tile);
    }

    //测试用，根据名字生成一个陷阱
    public void GetTrapByName(string name)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = TileFactory.GetTrapByName(name);
        shape.InitializeShape(tile);
    }
}
