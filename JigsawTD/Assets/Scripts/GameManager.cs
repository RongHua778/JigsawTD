using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("系统")]
    [SerializeField] private BoardSystem m_BoardSystem = default;//版图系统
    [SerializeField] private WaveSystem m_WaveSystem = default;//波次系统

    [Header("UI")]
    [SerializeField] private BluePrintShopUI m_BluePrintShopUI = default;
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private MessageUI m_MessageUI = default;

    [Header("TIPS")]
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private BluePrintTips m_BluePrintTips = default;

    [Header("工厂")]
    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    [SerializeField] BlueprintFactory _bluePrintFacotry = default;
    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }
    public BlueprintFactory BluePrintFactory { get => _bluePrintFacotry; }

    [Header("集合")]
    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection elementTurrets = new GameBehaviorCollection();
    public GameBehaviorCollection compositeTurrets = new GameBehaviorCollection();

    [Header("流程")]
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; }
    private BuildingState buildingState;
    private PickingState pickingState;
    private WaveState waveState;

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

        //初始化UI
        m_MainUI.Initialize(this);//主界面顶部UI
        m_FuncUI.Initialize(this);//主界面功能UI
        m_BluePrintShopUI.Initialize(this);//配方系统UI
        m_ShapeSelectUI.Initialize(this);//抽模块UI
        m_GameEndUI.Initialize(this);//游戏结束UI
        m_TrapTips.Initialize(this);//防御塔TIPS
        m_TurretTips.Initialize(this);//陷阱及其他TIPS
        m_MessageUI.Initialize(this);//提示系统UI

        //设置操作流程
        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this, m_WaveSystem);
        pickingState = new PickingState(this);
        EnterNewState(buildingState);

        //初始化商店
        RefreshShop(0);
    }

    //释放游戏系统
    public void Release()
    {
        m_BoardSystem.Release();
        m_WaveSystem.Release();

        m_MainUI.Release();
        m_FuncUI.Release();
        m_BluePrintShopUI.Release();
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
        elementTurrets.GameUpdate();
        compositeTurrets.GameUpdate();
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
        if (m_MainUI.Life <= 0)//游戏失败
        {
            GameEnd(false);
            return;
        }
        else if (m_MainUI.CurrentWave >= StaticData.Instance.LevelMaxWave)//游戏胜利
        {
            GameEnd(true);
            return;
        }
        m_BluePrintShopUI.NextRefreshTrun--;
        m_MainUI.PrepareNextWave();
        m_FuncUI.PrepareNextWave();
        m_FuncUI.Show();
        //重置所有防御塔的回合临时加成
        foreach (var turret in elementTurrets.behaviors)
        {
            ((TurretContent)turret).ClearTurnIntensify();
        }
        foreach (var turret in compositeTurrets.behaviors)
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
            case StateName.PickingState:
                StartCoroutine(OperationState.ExitState(pickingState));
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
        EnterNewState(pickingState);
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
        EnterNewState(buildingState);
        m_FuncUI.Show();
        m_BluePrintShopUI.CheckAllBluePrint();
    }

    public void CompositeShape(BluePrintGrid grid)//合成了一个防御塔
    {
        if (operationState.StateName != StateName.BuildingState)
        {
            ShowMessage("必须在非战斗或放置阶段合成");
            return;
        }
        if (grid.BluePrint.CheckBuildable())
        {
            EnterNewState(pickingState);
            m_BluePrintShopUI.CompositeBluePrint(grid);
            m_FuncUI.Hide();
        }
        else
        {
            ShowMessage("合成所需素材不足");
        }

    }

    public void BuyBluePrint(BluePrintGrid grid, int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_FuncUI.LuckPoint++;
            m_BluePrintShopUI.MoveBluePrintToPocket(grid);
        }
    }

    public void PreviewComposition(bool value, Element element = Element.Dust, int quality = 1)
    {
        m_BluePrintShopUI.PreviewComposition(value, element, quality);
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

    public void GainMoney(int amount)
    {
        m_MainUI.Coin += amount;
    }

    public void GainDraw(int amount)
    {
        m_FuncUI.DrawRemain += amount;
    }

    public void SpawnEnemy()
    {
        m_WaveSystem.SpawnEnemy(m_BoardSystem);
    }

    public void RefreshShop(int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_BluePrintShopUI.RefreshShop(m_FuncUI.PlayerLevel, cost);
        }
    }

    public void GetRandomBluePrint()
    {
        m_BluePrintShopUI.GetARandomBluePrintToPocket(m_FuncUI.PlayerLevel);
    }

    #endregion

    #region TIPS
    public void ShowTurretTips(TurretContent turret)
    {
        m_TurretTips.ReadTurret(turret);
        m_TurretTips.Show();
        m_TrapTips.Hide();
        m_BluePrintTips.Hide();
    }

    public void ShowTrapTips(TrapContent trap)
    {
        m_TrapTips.ReadTrap(trap);
        m_TurretTips.Hide();
        m_TrapTips.Show();
        m_BluePrintTips.Hide();
    }

    public void ShowTempTips(string text, Vector2 pos)
    {
        m_TempTips.gameObject.SetActive(true);
        m_TempTips.SendText(text,pos);
        //m_TempTips.SetPos(pos);
    }

    public void ShowBluePrintTips(BluePrintGrid grid)
    {
        m_TrapTips.Hide();
        m_TurretTips.Hide();
        m_BluePrintTips.ReadBluePrint(grid);
        m_BluePrintTips.Show();
    }
    public void HideTempTips()
    {
        m_TempTips.gameObject.SetActive(false);
    }

    public void HideTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
        m_BluePrintTips.Hide();
    }

    #endregion
}
