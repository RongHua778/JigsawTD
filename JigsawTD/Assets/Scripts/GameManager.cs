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

    [Header("TIPS")]
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private BuyGroundTips m_BuyGroundTips = default;
    [SerializeField] private EnemyTips m_EnemyTips = default;

    [Header("工厂")]
    [SerializeField] TileFactory _tileFactory = default;
    [SerializeField] TileContentFactory _contentFactory = default;
    [SerializeField] TileShapeFactory _shapeFactory = default;
    [SerializeField] EnemyFactory _enemyFactory = default;
    [SerializeField] BlueprintFactory _bluePrintFacotry = default;
    [SerializeField] SkillFactory _skillFactory = default;
    public TileFactory TileFactory { get => _tileFactory; }
    public TileContentFactory ContentFactory { get => _contentFactory; }
    public TileShapeFactory ShapeFactory { get => _shapeFactory; }
    public EnemyFactory EnemyFactory { get => _enemyFactory; }
    public BlueprintFactory BluePrintFactory { get => _bluePrintFacotry; }
    public SkillFactory SkillFactory { get => _skillFactory; set => _skillFactory = value; }


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
        //888888888888
        Game.Instance.Difficulty = 5;
        //888888888888
        //初始化工厂
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        _enemyFactory.InitializeFactory();

        //形状生成外观类
        ConstructHelper.Initialize();

        //初始化系统
        BoardSystem.Initialize(this);//版图系统
        WaveSystem.Initialize(this);//波次系统
        m_CamControl.Initialize(this, m_MainUI);//摄像机控制

        //初始化UI
        m_MainUI.Initialize(this);//主界面顶部UI
        m_FuncUI.Initialize(this);//主界面功能UI
        m_GuideUI.Initialize(this, m_FuncUI, m_MainUI, m_BluePrintShopUI);//教学系统UI
        m_BluePrintShopUI.Initialize(this);//配方系统UI
        m_ShapeSelectUI.Initialize(this);//抽模块UI
        m_GameEndUI.Initialize(this);//游戏结束UI
        m_TrapTips.Initialize(this);//防御塔TIPS
        m_TurretTips.Initialize(this);//陷阱及其他TIPS
        m_BuyGroundTips.Initialize(this);//购买地板TIPS
        m_MessageUI.Initialize(this);//提示系统UI
        m_GuideVideo.Initialize(this);//教程视频UI
        m_EnemyTips.Initialize(this);//敌人TIPS

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
            m_MainUI.PrepareForGuide();
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

        m_MainUI.Release();
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
            m_MainUI.GameSpeed = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_MainUI.GameSpeed = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_MainUI.GameSpeed = 3;
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
        WaveSystem.GetSequence();
        m_BluePrintShopUI.NextRefreshTrun--;
        m_MainUI.PrepareNextWave(WaveSystem.RunningSequence,m_FuncUI.LuckyCoin);
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
        TransitionToState(StateName.BuildingState);
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



    //public void EnterNewState(BattleOperationState newState)
    //{
    //    this.operationState = newState;
    //    StartCoroutine(this.operationState.EnterState());
    //}

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

    public void BuyBluePrint(BluePrintGrid grid, int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_FuncUI.LuckyCoin++;
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

    public void SpawnEnemy(int type)
    {
        WaveSystem.SpawnEnemy(BoardSystem, type);
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
        if (StaticData.GetNodeWalkable(BoardSystem.SelectingTile))
        {
            ShowMessage("此处已经有地基");
            return;
        }
        if (ConsumeMoney(m_BoardSystem.BuyOneGroundMoney))
        {
            m_BoardSystem.BuyOneEmptyTile();
            Sound.Instance.PlayEffect("Sound_ConfirmShape");
            m_BuyGroundTips.Hide();
        }
    }

    #endregion

    #region TIPS
    public void ShowTurretTips(StrategyBase strategy)
    {
        m_TurretTips.ReadTurret(strategy);
        m_TurretTips.Show();
        m_TrapTips.Hide();
        m_BuyGroundTips.Hide();
    }

    public void ShowTrapTips(TrapContent trap)
    {
        m_TrapTips.ReadTrap(trap);
        m_TurretTips.Hide();
        m_BuyGroundTips.Hide();
        m_TrapTips.Show();
    }

    public void ShowBuyGroundTips()
    {
        m_BuyGroundTips.ReadInfo(m_BoardSystem.BuyOneGroundMoney);
        m_TurretTips.Hide();
        m_TrapTips.Hide();
        m_BuyGroundTips.Show();
    }

    public void ShowTempTips(string text, Vector2 pos)
    {
        m_TempTips.gameObject.SetActive(true);
        m_TempTips.SendText(text, pos);
    }

    public void ShowBluePrintTips(BluePrintGrid grid)
    {
        m_TrapTips.Hide();
        m_BuyGroundTips.Hide();
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
        m_EnemyTips.ReadSequenceInfo(m_WaveSystem);
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
    }

    #endregion
}
