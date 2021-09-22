using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRes
{
    private static MainUI m_MainUI;
    private static FuncUI m_FuncUI;

    [Header("动态数据")]
    public static int PerfectElementCount = 0;//完美元素数量
    public static float OverallMoneyIntensify = 0;//金币加成
    public static int FreeGroundTileCount = 0;//免费地板数量
    public static int FreeTrapCount = 0;//免费陷阱换位数量
    public static Action<StrategyBase> NextCompositeCallback = null;

    [Header("全局元素加成")]
    public static float TempGoldIntensify=0;
    public static float TempWoodIntensify = 0;
    public static float TempWaterIntensify = 0;
    public static float TempFireIntensify = 0;
    public static float TempDustIntensify = 0;

    public static List<int> BattleElements = new List<int>();


    private static int coin;
    public static int Coin
    {
        get => coin;
        set
        {
            coin = value;
            m_MainUI.Coin = coin;
        }
    }

    private static int life;
    public static int Life
    {
        get => life;
        set
        {
            if (value <= 0)
            {
                GameManager.Instance.GameEnd(false);
            }
            life = Mathf.Clamp(value, 0, LevelManager.Instance.CurrentLevel.PlayerHealth);
            m_MainUI.Life = life;
        }
    }

    public static int currentWave;
    public static int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
        }
    }
    public static void Initialize(MainUI mainUI, FuncUI funcUI)
    {
        m_MainUI = mainUI;
        m_FuncUI = funcUI;
        CurrentWave = 0;
        Coin = StaticData.Instance.StartCoin;
        Life = LevelManager.Instance.CurrentLevel.PlayerHealth;

        PerfectElementCount = 0;
        OverallMoneyIntensify = 0;
        FreeGroundTileCount = 0;
        FreeTrapCount = 0;

        TempGoldIntensify = 0;
        TempWoodIntensify = 0;
        TempWaterIntensify = 0;
        TempFireIntensify = 0;
        TempDustIntensify = 0;

        SetBattleElements();
    }

    private static void SetBattleElements()
    {
        foreach(var select in Game.Instance.SaveData.SaveSelectedElement)
        {
            if(select.isSelect)
                BattleElements.Add(select.id);
        }
    }

}
