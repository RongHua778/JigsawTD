using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //版图系统
    [SerializeField] private BoardSystem m_BoardSystem = default;

    //建造系统
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;

    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;

    //波次系统
    [SerializeField] private WaveSystem m_WaveSystem = default;


    public BluePrintShop _bluePrintShop = default;

    //关卡难度
    private int difficulty = 2;
    public int Difficulty { get => difficulty; set => difficulty = value; }

    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;
    [SerializeField]
    TurretAttributeFactory _turretFactory = default;


    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();




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
        Difficulty = Game.Instance.Difficulty;

        //初始化工厂
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        _enemyFactory.InitializeFactory();

        //形状生成外观类
        ConstructHelper.Initialize();

        //初始化系统
        m_BoardSystem.Initialize(this);//版图系统
        m_WaveSystem.Initialize(this);//波次系统


        m_MainUI.Initialize(this);//主界面顶部UI
        m_FuncUI.Initialize(this);//主界面功能UI
        m_ShapeSelectUI.Initialize(this);//抽模块UI

        //// _bluePrintFacotry.InitializeFactory();
        //_bluePrintShop.RefreshShop(0);

        SetGameBoard();//初始化版图

        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this);
        EnterNewState(buildingState);


    }


    //释放游戏系统
    public void Release()
    {
        m_BoardSystem.Release();
        m_ShapeSelectUI.Release();
        m_WaveSystem.Release();
    }



    public void GameUpdate()
    {
        m_BoardSystem.GameUpdate();
        m_WaveSystem.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        turrets.GameUpdate();
        nonEnemies.GameUpdate();
    }

    public void SetGameBoard()
    {
        m_BoardSystem.SetGameBoard();
    }

    public void StartNewWave()
    {
        m_WaveSystem.GetSequence();
        m_FuncUI.Hide();
        TransitionToState(StateName.WaveState);
    }


    public void PlayerDie()
    {
        throw new NotImplementedException();
    }

    public void DrawShapes()
    {
        //SHAPESELECTUI打开并配置3个随机形状供选择
        m_FuncUI.Hide();
        m_ShapeSelectUI.Show();
        m_ShapeSelectUI.ShowThreeShapes(m_FuncUI.PlayerLevel);
    }

    public void SelectShape()//选择了一个模块
    {
        m_ShapeSelectUI.ClearAllSelections();
        m_ShapeSelectUI.Hide();
    }

    public void ConfirmShape()//放下了一个模块
    {
        m_FuncUI.Show();
    }


    public void PrepareNextWave()
    {
        //_bluePrintShop.NextRefreshTrun--;
        //ResourcesManager.Instance.PrepareNextWave(m_WaveSystem.CurrentWave, m_BuildingSystem.DrawThisTurn);
        m_MainUI.PrepareNextWave();
        m_FuncUI.Show();
        //重置所有防御塔的回合临时加成
        foreach (var turret in turrets.behaviors)
        {
            ((TurretContent)turret).ClearTurnIntensify();
        }
        Sound.Instance.PlayBg("preparing");
    }



    public void SpawnEnemy(EnemySequence sequence)
    {
        Enemy enemy = m_WaveSystem.SpawnEnemy(sequence.EnemyAttribute, sequence.Intensify);
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

    ////生成随机形状，配置随机元素塔
    //public TileShape GenerateRandomBasicShape()
    //{
    //    TileShape shape = _shapeFactory.GetRandomShape();
    //    GameTile randomElementTile = TileFactory.GetRandomElementTile();
    //    shape.SetTile(randomElementTile);
    //    return shape;
    //}

    ////消耗素材，生成合成塔及DShape供放置
    //public TileShape GenerateCompositeShape(Blueprint bluePrint)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    TurretTile tile = TileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
    //    //将蓝图赋值给合成塔turret
    //    Turret turret = tile.turret;
    //    ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
    //    shape.SetTile(tile);
    //    return shape;
    //}


    ////测试用，基于元素，等级，获取对应元素塔
    //public void GetTestElement(int quality, int element)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    GameTile tile = TileFactory.GetBasicTurret(quality, element);
    //    shape.SetTile(tile);
    //}

    ////获取基本元素塔，基于元素类型，用于配置UI等
    //public TurretAttribute GetElementAttribute(Element element)
    //{
    //    return _turretFactory.GetElementsAttributes(element);
    //}

    ////根据玩家等级概率获取对应随机配方
    //public TurretAttribute GetRandomCompositeAttributeByLevel()
    //{
    //    return _turretFactory.GetRandomCompositionTurretByLevel();
    //}

    ////获取一个随机的配方
    //public TurretAttribute GetRandomCompositeAttribute()
    //{
    //    return _turretFactory.GetRandomCompositionTurret();
    //}

    ////测试用，根据名字生成一个合成塔
    //public void GetCompositeAttributeByName(string name)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    TurretAttribute attribute = _turretFactory.TestGetCompositeByName(name);
    //    GameTile tile = TileFactory.GetCompositeTurretTile(attribute);
    //    Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
    //    Turret turret = ((TurretTile)tile).turret;
    //    ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
    //    shape.SetTile(tile);
    //}

    ////测试用，根据名字生成一个陷阱
    //public void GetTrapByName(string name)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    GameTile tile = TileFactory.GetTrapByName(name);
    //    shape.SetTile(tile);
    //}
}
