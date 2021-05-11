﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StaticData : Singleton<StaticData>
{

    [Header("LevelSetting")]
    public static DraggingShape holdingShape;
    public static LayerMask PathLayer = 1 << 6 | 1 << 10;
    public static string ConcreteTileMask = "ConcreteTile";
    public static string GroundTileMask = "GroundTile";
    public static string TempTileMask = "TempTile";
    public static string TurretMask = "Turret";

    [Header("GameSetting")]
    public float GameSlowDownRate = default;
    public float TileSize = default;

    [Header("ProbabilitySetting")]
    public float[] TileShapeChance = default;


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

    public static int[] GetRandomSequence(int total, int count)
    {
        int[] sequence = new int[total];
        int[] output = new int[count];

        for (int i = 0; i < total; i++)
        {
            sequence[i] = i;
        }
        int end = total - 1;
        for (int i = 0; i < count; i++)
        {
            int num = Random.Range(0, end + 1);
            output[i] = sequence[num];
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }

    public static List<Vector2> GetCirclePoints(int range)
    {
        List<Vector2> pointsToRetrun = new List<Vector2>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -(range - Mathf.Abs(x)); y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2 pos = new Vector2(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        return pointsToRetrun;
    }

    public static List<Vector2> GetHalfCirclePoints(int range)
    {
        List<Vector2> pointsToRetrun = new List<Vector2>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = 0; y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2 pos = new Vector2(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        return pointsToRetrun;
    }

    public static List<Vector2> GetLinePoints(int range)
    {
        List<Vector2> pointsToRetrun = new List<Vector2>();
        for(int i = 1; i <= range; i++)
        {
            Vector2 pos = new Vector2(0, i);
            pointsToRetrun.Add(pos);
        }
        return pointsToRetrun;
    }
}
