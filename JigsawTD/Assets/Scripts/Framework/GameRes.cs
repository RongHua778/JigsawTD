using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRes
{
    private static MainUI m_MainUI;
    private static FuncUI m_FuncUI;

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
            life = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth[Game.Instance.SelectDifficulty]);
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
        Life = StaticData.Instance.PlayerMaxHealth[Game.Instance.SelectDifficulty];
    }

}
