using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Canvas m_Canvas = default;
    [Header("系统")]
    [SerializeField] private BoardSystem m_BoardSystem = default;//版图系统
    [SerializeField] private WaveSystem m_WaveSystem = default;//波次系统
    [SerializeField] private ScaleAndMove m_CamControl = default;//摄像机控制系统

    [Header("UI")]
    [SerializeField] private BluePrintShopUI m_BluePrintShopUI = default;
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;
    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    //[SerializeField] private GuideUI m_GuideUI = default;
    [SerializeField] private GuideGirlUI m_GuideGirlUI = default;

    [SerializeField] private GameEndUI m_GameEndUI = default;
    [SerializeField] private MessageUI m_MessageUI = default;
    [SerializeField] private GuideVideo m_GuideVideo = default;
    [SerializeField] private PausePanel m_PausePanel = default;

    [Header("TIPS")]
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TempTips m_TempTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;
    [SerializeField] private BuyGroundTips m_BuyGroundTips = default;
    [SerializeField] private EnemyTips m_EnemyTips = default;
    [SerializeField] private UnlockBonusTips m_BonusTips = default;

    [Header("其他")]
    [SerializeField] private RectMaskController m_RectMaskController = default;
    [SerializeField] private EventPermeater m_EventPermeater = default;

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
    private EndState endState;
    private WonState wonState;

    public bool LockKeyboard = false;

    //初始化设定
    public void Initinal()
    {
        //形状生成外观类
        ConstructHelper.Initialize();
        //初始化系统
        //初始化全局数据
        GameRes.Initialize(m_MainUI, m_FuncUI, m_WaveSystem, m_BluePrintShopUI);

        m_MainUI.Initialize();//主界面顶部UI//要在wavesystem之前，因为敌人入侵事件，需要先掉血再判定下一波
        m_WaveSystem.Initialize();//波次系统
        m_CamControl.Initialize(m_MainUI);//摄像机控制
        m_BoardSystem.Initialize();//版图系统
        //初始化UI
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
        m_BonusTips.Initialize();//奖励解锁TIps
        m_PausePanel.Initialize();//暂停界面

        m_GuideGirlUI.Initialize();//教学小姐姐初始化

        //设置操作流程
        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this, m_WaveSystem, m_BoardSystem);
        pickingState = new PickingState(this);
        endState = new EndState(this);
        wonState = new WonState(this);

        //读取存档
        if (LevelManager.Instance.LastGameSave.HasLastGame)
        {
            GameRes.LoadSaveRes();
            m_WaveSystem.LoadSaveWave();
            m_BoardSystem.LoadSaveGame();
            m_BluePrintShopUI.LoadSaveGame();
            //继续保存的波
            ContinueWave();
        }
        else
        {
            m_WaveSystem.LevelInitialize();
            RefreshShop(0);
            m_BoardSystem.FirstGameSet();
            //开局准备下一波
            PrepareNextWave();
        }

        m_FuncUI.Show();
        m_MainUI.Show();
        //关闭显示强制摆放位置
        m_BoardSystem.SetTutorialPoss(false);
        m_GuideGirlUI.PrepareTutorial();

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
        if (LockKeyboard)//教学期间无法加速
        {
            return;
        }
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

        if (Input.GetKeyDown(KeyCode.Q)&&LevelManager.Instance.CurrentLevel.Mode!=0)
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
            GameEvents.Instance.TutorialTrigger(TutorialType.NextWaveBtnClick);
        }
        //参数设置

    }
    public void PrepareNextWave()
    {
        if (GameRes.Life <= 0)//游戏失败
        {
            TransitionToState(StateName.LoseState);
            return;
        }
        if (GameRes.CurrentWave >= LevelManager.Instance.CurrentLevel.Wave)
        {
            TransitionToState(StateName.WonState);
            GameEnd(true);
            return;
        }
        TransitionToState(StateName.BuildingState);

        GameRes.PrepareNextWave();
        m_WaveSystem.PrepareNextWave();
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence);
        m_FuncUI.Show();
        GameEvents.Instance.TutorialTrigger(TutorialType.NextWaveStart);
    }

    public void ContinueWave()
    {
        TransitionToState(StateName.BuildingState);
        m_WaveSystem.PrepareNextWave();
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence);
        m_FuncUI.Show();
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
                state = wonState;
                break;
            case StateName.LoseState:
                state = endState;
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

    public void RestartGame()
    {
        if (Game.Instance.OnTransition)
            return;
        LevelManager.Instance.LastGameSave.ClearGame();
        Game.Instance.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        if (Game.Instance.OnTransition)
            return;
        LevelManager.Instance.SaveAll();
        Game.Instance.LoadScene(0);
    }

    public void QuitGame()
    {
        if (Game.Instance.OnTransition)
            return;
        Game.Instance.QuitGame();
    }
    #endregion

    #region 形状控制
    public void DrawShapes()
    {
        GameEvents.Instance.TutorialTrigger(TutorialType.DrawBtnClick);
        TransitionToState(StateName.PickingState);
        m_FuncUI.Hide();
        m_ShapeSelectUI.ShowThreeShapes();
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
        GameRes.PreSetShape = new ShapeInfo[3];
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
        if (grid.Strategy.CheckBuildable())
        {
            TransitionToState(StateName.PickingState);
            m_BluePrintShopUI.RefactorBluePrint(grid);
            m_FuncUI.Hide();
            m_BoardSystem.SetTutorialPoss(true);//显示强制摆放位置
            GameEvents.Instance.TutorialTrigger(TutorialType.RefactorBtnClick);
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

    public void PauseGame()
    {
        m_PausePanel.Show();
    }
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
        int gold = (int)(amount * (1 + GameRes.OverallMoneyIntensify));
        GameRes.Coin += gold;
        GameRes.GainGold += gold;
    }

    public Enemy SpawnEnemy(EnemyType type, int pathIndex, float intensify)
    {
        return m_WaveSystem.SpawnEnemy(StaticData.Instance.EnemyFactory.Get(type), pathIndex, intensify);
    }

    public void RefreshShop(int cost)
    {
        m_BluePrintShopUI.RefreshShop(cost);
    }



    public void ShowGuideVideo(int index)
    {
        m_GuideVideo.Show();
        m_GuideVideo.ShowPage(index);
    }


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
        if (operationState.StateName != StateName.BuildingState)
        {
            ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
            return;
        }
        m_BoardSystem.SwitchTrap(trap);
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

    #endregion

    #region TIPS
    public void ShowTurretTips(StrategyBase strategy, Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_TurretTips.transform, pos);
        m_TurretTips.ReadTurret(strategy);
        m_TurretTips.Show();
    }

    private void SetCanvasPos(Transform tr, Vector2 pos)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, pos, m_Canvas.worldCamera, out newPos);
        tr.position = m_Canvas.transform.TransformPoint(newPos);
    }

    public void ShowTrapTips(TrapContent trap, Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_TrapTips.transform, pos);
        m_TrapTips.ReadTrap(trap,GameRes.SwitchTrapCost);
        m_TrapTips.Show();
    }

    public void ShowTrapTips(TrapAttribute att, Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_TrapTips.transform, pos);
        m_TrapTips.Show();
        m_TrapTips.ReadTrapAtt(att);
    }

    public void ShowTurretTips(TurretAttribute att, Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_TurretTips.transform, pos);
        m_TurretTips.Show();
        m_TurretTips.ReadAttribute(att);
    }

    public void ShowBuyGroundTips(Vector2 pos)
    {
        HideTips();
        SetCanvasPos(m_BuyGroundTips.transform, pos);
        m_BuyGroundTips.ReadInfo(GameRes.FreeGroundTileCount > 0 ? 0 : m_BoardSystem.BuyOneGroundMoney);
        m_BuyGroundTips.Show();
    }

    public void ShowTempTips(string text, Vector2 pos)
    {
        m_TempTips.gameObject.SetActive(true);
        m_TempTips.SendText(text, pos);
    }

    public void ShowBluePrintTips(BluePrintGrid grid, Vector2 pos)
    {
        //HideTips();
        SetCanvasPos(m_TurretTips.transform, pos);
        m_TurretTips.ReadBluePrint(grid);
        m_TurretTips.Show();
    }

    public void ShowBonusTips(GameLevelInfo info)
    {
        m_BonusTips.Show();
        m_BonusTips.SetBouns(info);
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



    public void HideEnemyTips()
    {
        m_EnemyTips.Hide();
    }

    public void HideTips()
    {
        m_TurretTips.CloseTips();
        m_TrapTips.CloseTips();
        m_BuyGroundTips.CloseTips();
    }

    #endregion

    #region 待加入功能
    public void LocateCamPos(Vector2 pos)
    {
        m_CamControl.LocatePos(pos);
    }
    public void AddtoWishList()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
    }

    public void SetCamMovable(bool value)
    {
        m_CamControl.CanControl = value;
    }

    public void SetSizeTutorial(bool value)
    {
        m_CamControl.SizeTutorial = value;
    }

    public void SetMoveTutorial(bool value)
    {
        m_CamControl.MoveTurorial = value;
    }

    public void SetRectMaskObj(GameObject obj,float delayTime)
    {
        m_RectMaskController.SetTarget(obj,delayTime);
    }

    public void SetEventPermeaterTarget(GameObject obj)
    {
        m_EventPermeater.SetTarget(obj);
    }

    public void ManualSetSequence(EnemyType type, float stage, int wave)
    {
        m_WaveSystem.ManualSetSequence(type, stage, wave);
        m_MainUI.PrepareNextWave(m_WaveSystem.RunningSequence);
    }

    public GameObject GetGuideObj(string objName)
    {
        return m_GuideGirlUI.GetGuideObj(objName);
    }

    public void ShowGuideGirl(bool value,int posID)
    {
        if (value)
        {
            m_GuideGirlUI.SetGirlPos(posID);
            m_GuideGirlUI.Show();
        }
        else
        {
            m_GuideGirlUI.Hide();
        }
    }

    public void AddBluePrint(RefactorStrategy strategy)
    {
        m_BluePrintShopUI.AddBluePrint(strategy);
    }

    public void RemoveBluePrint(int id)
    {
        m_BluePrintShopUI.RemoveGrid(BluePrintShopUI.ShopBluePrints[id]);
    }

    #endregion
}
