﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Pathfinding;

public class StaticData : Singleton<StaticData>
{

    [Header("LevelSetting")]
    //[SerializeField] private int difficulty = 2;
    //public int Difficulty { get => difficulty; set => difficulty = value; }

    public static LayerMask PathLayer = 1 << 6 | 1 << 10;
    public static string ConcreteTileMask = "ConcreteTile";
    public static string GroundTileMask = "GroundTile";
    public static string TempTileMask = "TempTile";
    public static string TempTurretMask = "TempTurret";
    public static string TempGroundMask = "TempGround";
    public static LayerMask GetGroundLayer = 1 << 8 | 1 << 12;
    //public static LayerMask RunTimeFindPathLayer = 1 << 8;
    public int PlayerMaxLevel;
    public int[] LevelUpMoney;
    public int StartCoin;
    public int StartLotteryDraw;
    public int BaseWaveIncome;
    public int WaveMultiplyIncome;
    public static float[,] QualityChances = new float[7, 5]
    {
        { 1f, 0f, 0f, 0f, 0f },
        { 0.6f, 0.3f, 0.1f, 0f, 0f },
        { 0.4f, 0.35f, 0.15f, 0.1f, 0f },
        { 0.3f, 0.35f, 0.2f, 0.125f, 0.025f },
        { 0.2f, 0.3f, 0.3f, 0.2f, 0.1f },
        { 0.1f, 0.2f, 0.25f, 0.3f, 0.15f },
        { 0.1f, 0.15f, 0.20f, 0.3f, 0.25f }
    };
    public static int BuyBluePrintCost = 20;
    [Header("GameSetting")]
    public static Vector2Int BoardOffset;
    public float EnvrionmentBaseVolume = .25f;
    public float TileSize = default;
    //塔的最大等级
    public static int maxLevel = 5;
    //一共有几种元素
    public static int elementN = 5;
    //最大quality
    public static int maxQuality = 5;
    public static int trapN = 15;
    public static int basicN = 25;

    [Header("ProbabilitySetting")]
    public float[] TileShapeChance = default;
    public int PlayerMaxHealth;
    [SerializeField] int[] difficutyWave;
    public int LevelMaxWave
    {
        get
        {
            return difficutyWave[Game.Instance.Difficulty - 1];
        }
    }
    //元素加成
    public static float GoldAttackIntensify = 0.1f;
    public static float WoodSpeedIntensify = 0.1f;
    public static float WaterSlowIntensify = 0.2f;
    public static float FireCriticalIntensify = 0.1f;
    public static float DustSputteringIntensify = 0.1f;

    //tips信息
    public static Dictionary<string, string> TipsInfoDIC;


    private void Start()
    {
        InitializeInfoDIC();
        //Difficulty = Game.Instance.Difficulty;
    }

    private void InitializeInfoDIC()
    {
        TipsInfoDIC = new Dictionary<string, string>();
        TipsInfoDIC.Add("LuckyInfo", GetLuckyInfo());
    }

    [Header("CompositionAttributes")]
    public int[,] LevelUpCost = new int[3, 2]//合成塔升级费用
    {
        { 75, 150 },
        { 150,300 },
        { 250,500 }
    };
    public float[,] RareChances = new float[7, 3]//配方刷新概率
    {
        { 0.9f,0.1f,0f },
        { 0.75f,0.25f,0f},
        { 0.6f,0.38f,0.02f },
        { 0.5f,0.42f,0.8f},
        { 0.4f,0.4f,0.2f},
        { 0.35f,0.35f,0.3f},
        { 0.3f,0.3f,0.4f},
    };


    public static int RandomNumber(float[] pros)
    {
        float total = 0f;
        foreach (float elem in pros)
        {
            total += elem;
        }
        float randomPoint = UnityEngine.Random.value * total;
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
            int num = UnityEngine.Random.Range(0, end + 1);
            output[i] = sequence[num];
            sequence[num] = sequence[end];
            end--;
        }
        return output;
    }

    public static List<Vector2Int> GetCirclePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = -(range - Mathf.Abs(x)); y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2Int pos = new Vector2Int(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = -(forbidRange - Mathf.Abs(x)); y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2Int pos = new Vector2Int(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2Int> GetHalfCirclePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int x = -range; x <= range; x++)
        {
            for (int y = 0; y <= range - Mathf.Abs(x); y++)
            {
                if (x == 0 && y == 0)
                    continue;
                Vector2Int pos = new Vector2Int(x, y);
                pointsToRetrun.Add(pos);
            }
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = 0; y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2Int pos = new Vector2Int(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2Int> GetLinePoints(int range, int forbidRange = 0)
    {
        List<Vector2Int> pointsToRetrun = new List<Vector2Int>();
        for (int i = 1; i <= range; i++)
        {
            Vector2Int pos = new Vector2Int(0, i);
            pointsToRetrun.Add(pos);
        }
        if (forbidRange > 0)
        {
            List<Vector2Int> pointsToExcept = new List<Vector2Int>();
            for (int i = 1; i <= forbidRange; i++)
            {
                Vector2Int pos = new Vector2Int(0, i);
                pointsToExcept.Add(pos);
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }
    //给定一个总等级，返回若干个随机数的方法
    public static int[] GetSomeRandoms(int totalLevel, int number)
    {
        if ((totalLevel / number) > maxLevel)
        {
            Debug.LogWarning("配方等级输错了，菜鸡");
            int[] errorRandom = new int[number];
            for (int i = 0; i < number; i++)
            {
                errorRandom[i] = maxLevel;
            }
            return errorRandom;
        }
        if (number < 1)
        {
            number = 1;
        }
        if (totalLevel < 1)
        {
            totalLevel = 1;
        }
        if (number > totalLevel)
        {
            totalLevel = number;
        }
        int[] result = new int[number];
        while (number > 1)
        {
            if (number == 2)
            {
                int min = 1;
                while (totalLevel - min > maxLevel)
                {
                    min++;
                }
                result[0] = UnityEngine.Random.Range(min, totalLevel - min + 1);
                result[1] = totalLevel - result[0];
                number--;
            }
            else if (number >= 2)
            {
                int max = Mathf.Min(totalLevel - (number - 1), maxLevel);
                int min = 1;
                while (totalLevel - min > maxLevel * (number - 1))
                {
                    min++;
                }
                int a = UnityEngine.Random.Range(min, max + 1);
                totalLevel -= a;
                result[number - 1] = a;
                number--;
            }
            else
            {
                Debug.LogWarning("刷随机等级的算法不支持！");
                return null;
            }
        }
        return result;
    }


    //total是总量，number是想要几个随机数
    public static List<int> SelectNoRepeat(int total, int number)
    {
        List<int> data = new List<int>();
        for (int i = 0; i < total; i++)
        {
            data.Add(i);
        }
        if (data.Count < number)
        {
            return data;
        }
        else
        {
            List<int> result = new List<int>();
            for (int i = 0; i < number; i++)
            {
                int index = UnityEngine.Random.Range(0, data.Count);
                result.Add(data[index]);
                data.RemoveAt(index);
            }
            return result;
        }
    }

    public static Collider2D RaycastCollider(Vector2 pos, LayerMask layer)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(pos, Vector3.forward, Mathf.Infinity, layer);
        if (hit.collider != null)
        {
            return hit.collider;
        }
        return null;
    }

    public static string GetElementIntensifyText(Element element, int quality)
    {
        string intensifyTxt = "+";//根据元素及品质设置显示加成效果
        switch (element)
        {
            case Element.Gold:
                intensifyTxt += GoldAttackIntensify * 100 * quality + "%攻击";
                break;
            case Element.Wood:
                intensifyTxt += WoodSpeedIntensify * 100 * quality + "%攻速";
                break;
            case Element.Water:
                intensifyTxt += WaterSlowIntensify * quality + "减速";
                break;
            case Element.Fire:
                intensifyTxt += FireCriticalIntensify * 100 * quality + "%暴击率";
                break;
            case Element.Dust:
                intensifyTxt += FireCriticalIntensify * quality + "溅射";
                break;
            default:
                Debug.Log("错误的元素，无法配置加成");
                break;
        }
        return intensifyTxt;
    }

    public static string GetTurretDes(TurretAttribute attribute, int quality)
    {
        string finalDes = "";
        if (attribute.Description != "")
            finalDes += attribute.Description + "\n";
        if (attribute.TurretLevels[quality - 1].TurretEffects.Count > 0)
        {
            foreach (TurretEffectInfo effect in attribute.TurretLevels[quality - 1].TurretEffects)
            {
                finalDes += effect.EffectDescription;
                finalDes += "\n";
            }
        }
        return finalDes;
    }

    public static string GetLevelInfo(int level)
    {
        float[] levelChances = new float[5];
        for (int i = 0; i < 5; i++)
        {
            levelChances[i] = QualityChances[level - 1, i];
        }
        string text = "";
        text += "\n  当前等级概率:\n";
        for (int x = 0; x < 5; x++)
        {
            text += "  品质" + (x + 1).ToString() + ": " + levelChances[x] * 100 + "%\n";
        }
        return text;
    }

    public static string GetLuckyInfo()
    {
        string text =
            "\n1.当前回合没有抽取时，获得1点累积点。\n" +
            "2.连续不抽取时，会获得额外累积点。\n" +
            "3.累积点每达到10点，获得1次额外抽取次数。\n";
        return text;
    }

    public static void CorrectTileCoord(TileBase tile)
    {
        Vector2Int coord = new Vector2Int(Convert.ToInt32(tile.transform.position.x), Convert.ToInt32(tile.transform.position.y));
        int newX = coord.x + BoardOffset.x;
        int newY = coord.y + BoardOffset.y;
        tile.OffsetCoord = new Vector2Int(newX, newY);
    }

    public static void SetNodeWalkable(TileBase tile, bool walkable, bool changeAble = true)
    {
        var grid = AstarPath.active.data.gridGraph;
        int p = tile.OffsetCoord.x;
        int q = tile.OffsetCoord.y;

        GridNodeBase node = grid.nodes[q * grid.width + p];

        node.Walkable = walkable;
        node.ChangeAbleNode = changeAble;
        grid.CalculateConnectionsForCellAndNeighbours(p, q);
    }

    public static bool GetNodeWalkable(TileBase tile)
    {
        var grid = AstarPath.active.data.gridGraph;
        int p = tile.OffsetCoord.x;
        int q = tile.OffsetCoord.y;

        GridNodeBase node = grid.nodes[q * grid.width + p];

        return node.Walkable;
    }
}
