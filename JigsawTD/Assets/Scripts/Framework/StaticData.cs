using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class StaticData : Singleton<StaticData>
{

    [Header("LevelSetting")]
    public static DraggingShape holdingShape;
    public static LayerMask PathLayer = 1 << 6 | 1 << 10;
    public static string ConcreteTileMask = "ConcreteTile";
    public static string TrapTileMask = "TrapTile";
    public static string GroundTileMask = "GroundTile";
    public static string TempTileMask = "TempTile";
    public static string TurretMask = "Turret";
    public static string TempTurretMask = "TempTurret";
    public static string TempGroundMask = "TempGround";
    public static LayerMask GetGroundLayer = 1 << 8 | 1 << 12;
    public static LayerMask RunTimeFindPathLayer = 1 << 8;// 1 << 7 |
    public int PlayerMaxLevel;
    public int[] LevelUpMoney;
    public int StartCoin;
    public int StartLotteryDraw;
    public int BaseWaveIncome;
    public int WaveMultiplyIncome;
    public float[,] LevelChances = new float[7, 5]
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
    public float GameSlowDownRate = default;
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
    public int LevelMaxWave;

    [Header("ElementAttributes")]
    public float GoldAttackIntensify;
    public float WoodSpeedIntensify;
    public float WaterSlowIntensify;
    public float FireCriticalIntensify;
    public float DustSputteringIntensify;


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

    public static List<Vector2> GetCirclePoints(int range, int forbidRange = 0)
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
        if (forbidRange > 0)
        {
            List<Vector2> pointsToExcept = new List<Vector2>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = -(forbidRange - Mathf.Abs(x)); y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2 pos = new Vector2(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2> GetHalfCirclePoints(int range, int forbidRange = 0)
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
        if (forbidRange > 0)
        {
            List<Vector2> pointsToExcept = new List<Vector2>();
            for (int x = -forbidRange; x <= forbidRange; x++)
            {
                for (int y = 0; y <= forbidRange - Mathf.Abs(x); y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    Vector2 pos = new Vector2(x, y);
                    pointsToExcept.Add(pos);
                }
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }

    public static List<Vector2> GetLinePoints(int range, int forbidRange = 0)
    {
        List<Vector2> pointsToRetrun = new List<Vector2>();
        for (int i = 1; i <= range; i++)
        {
            Vector2 pos = new Vector2(0, i);
            pointsToRetrun.Add(pos);
        }
        if (forbidRange > 0)
        {
            List<Vector2> pointsToExcept = new List<Vector2>();
            for (int i = 1; i <= forbidRange; i++)
            {
                Vector2 pos = new Vector2(0, i);
                pointsToExcept.Add(pos);
            }
            return pointsToRetrun.Except(pointsToExcept).ToList();
        }
        return pointsToRetrun;
    }
    //给定一个总等级，返回若干个随机数的方法
    public static int[] GetSomeRandoms(int totalLevel, int number)
    {
        if ((totalLevel / number )> maxLevel)
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
                result[0] = Random.Range(min, totalLevel - min + 1);
                result[1] = totalLevel - result[0];
                number--;
            }
            else if (number >= 2)
            {
                int max = Mathf.Min(totalLevel - (number - 1), maxLevel);
                int min = 1;
                while (totalLevel - min > maxLevel*(number-1))
                {
                    min++;
                }
                int a = Random.Range(min, max + 1);
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
                int index = Random.Range(0, data.Count);
                result.Add(data[index]);
                data.RemoveAt(index);
            }
            return result;
        }
    }

    public GameTile GetTile(Vector3 origin)
    {

        RaycastHit2D hit;
        hit = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, LayerMask.GetMask(ConcreteTileMask));
        if (hit.collider != null)
        {
            GameTile hitTile = hit.collider.GetComponentInParent<GameTile>();
            if (hitTile != null)
            {
                return hitTile;
            }
            hitTile = hit.collider.GetComponentInParent<TurretTile>();
            if (hitTile != null)
            {
                return hitTile;
            }
        }
        RaycastHit2D hit2 = Physics2D.Raycast(origin, Vector3.forward, Mathf.Infinity, LayerMask.GetMask(TrapTileMask));
        if (hit2.collider != null)
        {
            GameTile hitTile = hit2.collider.GetComponentInParent<GameTile>();
            if (hitTile != null)
            {
                return hitTile;
            }
        }
        return null;
    }
}
