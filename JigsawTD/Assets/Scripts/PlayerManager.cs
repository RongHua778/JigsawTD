using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public LevelUIManager levelUIManager;
    public int[] levelMoney;
    public GameManager gameManager;
    int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            gameManager._tileFactory.LevelTileChance = value;
            levelUIManager.SynchronizeLabels();
        }
    }
    int playerLvUpMoney=50;
    public int PlayerLvUpMoney
    {
        get => playerLvUpMoney;
        set
        {
            playerLvUpMoney = value;
            levelUIManager.SynchronizeLabels();
        }
    }

    int lotteryDraw = 2;
    public int LotteryDraw
    {
        get => lotteryDraw;
        set
        {
            lotteryDraw = value;
            levelUIManager.SynchronizeLabels();
        }
    }
    //控制每回合加的幸运点数
    public int luckPointsProcess = 0;
    int luckyPoints = 5;
    public int LuckyPoints
    {
        get => luckyPoints;
        set
        {
            luckyPoints = value;
            levelUIManager.SynchronizeLabels();
        }
    }
    int playerCoin = 1000;
    public int PlayerCoin
    {
        get => playerCoin;
        set
        {
            playerCoin = value;
            levelUIManager.SynchronizeLabels();
        }
    }

    float playerHealth;
    public float PlayerHealth
    {
        get => playerHealth;
        set
        {
            playerHealth = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth);
            levelUIManager.SynchronizeLabels();
        }
    }

    private void Start()
    {
        PlayerHealth = StaticData.Instance.PlayerMaxHealth;
        PlayerLevel = playerLevel;
        PlayerLvUpMoney = playerLvUpMoney;
        LotteryDraw = lotteryDraw;
        LuckyPoints = luckyPoints;
        PlayerCoin = playerCoin;
    }
}
