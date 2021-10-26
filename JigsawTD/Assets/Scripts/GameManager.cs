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

    [Header("UI")]
    [SerializeField] private BluePrintShopUI m_BluePrintShopUI = default;
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    [SerializeField] private GuideUI m_GuideUI = default;
    [SerializeField] private GuideGirlUI m_GuideGirlUI = default;

    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private MessageUI m_MessageUI = default;
    [SerializeField] private GuideVideo m_GuideVideo = default;


    [Header("TIPS")]
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private BuyGroundTips m_BuyGroundTips = default;
    [SerializeField] private EnemyTips m_EnemyTips = default;
    [SerializeField] private TurretBaseTips m_TurretBaseTips = default;



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
        //初始化全局数据
        GameRes.Initialize(m_MainUI, m_FuncUI);

        //形状生成外观类
        ConstructHelper.Initialize();

        //初始化系统

        m_WaveSystem.Initialize();//波次系统
        m_CamControl.Initialize(m_MainUI);//摄像机控制
        m_BoardSystem.Initialize();//版图系统
        //初始化UI
        m_MainUI.Initialize();//主界面顶部UI
        m_FuncUI.Initialize();//主界面功能UI
        m_BluePrintShopUI.Initialize();//配方系统UI
        m_ShapeSelectUI.Initialize();//抽模块UI
        m_GameEndUI.Initialize();//游戏结束UI
        m_TrapTips.Initialize();//防御塔TIPS
        m_TurretTips.Initialize();//陷阱及其他TIPS
        m_BuyGroundTips.Initialize();//购买地板TIPS
        m_MessageUI.Initialize();//提示系统UI
        m_GuideVideo.Initialize();//教程视频UI
        m_EnemyTips.Initialize();//敌人TIPS
        m_TurretBaseTips.Initialize();//基座tips

        //m_GuideUI.Initialize(m_FuncUI, m_MainUI, m_BluePrintShopUI, m_ShapeSelectUI);//教学系统UI
        //m_GuideUI.Initialize();//IuserInterface初始化

        m_GuideGirlUI.Initialize(m_FuncUI, m_MainUI, m_BluePrintShopUI, m_ShapeSelectUI);
        m_GuideGirlUI.Initialize();

        //设置操作流程
        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this, m_WaveSystem, m_BoardSystem);
        pickingState = new PickingState(this);

        //开局准备下一波
        PrepareNextWave();

        //初始化商店
        RefreshShop(0);

        //关闭显示强制摆放位置
        m_BoardSystem.SetTutorialPoss(false);

        //初始化教程
        if (Game.Instance.Tutorial)
        {
            m_FuncUI.PrepareForGuide();
            m_MainUI.PrepareForGuide();
            m_BluePrintShopUI.PrepareForGuide();
            //m_GuideUI.Show();
            m_GuideGirlUI.Show();
        }
        else
        {

            ScaleAndMove.CanControl = true;//默认为锁镜头
        }
    }

    //释放游戏系统
    public void Release()
    {
        m_BoardSystem.Release();
        m_WaveSystem.Release();
        m_CamControl.Release();

        m_MainUI.Release();
        m_FuncUI.Release();
        //m_GuideUI.Release();
        m_GuideGirlUI.Release();
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
        m_BoardSystem.GameUpdate();
        m_WaveSystem.GameUpdate();
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
        if (OperationState.StateName == StateName.BuildingState)
        {
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
        if (GameRes.Life <= 0)//游戏失败
        {
            return;
        }
        if (GameRes.CurrentWave >= LevelManager.Instance.CurrentLevel.Wave)
        {
            GameEnd(true);
            return;
        }
        TransitionToState(StateName.BuildingState);

        m_WaveSystem.GetSequence();
        m_BluePrintShopUI.NextRefreshTrun--;
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence);
        m_FuncUI.PrepareNextWave();

        GameEvents.Instance.TutorialTrigger(TutorialType.NextWaveStart);
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
        GameEvents.Instance.TutorialTrigger(TutorialType.DrawBtnClick);
        TransitionToState(StateName.PickingState);
        m_FuncUI.Hide();
        m_ShapeSelectUI.ShowThreeShapes(m_FuncUI.ModuleLevel);
    }

    public void SelectShape()//选择了一个模块
    {
        m_ShapeSelectUI.ClearAllSelections();
        m_BoardSystem.SetTutorialPoss(true);//显示强制摆放位置
    }

    public void ConfirmShape()//放下了一个模块
    {
        TransitionToState(StateName.BuildingState);
        m_BoardSystem.CheckPathTrap();
        m_FuncUI.Show();
        m_BluePrintShopUI.CheckAllBluePrint();
        m_BoardSystem.SetTutorialPoss(false);//关闭显示强制摆放位置
        GameRes.ForcePlace = null;
        GameRes.PreSetShape = null;
        //新手引导
        GameEvents.Instance.TutorialTrigger(TutorialType.ConfirmShape);
    }

    public void CompositeShape(BluePrintGrid grid)//合成了一个防御塔
    {
        if (operationState.StateName == StateName.PickingState)
        {
            ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
            return;
        }
        if (operationState.StateName == StateName.WaveState)
        {
            ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        if (grid.BluePrint.CheckBuildable())
        {
            TransitionToState(StateName.PickingState);
            m_BluePrintShopUI.CompositeBluePrint(grid);
            m_FuncUI.Hide();
            m_BoardSystem.SetTutorialPoss(true);//显示强制摆放位置
        }
        else
        {
            ShowMessage(GameMultiLang.GetTraduction("LACKMATERIAL"));
        }

    }

    public void PreviewComposition(bool value, ElementType element = ElementType.DUST, int quality = 1)
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
        GameRes.Coin += (int)(amount * (1 + GameRes.OverallMoneyIntensify));
    }

    public void GainInterest()
    {
        int interest = Mathf.Min(100, (int)(GameRes.Coin * StaticData.Instance.CoinInterest));
        GameRes.Coin += interest;
    }

    public void GainDraw(int amount)
    {
        m_FuncUI.DrawRemain += amount;
    }

    public Enemy SpawnEnemy(EnemyType type, int pathIndex, float intensify)
    {
        return m_WaveSystem.SpawnEnemy(StaticData.Instance.EnemyFactory.Get(type), pathIndex, intensify);
    }

    public void RefreshShop(int cost)
    {
        if (ConsumeMoney(cost))
        {
            m_BluePrintShopUI.RefreshShop(m_FuncUI.ModuleLevel);
        }
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

    //public void TriggerGuide(TutorialType triggerType)
    //{
    //    m_GuideGirlUI.GuideTrigger(triggerType);
    //}

    public void BuyOneGround()
    {
        m_BoardSystem.BuyOneEmptyTile();
    }

    public void SwitchTrap(TrapContent trap)
    {
        if (operationState.StateName == StateName.PickingState)
        {
            ShowMessage(GameMultiLang.GetTraduction("PUTFIRST"));
            return;
        }
        if (operationState.StateName == StateName.WaveState)
        {
            ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        m_BoardSystem.SwitchTrap(trap);
    }

    public void IncreaseShopCapacity(int amount)
    {
        m_BluePrintShopUI.ShopCapacity += amount;
    }
    public void GetPerfectElement(int count)
    {
        GameRes.PerfectElementCount += count;
        m_BluePrintShopUI.SetPerfectElementCount(GameRes.PerfectElementCount);
    }

    public void CheckDetectSkill()
    {
        foreach (var turret in compositeTurrets.behaviors)
        {
            (((TurretContent)turret).Strategy).LandedTurretSkill();
        }
    }

    public void CheckDrawSkill()
    {
        foreach (var turret in compositeTurrets.behaviors)
        {
            (((TurretContent)turret).Strategy).DrawTurretSkill();
        }
    }

    public void SetModuleSystemDiscount(float discount)
    {
        if (m_FuncUI.ModuleLevel < StaticData.Instance.PlayerMaxLevel)
        {
            m_FuncUI.PlayerLvUpMoney = Mathf.RoundToInt(m_FuncUI.PlayerLvUpMoney * (1 - discount));
        }
    }
    #endregion

    #region TIPS
    public void ShowTurretTips(StrategyBase strategy, Vector2 pos)
    {
        HideTips();

        m_TurretTips.ReadTurret(strategy, pos);
        m_TurretTips.Show();
    }

    public void ShowTrapTips(TrapContent trap)
    {
        HideTips();
        m_TrapTips.ReadTrap(trap, GameRes.FreeTrapCount > 0 ? 0 : m_BoardSystem.SwitchTrapCost);
        m_TrapTips.Show();
    }

    public void ShowBuyGroundTips()
    {
        HideTips();
        m_BuyGroundTips.ReadInfo(GameRes.FreeGroundTileCount > 0 ? 0 : m_BoardSystem.BuyOneGroundMoney);
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
        //if (m_WaveSystem.LevelSequence[0] == null)
        //    return;
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
    public void SetBuyShapeCostDiscount(float discount)
    {
        m_FuncUI.BuyShapeCost = Mathf.RoundToInt(m_FuncUI.BuyShapeCost * (1 - discount));
    }
    public void SetFreeShapeCount(int count)
    {
        m_FuncUI.FreeShapeCount += count;
    }

    public void AddtoWishList()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
    }

    #endregion
}
