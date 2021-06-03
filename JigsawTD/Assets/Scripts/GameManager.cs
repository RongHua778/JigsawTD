using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    //��ͼϵͳ
    [SerializeField] private BoardSystem m_BoardSystem = default;

    //����ϵͳ
    [SerializeField] private BuildingSystem m_BuildingSystem = default;


    public BluePrintShop _bluePrintShop = default;

    //��Ϸ�ٶ�
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

    //�ؿ��Ѷ�
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
        GameSpeed = 1;
        Difficulty = Game.Instance.Difficulty;

        //��ʼ������
        TileFactory.Initialize();
        ContentFactory.Initialize();
        ShapeFactory.Initialize();

        //��ʼ��ϵͳ
        m_BoardSystem.Initialize(this);//��ͼϵͳ
        m_BuildingSystem.Initialize(this);//��״ϵͳ

        

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

    //�ͷ���Ϸϵͳ
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


    //***************�����н��߷�����
    public Blueprint GetSingleBluePrint(TurretAttribute attribute)
    {
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        return bluePrint;
    }

    //���������״���������Ԫ����
    public TileShape GenerateRandomBasicShape()
    {
        TileShape shape = _shapeFactory.GetRandomShape();
        GameTile randomElementTile = TileFactory.GetRandomElementTile();
        shape.InitializeShape(randomElementTile);
        return shape;
    }

    //�����زģ����ɺϳ�����DShape������
    public TileShape GenerateCompositeShape(Blueprint bluePrint)
    {
        TileShape shape = _shapeFactory.GetDShape();
        TurretTile tile = TileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
        //����ͼ��ֵ���ϳ���turret
        Turret turret = tile.turret;
        ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
        shape.InitializeShape(tile);
        return shape;
    }


    //�����ã�����Ԫ�أ��ȼ�����ȡ��ӦԪ����
    public void GetTestElement(int quality, int element)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = TileFactory.GetBasicTurret(quality, element);
        shape.InitializeShape(tile);
    }

    //��ȡ����Ԫ����������Ԫ�����ͣ���������UI��
    public TurretAttribute GetElementAttribute(Element element)
    {
        return _turretFactory.GetElementsAttributes(element);
    }

    //������ҵȼ����ʻ�ȡ��Ӧ����䷽
    public TurretAttribute GetRandomCompositeAttributeByLevel()
    {
        return _turretFactory.GetRandomCompositionTurretByLevel();
    }

    //��ȡһ��������䷽
    public TurretAttribute GetRandomCompositeAttribute()
    {
        return _turretFactory.GetRandomCompositionTurret();
    }

    //�����ã�������������һ���ϳ���
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

    //�����ã�������������һ������
    public void GetTrapByName(string name)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = TileFactory.GetTrapByName(name);
        shape.InitializeShape(tile);
    }
}
