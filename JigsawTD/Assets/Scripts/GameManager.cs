using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    public GameBoard Board = default;
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

    //_groundsize�ǵ�ͼÿһ���Ϸ��������
    //startSize�ǳ�ʼ���ɵ��з���Ĵ�С
    [SerializeField]
    Vector2Int _startSize, _groundSize = default;
    public Vector2Int GroundSize { get => _groundSize; set => _groundSize = value; }

    //[SerializeField]
    //GameTileContentFactory _contentFactory = default;
    [SerializeField]
    BlueprintFactory _bluePrintFacotry = default;
    [SerializeField]
    TileShapeFactory _shapeFactory = default;
    [SerializeField]
    TurretAttributeFactory _turretFactory = default;
    [SerializeField]
    TileFactory _tileFactory = default;
    [SerializeField]
    EnemyFactory _enemyFactory = default;

    public GameBehaviorCollection enemies = new GameBehaviorCollection();
    public GameBehaviorCollection nonEnemies = new GameBehaviorCollection();
    public GameBehaviorCollection turrets = new GameBehaviorCollection();


    //������ѡ��
    static GameObject selection;
    static float pressCounter = 0;
    public bool IsPressingTile = false;
    public bool IsLongPress { get => pressCounter >= 0.3f; }
    private static GameTile selectingTile;
    public static GameTile SelectingTile
    {
        get => selectingTile;
        set
        {
            if (selectingTile != null)
            {
                if (selectingTile.BasicTileType == BasicTileType.Turret)
                {
                    ((TurretTile)selectingTile).ShowTurretRange(false);
                }
                selectingTile = selectingTile == value ? null : value;
            }
            else
            {
                selectingTile = value;
            }
            if (selectingTile != null)
            {
                if (selectingTile.BasicTileType == BasicTileType.Turret)
                {
                    ((TurretTile)selectingTile).ShowTurretRange(true);
                }
                LevelUIManager.Instance.ShowTileTips(selectingTile);
                selection.transform.position = selectingTile.transform.position;
            }
            selection.SetActive(selectingTile != null);
        }

    }
    public EnemySpawner EnemySpawnHelper;

    //State
    private State state;
    public State State { get => state; }

    public BuildingState buildingState;
    public WaveState waveState;

    private void OnDisable()
    {
        GameSpeed = 1;
        GameEvents.Instance.onTileClick -= TileClick;
        GameEvents.Instance.onTileUp -= TileUp;
    }
    void Start()
    {
        Sound.Instance.BgVolume = 0.3f;

        GameEvents.Instance.onTileClick += TileClick;
        GameEvents.Instance.onTileUp += TileUp;

        selection = transform.Find("Selection").gameObject;

        buildingState = new BuildingState(this);
        waveState = new WaveState(this);
        state = buildingState;
        StartCoroutine(state.EnterState());

        _enemyFactory.InitializeFactory();
        _tileFactory.InitializeFactory();
        // _bluePrintFacotry.InitializeFactory();
        _turretFactory.InitializeFacotory();

        Board.Initialize(_startSize, GroundSize, _tileFactory);

        EnemySpawnHelper = this.GetComponent<EnemySpawner>();
        EnemySpawnHelper.LevelInitialize(_enemyFactory,GameManager.Instance.difficulty);
    }

    private void TileClick()
    {
        IsPressingTile = true;
    }

    private void TileUp(GameTile tile)
    {
        if (!IsLongPress)
        {
            LevelUIManager.Instance.HideTips();
            SelectingTile = tile;
        }
        IsPressingTile = false;
    }

    void Update()
    {
        EnemySpawnHelper.GameUpdate();
        enemies.GameUpdate();
        Physics2D.SyncTransforms();
        turrets.GameUpdate();
        nonEnemies.GameUpdate();

        if (Input.GetKeyDown(KeyCode.R) && StaticData.holdingShape != null)
        {
            StaticData.holdingShape.RotateShape();
        }


        if (IsPressingTile && Input.GetMouseButton(0))
        {
            pressCounter += Time.deltaTime;
        }
        else
        {
            pressCounter = 0;
        }


        if (SelectingTile != null)
        {
            selection.SetActive(true);
            selection.transform.position = SelectingTile.transform.position;
        }
        else
        {
            selection.SetActive(false);
        }

    }

    public void HideSelection()
    {
        selection.SetActive(false);
    }

    public void SpawnEnemy(EnemySequence sequence)
    {
        Enemy enemy = EnemySpawnHelper.SpawnEnemy(sequence.EnemyAttribute, sequence.Intensify);
        GameTile tile = Board.SpawnPoint;
        enemy.SpawnOn(tile);
        enemies.Add(enemy);
    }

    public void TransitionToState(StateName stateName)
    {
        switch (stateName)
        {
            case StateName.BuildingState:
                StartCoroutine(this.state.ExitState(buildingState));
                break;
            case StateName.WaveState:
                StartCoroutine(this.state.ExitState(waveState));
                break;
            case StateName.WonState:
                break;
            case StateName.LoseState:
                break;
        }
    }

    public void EnterNewState(State newState)
    {
        this.state = newState;
        StartCoroutine(this.state.EnterState());
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
        TileShape shape = _shapeFactory.GetBasicShape();
        GameTile randomElementTile = _tileFactory.GetRandomElementTile();
        shape.InitializeShape(randomElementTile, _tileFactory);
        return shape;
    }

    //�����زģ����ɺϳ�����DShape������
    public TileShape GenerateCompositeShape(Blueprint bluePrint)
    {
        TileShape shape = _shapeFactory.GetDShape();
        TurretTile tile = _tileFactory.GetCompositeTurretTile(bluePrint.CompositeTurretAttribute);
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
        GameTile tile = _tileFactory.GetBasicTurret(quality, element);
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
        GameTile tile = _tileFactory.GetCompositeTurretTile(attribute);
        Blueprint bluePrint = _bluePrintFacotry.GetRandomBluePrint(attribute);
        Turret turret = ((TurretTile)tile).turret;
        ((CompositeTurret)turret).CompositeBluePrint = bluePrint;
        shape.InitializeShape(tile);
    }

    //�����ã�������������һ������
    public void GetTrapByName(string name)
    {
        TileShape shape = _shapeFactory.GetDShape();
        GameTile tile = _tileFactory.GetTrapByName(name);
        shape.InitializeShape(tile);
    }
}
