using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcePlace
{
    public Vector2 ForcePos;
    public Vector2 ForceDir;
    public List<Vector2> GuidePos = new List<Vector2>();
    public ForcePlace(Vector2 forcePos, Vector2 forceDir, List<Vector2> guidePoss)
    {
        this.ForcePos = forcePos;
        this.ForceDir = forceDir;
        this.GuidePos = guidePoss;
    }
}
public class ShapeInfo
{
    public ShapeType ShapeType;
    public ElementType Element;
    public int Quality;
    public int TurretPos;

    public ShapeInfo(ShapeType type, ElementType element, int quality, int pos)
    {
        this.ShapeType = type;
        this.Element = element;
        this.Quality = quality;
        this.TurretPos = pos;

    }
}
public static class GameRes
{


    private static MainUI m_MainUI;
    private static FuncUI m_FuncUI;
    private static WaveSystem m_WaveSystem;

    [Header("��̬����")]
    public static int PerfectElementCount = 0;//����Ԫ������
    public static float OverallMoneyIntensify = 0;//��Ҽӳ�
    public static int FreeGroundTileCount = 0;//��ѵذ�����
    public static int FreeTrapCount = 0;//������廻λ����
    public static Action<StrategyBase> NextCompositeCallback = null;

    [Header("ͳ������")]
    public static DateTime LevelStart;
    public static DateTime LevelEnd;
    public static int TotalRefactor = 0;
    public static int TotalCooporative = 0;
    public static int TotalDamage = 0;
    private static int maxPath = 0;
    public static int MaxPath { get => maxPath; set { maxPath = value > maxPath ? value : maxPath; } }
    public static int MaxMark = 0;
    public static int GainGold = 0;

    [Header("ȫ��Ԫ�ؼӳ�")]
    public static float TempGoldIntensify = 0;
    public static float TempWoodIntensify = 0;
    public static float TempWaterIntensify = 0;
    public static float TempFireIntensify = 0;
    public static float TempDustIntensify = 0;

    //��������
    static ShapeInfo preSetShape;//Ԥ����״
    public static ShapeInfo PreSetShape { get => preSetShape; set => preSetShape = value; }

    static ForcePlace forcePlace;//ǿ�ư�λ
    public static ForcePlace ForcePlace { get => forcePlace; set => forcePlace = value; }



    private static int coin;
    public static int Coin//ӵ�д���
    {
        get => coin;
        set
        {
            coin = value;
            m_MainUI.Coin = coin;
        }
    }

    private static int life;
    public static int Life//��������ֵ
    {
        get => life;
        set
        {
            if (value <= 0)
            {
                if (life <= 0)
                    return;
                GameManager.Instance.GameEnd(false);
            }
            life = Mathf.Clamp(value, 0, LevelManager.Instance.CurrentLevel.PlayerHealth);
            m_MainUI.Life = life;
        }
    }
    private static int enemyRemain;
    public static int EnemyRemain 
    { 
        get => enemyRemain;
        set 
        {
            enemyRemain = value;
            if (enemyRemain <= 0 && !m_WaveSystem.RunningSpawn)
            {
                enemyRemain = 0;
                GameManager.Instance.PrepareNextWave();
            }
        }
    }

    public static int currentWave;
    public static int CurrentWave//��ǰ����
    {
        get => currentWave;
        set
        {
            currentWave = value;
        }
    }

    private static int buyShapeCost;
    public static int BuyShapeCost//�����۸�
    {
        get => buyShapeCost;
        set
        {
            buyShapeCost = Mathf.Max(0, value);
            m_FuncUI.BuyShapeCost = buyShapeCost;
        }
    }

    private static int switchMarkCost;
    public static int SwitchMarkCost
    {
        get => switchMarkCost;
        set
        {
            switchMarkCost = value;
        }
    }

    private static int moduleLevel;
    public static int ModuleLevel   //ģ��ȼ�
    {
        get => moduleLevel;
        set
        {
            moduleLevel = Mathf.Clamp(value, 1, 6);
            m_FuncUI.ModuleLevel = moduleLevel;
        }
    }

    private static float discountRate;
    public static float DiscountRate
    {
        get => discountRate;
        set
        {
            discountRate = Mathf.Min(0.5f, value);
            m_FuncUI.DiscountRate = discountRate;
        }
    }

    private static int shopCapacity;
    public static int ShopCapacity { get => shopCapacity; set => shopCapacity = value; }//�̵�����


    private static int maxLock;
    public static int MaxLock { get => maxLock; set => maxLock = value; }//���������

    public static void Initialize(MainUI mainUI, FuncUI funcUI,WaveSystem waveSystem)
    {
        LevelManager.Instance.SaveContents = new List<ContentStruct>();
        m_MainUI = mainUI;
        m_FuncUI = funcUI;
        m_WaveSystem = waveSystem;

        LevelStart = DateTime.Now;
        TotalRefactor = 0;
        TotalCooporative = 0;
        TotalDamage = 0;
        MaxPath = 0;
        MaxMark = 0;
        GainGold = 0;
        enemyRemain = 0;

        DiscountRate = 0.1f;
        ShopCapacity = 3;
        MaxLock = 1;
        ModuleLevel = 1;
        CurrentWave = 0;
        SwitchMarkCost = StaticData.Instance.SwitchTrapCost;
        Coin = StaticData.Instance.StartCoin;
        Life = LevelManager.Instance.CurrentLevel.PlayerHealth;
        BuyShapeCost = StaticData.Instance.BaseShapeCost;

        PerfectElementCount = 0;
        OverallMoneyIntensify = 0;
        FreeGroundTileCount = 0;
        FreeTrapCount = 0;

        TempGoldIntensify = 0;
        TempWoodIntensify = 0;
        TempWaterIntensify = 0;
        TempFireIntensify = 0;
        TempDustIntensify = 0;

        PreSetShape = null;
        ForcePlace = null;

    }

    public static bool CheckForcePlacement(Vector2 pos, Vector2 dir)
    {
        if (ForcePlace == null)
            return true;
        if (Vector2.SqrMagnitude(pos - ForcePlace.ForcePos) < 0.1f
            && (ForcePlace.ForceDir == Vector2.zero || Vector2.Dot(dir, ForcePlace.ForceDir) > 0.99f))
            return true;
        else
            return false;
    }

}
