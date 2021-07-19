using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("系统")]
    [SerializeField] private BoardSystem m_BoardSystem = default;//版图系统
    [SerializeField] private WaveSystem m_WaveSystem = default;//波次系统
    [SerializeField] private ScaleAndMove m_CamControl = default;//摄像机控制系统
    public WaveSystem WaveSystem { get => m_WaveSystem; set => m_WaveSystem = value; }
    public BoardSystem BoardSystem { get => m_BoardSystem; set => m_BoardSystem = value; }

    [Header("UI")]
    [SerializeField] private BluePrintShopUI m_BluePrintShopUI = default;
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    [SerializeField] private GuideUI m_GuideUI = default;
    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private MessageUI m_MessageUI = default;
    [SerializeField] private GuideVideo m_GuideVideo = default;
    public MainUI MainUI { get => m_MainUI; set => m_MainUI = value; }


    [Header("TIPS")]
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private BuyGroundTips m_BuyGroundTips = default;
    [SerializeField] private EnemyTips m_EnemyTips = default;
    [SerializeField] private TurretBaseTips m_TurretBaseTips = default;

    [Header("工厂")]
    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    [SerializeField] BlueprintFactory _bluePrintFacotry = default;
    [SerializeField] SkillFactory _skillFactory = default;
    [SerializeField] NonEnemyFactory _nonEnemyFactory = default;
    [SerializeField] TaskFactory _taskFactory = default;
    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }
    public BlueprintFactory BluePrintFactory { get => _bluePrintFacotry; }
    public SkillFactory SkillFactory { get => _skillFactory; set => _skillFactory = value; }
    public NonEnemyFactory NonEnemyFactory { get => _nonEnemyFactory; set => _nonEnemyFactory = value; }
    public TaskFactory TaskFactory { get => _taskFactory; set => _taskFactory = value; }


    [Header("集合")]
    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection elementTurrets = new GameBehaviorCollection();
    public GameBehaviorCollection compositeTurrets = new GameBehaviorCollection();

    [Header("流程")]
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; set => operationState = value; }

    private BuildingState buildingState;
    private PickingState pickingState;
    private WaveState waveState;

    //初始化设定
    public void Initinal()
    {
        //初始化工厂
        TurretEffectFactory.Initialize();
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        EnemyFactory.InitializeFactory();
        TaskFactory.InitializeFactory();

        //形状生成外观类
        ConstructHelper.Initialize();

        //初始化系统
        m_BoardSystem.Initialize(this);//版图系统
        m_WaveSystem.Initialize(this);//波次系统
        m_CamControl.Initialize(this, MainUI);//摄像机控制

        //初始化UI
        MainUI.Initialize(this);//主界面顶部UI
        m_FuncUI.Initialize(this);//主界面功能UI
        m_GuideUI.Initialize(this, m_FuncUI, MainUI, m_BluePrintShopUI);//教学系统UI
        m_BluePrintShopUI.Initialize(this);//配方系统UI
        m_ShapeSelectUI.Initialize(this);//抽模块UI
        m_GameEndUI.Initialize(this);//游戏结束UI
        m_TrapTips.Initialize(this);//防御塔TIPS
        m_TurretTips.Initialize(this);//陷阱及其他TIPS
        m_BuyGroundTips.Initialize(this);//购买地板TIPS
        m_MessageUI.Initialize(this);//提示系统UI
        m_GuideVideo.Initialize(this);//教程视频UI
        m_EnemyTips.Initialize(this);//敌人TIPS
        m_TurretBaseTips.Initialize(this);//基座tips

        //设置操作流程
        buildingState = new BuildingState(this, BoardSystem);
        waveState = new WaveState(this, WaveSystem);
        pickingState = new PickingState(this);
        PrepareNextWave();

        //初始化商店
        RefreshShop(0);
        //初始化教程
        if (Game.Instance.Tutorial)
        {
            m_FuncUI.PrepareForGuide();
            MainUI.PrepareForGuide();
            m_BluePrintShopUI.ShopBtnObj.SetActive(false);
            m_GuideUI.Show();
            TriggerGuide(0);
        }
        else
        {

            ScaleAndMove.CanControl = true;//默认为锁镜头
        }
    }

    //释放游戏系统
    public void Release()
    {
        BoardSystem.Release();
        WaveSystem.Release();
        m_CamControl.Release();

        MainUI.Release();
        m_FuncUI.Release();
        m_GuideUI.Release();
        m_BluePrintShopUI.Release();
        m_ShapeSelectUI.Release();
        m_GameEndUI.Release();
        m_TrapTips.Release();
        m_TurretTips.Release();
        m_BuyGroundTips.Release();
        m_MessageUI.Release();
        m_GuideVideo.Release();
        m_EnemyTips.Release();

        Instance = null;
    }

    public void GameUpdate()
    {
        m_CamControl.GameUpdate();
        BoardSystem.GameUpdate();
        WaveSystem.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        elementTurrets.GameUpdate();
        compositeTurrets.GameUpdate();
        nonEnemies.GameUpdate();

        KeyboardControl();
    }


    private void KeyboardControl()
    {
        if (Game.Instance.Tutorial)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            MainUI.GameSpeed = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            MainUI.GameSpeed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            MainUI.GameSpeed = 3;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_BluePrintShopUI.ShopBtnClick();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_FuncUI.NextWaveBtnClick();
        }
    }

    #region 阶段控制
    public void StartNewWave()
    {
        if (OperationState.StateName == StateName.BuildingState)
        {
            //WaveSystem.EnemyRemain = WaveSystem.RunningSequence.index.Count;
            m_FuncUI.Hide();
            TransitionToState(StateName.WaveState);
            foreach (var turret in compositeTurrets.behaviors)
            {
                ((TurretContent)turret).Strategy.StartTurnSkills();
            }
        }
        //参数设置

    }
    public void PrepareNextWave()
    {
        TransitionToState(StateName.BuildingState);
        if (MainUI.Life <= 0)//游戏失败
        {
            GameEnd(false);
            return;
        }
        else if (MainUI.CurrentWave >= StaticData.Instance.LevelMaxWave)//游戏胜利
        {
            GameEnd(true);
            return;
        }
        WaveSystem.GetSequence();
        m_BluePrintShopUI.NextRefreshTrun--;
        MainUI.PrepareNextWave(WaveSystem.RunningSequence);
        m_FuncUI.PrepareNextWave();
        m_FuncUI.Show();

        TriggerGuide(6);
        TriggerGuide(7);
        TriggerGuide(8);
        //重置所有防御塔的回合临时加成
        foreach (var turret in elementTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnIntensify();
        }
        foreach (var turret in compositeTurrets.behaviors)
        {
            ((TurretContent)turret).Strategy.ClearTurnIntensify();
        }

    }

    public void GameEnd(bool win)
    {
        m_GameEndUI.Show();
        m_GameEndUI.SetGameResult(win);
    }

    public void TransitionToState(StateName stateName)
    {
        BattleOperationState state = null;
        switch (stateName)
        {
            case StateName.BuildingState:
                state = buildingState;
                break;
            case StateName.WaveState:
                state = waveState;
                break;
            case StateName.PickingState:
                state = pickingState;
                break;
            case StateName.WonState:
                break;
            case StateName.LoseState:
                break;
        }
        if (operationState == null)
        {
            operationState = state;
            StartCoroutine(operationState.EnterState());
        }
        else
        {
            StartCoroutine(OperationState.ExitState(state));
            operationState = state;
        }
    }



    #endregion

    #region 形状控制
    public void DrawShapes()
    {
        TransitionToState(StateName.PickingState);
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
        TransitionToState(StateName.BuildingState);

        m_FuncUI.Show();
        m_BluePrintShopUI.CheckAllBluePrint();

    }

    public void CompositeShape(BluePrintGrid grid)//合成了一个防御塔
    {
        if (operationState.StateName == StateName.PickingState)
        {
            ShowMessage("请先放置抽取模块");
            return;
        }
        if (operationState.StateName == StateName.WaveState)
        {
            ShowMessage("必须在非战斗阶段合成");
            return;
        }
        if (grid.BluePrint.CheckBuildable())
        {
            TransitionToState(StateName.PickingState);
            m_BluePrintShopUI.CompositeBluePrint(grid);
            m_FuncUI.Hide();
        }
        else
        {
            ShowMessage("合成所需素材不足");
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
        return MainUI.ConsumeMoney(cost);
    }

    public void ShowMessage(string text)
    {
        m_MessageUI.SetText(text);
    }

    public void GainMoney(int amount)
    {
        MainUI.Coin += (int)(amount * (1 + StaticData.OverallMoneyIntensify));
    }

    public void GainDraw(int amount)
    {
        m_FuncUI.DrawRemain += amount;
    }

    public Enemy SpawnEnemy(EnemyType type, int pathIndex, float intensify)
    {
        return WaveSystem.SpawnEnemy(BoardSystem, EnemyFactory.Get(type), pathIndex, intensify);
    }

    public void RefreshShop(int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_BluePrintShopUI.RefreshShop(m_FuncUI.PlayerLevel, cost);
        }
    }

    public void GetRandomBluePrint(bool isIntensify = false)
    {
        m_BluePrintShopUI.GetARandomBluePrintToPocket(m_FuncUI.PlayerLevel, isIntensify);
    }

    public void BuyBluePrint(BluePrintGrid grid, int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_BluePrintShopUI.MoveBluePrintToPocket(grid);
        }
    }

    public void ShowGuideVideo(int index)
    {
        m_GuideVideo.Show();
        m_GuideVideo.ShowPage(index);
    }

    public void TriggerGuide(int index)
    {
        if (Game.Instance.Tutorial)
            m_GuideUI.GuideTrigger(index);
    }

    public void BuyOneGround()
    {
        m_BoardSystem.BuyOneEmptyTile();
    }

    public void IncreaseShopCapacity()
    {
        m_BluePrintShopUI.ShopCapacity++;
    }

    #endregion

    #region TIPS
    public void ShowTurretTips(StrategyBase strategy)
    {
        HideTips();
        m_TurretTips.ReadTurret(strategy);
        m_TurretTips.Show();
    }

    public void ShowTrapTips(TrapContent trap)
    {
        HideTips();
        m_TrapTips.ReadTrap(trap);
        m_TrapTips.Show();
    }

    public void ShowBuyGroundTips()
    {
        HideTips();
        m_BuyGroundTips.ReadInfo(StaticData.FreeGroundTileCount > 0 ? 0 : m_BoardSystem.BuyOneGroundMoney);
        m_BuyGroundTips.Show();
    }

    public void ShowTempTips(string text, Vector2 pos)
    {
        m_TempTips.gameObject.SetActive(true);
        m_TempTips.SendText(text, pos);
    }

    public void ShowBluePrintTips(BluePrintGrid grid)
    {
        HideTips();
        m_TurretTips.ReadBluePrint(grid);
        m_TurretTips.Show();
    }
    public void HideTempTips()
    {
        m_TempTips.gameObject.SetActive(false);
    }

    public void ShowEnemyTips()
    {
        m_EnemyTips.Show();
        m_EnemyTips.ReadSequenceInfo(m_WaveSystem.RunningSequence);
    }

    public void ShowTurretBaseTips(TurretBaseContent content)
    {
        HideTips();
        m_TurretBaseTips.Show();
        m_TurretBaseTips.ReadTurretBase(content);
    }

    public void HideEnemyTips()
    {
        m_EnemyTips.Hide();
    }

    public void HideTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
        m_BuyGroundTips.Hide();
        m_TurretBaseTips.Hide();
    }

    #endregion

    #region 待加入功能
    public void GetPerfectElement(int count)
    {
        StaticData.PerfectElementCount += count;
        m_BluePrintShopUI.SetPerfectElementCount(StaticData.PerfectElementCount);
    }

    public void CheckDetectSkill()
    {
        foreach (var turret in compositeTurrets.behaviors)
        {
            ((StrategyComposite)(((TurretContent)turret).Strategy)).LandedTurretSkill();
        }
    }
    #endregion
}
