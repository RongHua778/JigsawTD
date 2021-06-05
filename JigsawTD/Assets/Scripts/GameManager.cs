using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //��ͼϵͳ
    [SerializeField] private BoardSystem m_BoardSystem = default;

    //����ϵͳ
    [SerializeField] private ShapeSelectUI m_ShapeSelectUI = default;

    [SerializeField] private MainUI m_MainUI = default;
    [SerializeField] private FuncUI m_FuncUI = default;
    [SerializeField] private GameEndUI m_GameEndUI = default;


    //����ϵͳ
    [SerializeField] private WaveSystem m_WaveSystem = default;

    //������TIPS
    [SerializeField] private TurretTips m_TurretTips = default;
    [SerializeField] private TrapTips m_TrapTips = default;


    public BluePrintShop _bluePrintShop = default;

    //�ؿ��Ѷ�
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




    //*********ս��������State
    private BattleOperationState operationState;
    public BattleOperationState OperationState { get => operationState; }
    private BuildingState buildingState;
    private WaveState waveState;
    //************


    //��ʼ���趨
    public void Initinal()
    {
        //������������
        Difficulty = Game.Instance.Difficulty;

        //��ʼ������
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();
        _enemyFactory.InitializeFactory();

        //��״���������
        ConstructHelper.Initialize();

        //��ʼ��ϵͳ
        m_BoardSystem.Initialize(this);//��ͼϵͳ
        m_WaveSystem.Initialize(this);//����ϵͳ


        m_MainUI.Initialize(this);//�����涥��UI
        m_FuncUI.Initialize(this);//�����湦��UI
        m_ShapeSelectUI.Initialize(this);//��ģ��UI
        m_GameEndUI.Initialize(this);//��Ϸ����UI
        m_TrapTips.Initialize(this);//������TIPS
        m_TurretTips.Initialize(this);//���弰����TIPS

        //// _bluePrintFacotry.InitializeFactory();
        //_bluePrintShop.RefreshShop(0);

        SetGameBoard();//��ʼ����ͼ

        buildingState = new BuildingState(this, m_BoardSystem);
        waveState = new WaveState(this, m_WaveSystem);
        EnterNewState(buildingState);


    }


    //�ͷ���Ϸϵͳ
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
        m_FuncUI.Hide();
        TransitionToState(StateName.WaveState);
    }

    public void DrawShapes()
    {
        //SHAPESELECTUI�򿪲�����3�������״��ѡ��
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
        m_FuncUI.Show();
    }


    public void PrepareNextWave()
    {
        //_bluePrintShop.NextRefreshTrun--;

        if (m_MainUI.Life <= 0)//��Ϸʧ��
        {
            GameEnd(true);
            return;
        }
        else if (m_MainUI.CurrentWave >= StaticData.Instance.LevelMaxWave)//��Ϸʤ��
        {
            GameEnd(false);
            return;
        }

        m_MainUI.PrepareNextWave();
        m_FuncUI.Show();

        //�������з������Ļغ���ʱ�ӳ�
        foreach (var turret in turrets.behaviors)
        {
            ((TurretContent)turret).ClearTurnIntensify();
        }
        TransitionToState(StateName.BuildingState);
    }

    public bool ConsumeMoney(int cost)
    {
        return m_MainUI.ConsumeMoney(cost);
    }

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

    public void HideTileTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
    }

    public void GameEnd(bool win)
    {
        m_GameEndUI.Show();
        m_GameEndUI.SetGameResult(win);
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


    //***************�����н��߷�����
    public Blueprint GetSingleBluePrint(TurretAttribute attribute)
    {
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        return bluePrint;
    }

    ////���������״���������Ԫ����
    //public TileShape GenerateRandomBasicShape()
    //{
    //    TileShape shape = _shapeFactory.GetRandomShape();
    //    GameTile randomElementTile = TileFactory.GetRandomElementTile();
    //    shape.SetTile(randomElementTile);
    //    return shape;
    //}

    ////�����زģ����ɺϳ�����DShape������
    //public TileShape GenerateCompositeShape(Blueprint bluePrint)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    TurretTile tile = TileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
    //    //����ͼ��ֵ���ϳ���turret
    //    Turret turret = tile.turret;
    //    ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
    //    shape.SetTile(tile);
    //    return shape;
    //}


    ////�����ã�����Ԫ�أ��ȼ�����ȡ��ӦԪ����
    //public void GetTestElement(int quality, int element)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    GameTile tile = TileFactory.GetBasicTurret(quality, element);
    //    shape.SetTile(tile);
    //}

    ////��ȡ����Ԫ����������Ԫ�����ͣ���������UI��
    //public TurretAttribute GetElementAttribute(Element element)
    //{
    //    return _turretFactory.GetElementsAttributes(element);
    //}

    ////������ҵȼ����ʻ�ȡ��Ӧ����䷽
    //public TurretAttribute GetRandomCompositeAttributeByLevel()
    //{
    //    return _turretFactory.GetRandomCompositionTurretByLevel();
    //}

    ////��ȡһ��������䷽
    //public TurretAttribute GetRandomCompositeAttribute()
    //{
    //    return _turretFactory.GetRandomCompositionTurret();
    //}

    ////�����ã�������������һ���ϳ���
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

    ////�����ã�������������һ������
    //public void GetTrapByName(string name)
    //{
    //    TileShape shape = _shapeFactory.GetDShape();
    //    GameTile tile = TileFactory.GetTrapByName(name);
    //    shape.SetTile(tile);
    //}
}
