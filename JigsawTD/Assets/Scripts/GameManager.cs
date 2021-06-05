using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //系统
    [SerializeField] private BoardSystem m_BoardSystem = default;//版图系统
    [SerializeField] private WaveSystem m_WaveSystem = default;//波次系统

    //UI
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private MessageUI m_MessageUI = default;

    //TIPS
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;

    //工厂
    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }


    public BluePrintShop _bluePrintShop = default;

    //Behavior集合
    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();


    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;


    //流程
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; }
    private BuildingState buildingState;
    private WaveState waveState;
    //************


    //初始化设定
    public void Initinal()
    {
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
        m_GameEndUI.Initialize(this);//游戏结束UI
        m_TrapTips.Initialize(this);//防御塔TIPS
        m_TurretTips.Initialize(this);//陷阱及其他TIPS
        m_MessageUI.Initialize(this);

        //// _bluePrintFacotry.InitializeFactory();
        //_bluePrintShop.RefreshShop(0);

        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this, m_WaveSystem);
        EnterNewState(buildingState);


    }

    //释放游戏系统
    public void Release()
    {
        m_BoardSystem.Release();
        m_WaveSystem.Release();

        m_MainUI.Release();
        m_FuncUI.Release();
        m_ShapeSelectUI.Release();
        m_GameEndUI.Release();
        m_TrapTips.Release();
        m_TurretTips.Release();
        m_MessageUI.Release();

        Instance = null;
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

    #region 阶段控制
    public void StartNewWave()
    {
        m_FuncUI.Hide();
        TransitionToState(StateName.WaveState);
    }
    public void PrepareNextWave()
    {
        //_bluePrintShop.NextRefreshTrun--;
        if (m_MainUI.Life <= 0)//游戏失败
        {
            GameEnd(true);
            return;
        }
        else if (m_MainUI.CurrentWave >= StaticData.Instance.LevelMaxWave)//游戏胜利
        {
            GameEnd(false);
            return;
        }
        m_MainUI.PrepareNextWave();
        m_FuncUI.Show();
        //重置所有防御塔的回合临时加成
        foreach (var turret in turrets.behaviors)
        {
            ((TurretContent)turret).ClearTurnIntensify();
        }
        TransitionToState(StateName.BuildingState);
    }

    public void GameEnd(bool win)
    {
        m_GameEndUI.Show();
        m_GameEndUI.SetGameResult(win);
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

    #endregion

    #region 形状控制
    public void DrawShapes()
    {
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
    #endregion

    #region 通用功能
    public bool ConsumeMoney(int cost)
    {
        return m_MainUI.ConsumeMoney(cost);
    }

    public void ShowMessage(string text)
    {
        m_MessageUI.SetText(text);
    }
    #endregion

    #region TIPS
    public void ShowTurretTips(TurretContent turret)
    {
        m_TurretTips.ReadTurret(turret);
        m_TurretTips.Show();
        m_TrapTips.Hide();
    }

    public void ShowTrapTips(TrapContent trap)
    {
        m_TrapTips.ReadTrap(trap);
        m_TurretTips.Hide();
        m_TrapTips.Show();
    }

    public void ShowTempTips(string text,Vector2 pos)
    {
        m_TempTips.gameObject.SetActive(true);
        m_TempTips.SendText(text);
        m_TempTips.SetPos(pos);
    }
    public void HideTempTips()
    {
        m_TempTips.gameObject.SetActive(false);
    }

    public void HideTileTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
    }

    #endregion




    public void SpawnEnemy(EnemySequence sequence)
    {
        Enemy enemy = m_WaveSystem.SpawnEnemy(sequence.EnemyAttribute, sequence.Intensify);
        GameTile tile = m_BoardSystem.SpawnPoint;
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
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
