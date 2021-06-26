using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    [Header("ϵͳ")]
    [SerializeField] private BoardSystem m_BoardSystem = default;//��ͼϵͳ
    [SerializeField] private WaveSystem m_WaveSystem = default;//����ϵͳ
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

    [Header("����")]
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


    [Header("����")]
    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection elementTurrets = new GameBehaviorCollection();
    public GameBehaviorCollection compositeTurrets = new GameBehaviorCollection();

    [Header("����")]
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; set => operationState = value; }

    private BuildingState buildingState;
    private PickingState pickingState;
    private WaveState waveState;

    //��ʼ���趨
    public void Initinal()
    {
        //��ʼ������
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        _enemyFactory.InitializeFactory();

        //��״���������
        ConstructHelper.Initialize();

        //��ʼ��ϵͳ
        BoardSystem.Initialize(this);//��ͼϵͳ
        WaveSystem.Initialize(this);//����ϵͳ

        //��ʼ��UI
        m_MainUI.Initialize(this);//�����涥��UI
        m_FuncUI.Initialize(this);//�����湦��UI
        m_GuideUI.Initialize(this, m_FuncUI, m_MainUI,m_BluePrintShopUI);//��ѧϵͳUI
        m_BluePrintShopUI.Initialize(this);//�䷽ϵͳUI
        m_ShapeSelectUI.Initialize(this);//��ģ��UI
        m_GameEndUI.Initialize(this);//��Ϸ����UI
        m_TrapTips.Initialize(this);//������TIPS
        m_TurretTips.Initialize(this);//���弰����TIPS
        m_BuyGroundTips.Initialize(this);//����ذ�TIPS
        m_MessageUI.Initialize(this);//��ʾϵͳUI
        m_GuideVideo.Initialize(this);//�̳���ƵUI

        //���ò�������
        buildingState = new BuildingState(this, BoardSystem);
        waveState = new WaveState(this, WaveSystem);
        pickingState = new PickingState(this);
        PrepareNextWave();

        //��ʼ���̵�
        RefreshShop(0);
        //��ʼ���̳�
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

            ScaleAndMove.CanControl = true;//Ĭ��Ϊ����ͷ
        }
    }

    //�ͷ���Ϸϵͳ
    public void Release()
    {
        BoardSystem.Release();
        WaveSystem.Release();

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

        Instance = null;
    }

    public void GameUpdate()
    {
        BoardSystem.GameUpdate();
        WaveSystem.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        elementTurrets.GameUpdate();
        compositeTurrets.GameUpdate();
        nonEnemies.GameUpdate();
    }

    #region �׶ο���
    public void StartNewWave()
    {
        m_FuncUI.Hide();
        TransitionToState(StateName.WaveState);
    }
    public void PrepareNextWave()
    {
        if (m_MainUI.Life <= 0)//��Ϸʧ��
        {
            GameEnd(false);
            return;
        }
        else if (m_MainUI.CurrentWave >= StaticData.Instance.LevelMaxWave)//��Ϸʤ��
        {
            GameEnd(true);
            return;
        }
        WaveSystem.GetSequence();
        m_BluePrintShopUI.NextRefreshTrun--;
        m_MainUI.PrepareNextWave(WaveSystem.RunningSequence);
        m_FuncUI.PrepareNextWave();
        m_FuncUI.Show();

        TriggerGuide(6);
        TriggerGuide(7);
        TriggerGuide(8);
        //�������з������Ļغ���ʱ�ӳ�
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
        BattleOperationState state=null;
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

    #region ��״����
    public void DrawShapes()
    {
        TransitionToState(StateName.PickingState);
        m_FuncUI.Hide();
        m_ShapeSelectUI.Show();
        m_ShapeSelectUI.ShowThreeShapes(m_FuncUI.PlayerLevel);
    }

    public void SelectShape()//ѡ����һ��ģ��
    {
        m_ShapeSelectUI.ClearAllSelections();
        m_ShapeSelectUI.Hide();
    }

    public void ConfirmShape()//������һ��ģ��
    {
        TransitionToState(StateName.BuildingState);

        m_FuncUI.Show();
        m_BluePrintShopUI.CheckAllBluePrint();

    }

    public void CompositeShape(BluePrintGrid grid)//�ϳ���һ��������
    {
        if (operationState.StateName == StateName.PickingState)
        {
            ShowMessage("���ȷ��ó�ȡģ��");
            return;
        }
        if (operationState.StateName == StateName.WaveState)
        {
            ShowMessage("�����ڷ�ս���׶κϳ�");
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
            ShowMessage("�ϳ������زĲ���");
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

    #region ͨ�ù���
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
        WaveSystem.SpawnEnemy(BoardSystem);
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
            ShowMessage("�˴��Ѿ��еػ�");
            return;
        }
        if (ConsumeMoney(m_BoardSystem.BuyOneGroundMoney))
        {
            m_BoardSystem.BuyOneEmptyTile();
            m_BuyGroundTips.Hide();
        }
    }

    #endregion

    #region TIPS
    public void ShowTurretTips(BasicStrategy strategy)
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

    public void HideTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
        m_BuyGroundTips.Hide();
    }

    #endregion
}
