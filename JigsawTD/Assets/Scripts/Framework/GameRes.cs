using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
[System.Serializable]
public class ShapeInfo
{
    public ShapeType ShapeType;
    public ElementType Element;
    public int Quality;
    public int TurretPos;
    public int TurretDir;

    public ShapeInfo(ShapeType type, ElementType element, int quality, int pos,int dir)
    {
        this.ShapeType = type;
        this.Element = element;
        this.Quality = quality;
        this.TurretPos = pos;
        this.TurretDir = dir;
    }
}
public static class GameRes
{
    public static GameResStruct SaveRes => SetSaveRes();

    private static MainUI m_MainUI;
    private static FuncUI m_FuncUI;
    private static BluePrintShopUI m_BluePrintShop;
    private static WaveSystem m_WaveSystem;

    [Header("动态数据")]
    public static float OverallMoneyIntensify = 0;//金币加成
    public static int FreeGroundTileCount = 0;//免费地板数量
    public static Action<StrategyBase> NextCompositeCallback = null;

    [Header("统计数据")]
    public static DateTime LevelStart;
    public static DateTime LevelEnd;
    public static int TotalRefactor = 0;
    public static int TotalCooporative = 0;
    public static int TotalDamage = 0;
    private static int maxPath = 0;
    public static int MaxPath { get => maxPath; set { maxPath = value > maxPath ? value : maxPath; } }
    public static int MaxMark = 0;
    public static int GainGold = 0;

    [Header("全局元素加成")]
    public static float TempGoldIntensify = 0;
    public static float TempWoodIntensify = 0;
    public static float TempWaterIntensify = 0;
    public static float TempFireIntensify = 0;
    public static float TempDustIntensify = 0;

    //新手引导
    static ShapeInfo[] preSetShape;//预设形状
    public static ShapeInfo[] PreSetShape { get => preSetShape; set => preSetShape = value; }

    static ForcePlace forcePlace;//强制摆位
    public static ForcePlace ForcePlace { get => forcePlace; set => forcePlace = value; }

    private static int perfectElementCount;
    public static int PerfectElementCount
    {
        get => perfectElementCount;
        set
        {
            perfectElementCount = value;
            m_BluePrintShop.PerfectElementCount = perfectElementCount;
        }
    }

    private static int nextRefreshTurn;
    public static int NextRefreshTurn
    {
        get => nextRefreshTurn;
        set
        {
            if (value <= 0)
            {
                nextRefreshTurn = 3;
                GameManager.Instance.RefreshShop(0);
            }
            else
            {
                nextRefreshTurn = value;
            }
            m_BluePrintShop.NextRefreshTrun = nextRefreshTurn;
        }
    }

    private static bool drawThisTurn;
    public static bool DrawThisTurn
    {
        get => drawThisTurn;
        set
        {
            drawThisTurn = value;
        }
    }

    private static int coin;
    public static int Coin//拥有代币
    {
        get => coin;
        set
        {
            coin = value;
            m_MainUI.Coin = coin;
        }
    }

    private static int life;
    public static int Life//当亲生命值
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
    public static int CurrentWave//当前波数
    {
        get => currentWave;
        set
        {
            currentWave = value;
            m_MainUI.CurrentWave = currentWave;
        }
    }

    private static int buildCost;
    public static int BuildCost//构建价格
    {
        get => buildCost;
        set
        {
            buildCost = Mathf.Max(0, value);
            m_FuncUI.BuyShapeCost = buildCost;
        }
    }

    private static int switchTrapCost;
    public static int SwitchTrapCost
    {
        get => switchTrapCost;
        set
        {
            switchTrapCost = value;
        }
    }

    private static int systemLevel;
    public static int SystemLevel   //模块等级
    {
        get => systemLevel;
        set
        {
            systemLevel = Mathf.Clamp(value, 1, 6);
            SystemUpgradeCost = StaticData.Instance.LevelUpMoney[systemLevel];
            if (systemLevel == 2 || systemLevel == 4 || systemLevel == 6)//2，4,6级增加一个商店容量
            {
                ShopCapacity++;
            }
            m_FuncUI.SystemLevel = systemLevel;
        }
    }

    private static int systemUpgradeCost;
    public static int SystemUpgradeCost
    {
        get => systemUpgradeCost;
        set
        {
            systemUpgradeCost = value;
            m_FuncUI.SystemUpgradeCost = systemUpgradeCost;
        }

    }

    private static float discountRate;
    public static float BuildDiscount
    {
        get => discountRate;
        set
        {
            discountRate = Mathf.Min(0.5f, value);
            m_FuncUI.DiscountRate = discountRate;
        }
    }

    private static int shopCapacity;
    public static int ShopCapacity { get => shopCapacity; set => shopCapacity = value; }//商店容量


    private static int maxLock;
    public static int MaxLock { get => maxLock; set => maxLock = value; }//最大锁定量

    public static void Initialize(MainUI mainUI, FuncUI funcUI, WaveSystem waveSystem, BluePrintShopUI bluePrintShop)
    {
        m_MainUI = mainUI;
        m_FuncUI = funcUI;
        m_WaveSystem = waveSystem;
        m_BluePrintShop = bluePrintShop;

        DrawThisTurn = true;
        TotalRefactor = 0;
        TotalCooporative = 0;
        TotalDamage = 0;
        NextRefreshTurn = 4;
        BuildDiscount = 0.1f;
        ShopCapacity = 3;
        SystemLevel = 1;
        CurrentWave = 0;
        SwitchTrapCost = StaticData.Instance.SwitchTrapCost;
        Coin = StaticData.Instance.StartCoin;
        Life = LevelManager.Instance.CurrentLevel.PlayerHealth;
        BuildCost = StaticData.Instance.BaseShapeCost;

        PerfectElementCount = 0;

        LevelStart = DateTime.Now;
        MaxPath = 0;
        MaxMark = 0;
        GainGold = 0;
        enemyRemain = 0;

        MaxLock = 1;

        OverallMoneyIntensify = 0;
        FreeGroundTileCount = 0;

        TempGoldIntensify = 0;
        TempWoodIntensify = 0;
        TempWaterIntensify = 0;
        TempFireIntensify = 0;
        TempDustIntensify = 0;

        PreSetShape = new ShapeInfo[3];
        ForcePlace = null;

    }


    private static GameResStruct SetSaveRes()
    {
        GameResStruct resStruct = new GameResStruct();
        resStruct.Mode = LevelManager.Instance.CurrentLevel.Mode;
        resStruct.Coin = Coin;
        resStruct.Wave = CurrentWave;
        resStruct.CurrentLife = Life;
        resStruct.MaxLife = LevelManager.Instance.CurrentLevel.PlayerHealth;
        resStruct.BuildCost = BuildCost;
        resStruct.BuildDiscount = BuildDiscount;
        resStruct.SwitchTrapCost = SwitchTrapCost;
        resStruct.SystemLevel = SystemLevel;
        resStruct.SystemUpgradeCost = systemUpgradeCost;
        resStruct.TotalCooporative = TotalCooporative;
        resStruct.TotalDamage = TotalDamage;
        resStruct.TotalRefactor = TotalRefactor;
        resStruct.ShopCapacity = ShopCapacity;
        resStruct.NextRefreshTurn = NextRefreshTurn;
        resStruct.PefectElementCount = PerfectElementCount;
        resStruct.DrawThisTurn = DrawThisTurn;
        return resStruct;

    }

    public static void LoadSaveRes()
    {
        GameResStruct saveRes = LevelManager.Instance.LastGameSave.SaveRes;
        Coin = saveRes.Coin;
        CurrentWave = saveRes.Wave;//prepareNextWave导致+1
        Life = saveRes.CurrentLife;
        BuildCost = saveRes.BuildCost;
        BuildDiscount = saveRes.BuildDiscount;
        SwitchTrapCost = saveRes.SwitchTrapCost;
        SystemLevel = saveRes.SystemLevel;
        SystemUpgradeCost = saveRes.SystemUpgradeCost;
        TotalCooporative = saveRes.TotalCooporative;
        TotalRefactor = saveRes.TotalRefactor;
        ShopCapacity = saveRes.ShopCapacity;
        NextRefreshTurn = saveRes.NextRefreshTurn;
        PerfectElementCount = saveRes.PefectElementCount;
        DrawThisTurn = saveRes.DrawThisTurn;
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

    public static void PrepareNextWave()
    {
        NextRefreshTurn--;
        CurrentWave++;

        //获得回合金币
        GameManager.Instance.GainMoney(Mathf.Min(300, (StaticData.Instance.BaseWaveIncome +
        StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1))));
        BuildCost = Mathf.RoundToInt(BuildCost * (1 - BuildDiscount));

        //没抽就减5%的价格
        //if (!DrawThisTurn)
        //{
        //    BuildCost = Mathf.RoundToInt(BuildCost * (1 - BuildDiscount));
        //}
        DrawThisTurn = false;
    }

}
