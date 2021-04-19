using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticData : Singleton<StaticData>
{
    [Header("LevelSetting")]
    public float MaxPersistime = default;
    public int StartMoney = default;
    public int InitMaxMoney = default;
    public int MaxMoney = default;
    public int BasicIncome = default;
    public float BasicIncomeInterval = default;
    public float InitNodeSpeed = default;
    public float NodeSpeed = default;
    public float NodeSpawnInterval = default;
    public GameObject NoTargetEffect = default;

    [Header("GameSetting")]
    public float GameSlowDownRate = default;

    [Header("BuffValue")]
    public float SlowDownRate = default;
    public float MagicRangeIntensify = default;
    public Color TowerRangeColor;
    public Color MagicRangeColor;


    public void GameSlowDown()
    {
        Time.timeScale = GameSlowDownRate;
    }

    public void GameSpeedResume()
    {
        Time.timeScale = 1;
    }

    public static int RandomNumber(float[] pros)
    {
        float total = 0f;
        foreach (float elem in pros)
        {
            total += elem;
        }
        float randomPoint = Random.value * total;
        for (int i = 0; i < pros.Length; i++)
        {
            if (randomPoint < pros[i])
            {
                return i;
            }
            else
            {
                randomPoint -= pros[i];
            }
        }
        return pros.Length - 1;

    }

}
