using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Pathfinding;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class StaticData : Singleton<StaticData>
{
    [Header("LevelSetting")]
    public static LayerMask PathLayer = 1 << 6 | 1 << 10;
    public static string TrapMask = "Trap";
    public static string ConcreteTileMask = "ConcreteTile";
    public static string GroundTileMask = "GroundTile";
    public static string TempTileMask = "TempTile";
    public static string TempTurretMask = "TempTurret";
    public static string TurretMask = "Turret";
    public static string TempGroundMask = "TempGround";
    public static LayerMask EnemyLayerMask = 1 << 11;
    public static LayerMask GetGroundLayer = 1 << 8 | 1 << 12;
    //public static LayerMask RunTimeFindPathLayer = 1 << 8;
    public int PlayerMaxLevel;
    public int[] LevelUpMoney;
    public int StartCoin;
    public int StartLotteryDraw;
    public int BaseWaveIncome;
    public int WaveMultiplyIncome;
    public int BuyBluePrintCost;
    public int ShopRefreshCost;
    public float CoinInterest;
    public int BaseShapeCost;
    public int MultipleShapeCost;
    public int BuyGroundCost;
    public int BuyGroundCostMultyply;
    public int SwitchTrapCost;
    public int SwitchTrapCostMultiply;
    //抽取品质概率
    public static float[,] QualityChances = new float[6, 5]
    {
        //{ 1f, 0f, 0f, 0f, 0f },
        //{ 0.6f, 0.3f, 0.1f, 0f, 0f },
        //{ 0.4f, 0.35f, 0.15f, 0.1f, 0f },
        //{ 0.3f, 0.35f, 0.2f, 0.125f, 0.025f },
        //{ 0.15f, 0.3f, 0.3f, 0.25f, 0.1f },
        //{ 0f, 0.2f, 0.3f, 0.3f, 0.2f },
        //{ 0f, 0.15f, 0.25f, 0.3f, 0.3f }
        { 1f, 0f, 0f, 0f, 0f },
        { 0.6f, 0.3f, 0.1f, 0f, 0f },
        { 0.4f, 0.35f, 0.15f, 0.1f, 0f },
        { 0.3f, 0.35f, 0.2f, 0.125f, 0.025f },
        { 0.15f, 0.3f, 0.3f, 0.25f, 0.1f },
        { 0f, 0.2f, 0.3f, 0.3f, 0.2f },
    };
    [Header("GameSetting")]
    public static Vector2Int BoardOffset;
    public float EnvrionmentBaseVolume = .25f;
    public float TileSize = default;
    //塔的最大等级
    public static int maxLevel = 5;
    //一共有几种战斗元素
    public static int elementN = 4;
    //最大quality
    public static int maxQuality = 5;
    public int trapN = 15;
    public static int basicN = 25;

    [Header("动态数据")]
    public static int PerfectElementCount = 0;//完美元素数量
    public static float OverallMoneyIntensify = 0;//金币加成
    public static int FreeGroundTileCount = 0;//免费地板数量
    public static Action<StrategyBase> NextCompositeCallback = null;

    [Header("ProbabilitySetting")]
    public float[] TileShapeChance = default;
    public int[] PlayerMaxHealth;
    [SerializeField] int[] difficutyWave;
    public int LevelMaxWave
    {
        get
        {
            return difficutyWave[Game.Instance.SelectDifficulty];
        }
    }
    //元素加成
    public static float GoldAttackIntensify = 0.3f;
    public static float WoodSpeedIntensify = 0.3f;
    public static float WaterSlowIntensify = 0.5f;
    public static float FireCriticalIntensify = 0.2f;
    public static float DustSputteringIntensify = 0.3f;

    public static Color32 RedColor;
    public static Color32 GreenColor;
    public static Color32 BlueColor;
    public static Color32 YellowColor;
    public static Color32 PurpleColor;

    //攻击范围PREFAB
    public RangeHolder CircleCol;
    //public Collider2D HalfCircleCol;
    //public Collider2D LineCol;
    public RangeIndicator RangeIndicatorPrefab;
    public JumpDamage JumpDamagePrefab;
    public Sprite UnrevealTrap;
    public ParticalControl LandedEffect;
    //public GainCoinAnim GainCoinAnimPrefab;



    [Header("CompositionAttributes")]
    public int LevelUpCostPerRare;
    public int[,] LevelUpCost = new int[3, 2]//合成塔升级费用
    {
        { 75, 150 },
        { 150,300 },
        { 250,500 }
    };
    public float[,] RareChances = new float[7, 3]//配方刷新概率
    {
        { 1f,0f,0f },
        { 0.9f,0.1f,0f},
        { 0.75f,0.25f,0f },
        { 0.6f,0.35f,0.1f},
        { 0.5f,0.3f,0.2f},
        { 0.3f,0.4f,0.3f},
        { 0.2f,0.4f,0.4f},
    };

    //随机打乱一个int list的方法
    public static List<T> RandomSort<T>(List<T> list)
    {
        var random = new System.Random();
        var newList = new List<T>();
        foreach (var item in list)
        {
            newList.Insert(random.Next(newList.Count), item);
        }
        return newList;
    }

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
                intensifyTxt += GoldAttackIntensify * 100 + GameMultiLang.GetTraduction("ATTACKUP");
                break;
            case Element.Wood:
                intensifyTxt += WoodSpeedIntensify * 100 + GameMultiLang.GetTraduction("SPEEDUP");
                break;
            case Element.Water:
                intensifyTxt += WaterSlowIntensify + GameMultiLang.GetTraduction("SLOWUP");
                break;
            case Element.Fire:
                intensifyTxt += FireCriticalIntensify * 100 + GameMultiLang.GetTraduction("CRITICALUP");
                break;
            case Element.Dust:
                intensifyTxt += DustSputteringIntensify + GameMultiLang.GetTraduction("SPUTTERINGUP");
                break;
            default:
                Debug.Log("错误的元素，无法配置加成");
                break;
        }
        return intensifyTxt;
    }

    public static string SetElementSkillInfo(ElementSkill skill)
    {
        string intensifyTxt = GameMultiLang.GetTraduction("ELEMENTSKILL") + ":";//根据元素及品质设置显示加成效果
        foreach (var element in skill.Elements)
        {
            switch ((Element)element)
            {
                case Element.Gold:
                    intensifyTxt += "\n" + GoldAttackIntensify * 100 + GameMultiLang.GetTraduction("ATTACKUP");
                    break;
                case Element.Wood:
                    intensifyTxt += "\n" + WoodSpeedIntensify * 100 + GameMultiLang.GetTraduction("SPEEDUP");
                    break;
                case Element.Water:
                    intensifyTxt += "\n" + WaterSlowIntensify + GameMultiLang.GetTraduction("SLOWUP");
                    break;
                case Element.Fire:
                    intensifyTxt += "\n" + FireCriticalIntensify * 100 + GameMultiLang.GetTraduction("CRITICALUP");
                    break;
                case Element.Dust:
                    intensifyTxt += "\n" + DustSputteringIntensify + GameMultiLang.GetTraduction("SPUTTERINGUP");
                    break;
            }
        }
        //intensifyTxt += bluePrint.CompositeAttackDamage > 0 ? "\n+" + bluePrint.CompositeAttackDamage * 100 + GameMultiLang.GetTraduction("ATTACKUP") : "";
        //intensifyTxt += bluePrint.CompositeAttackSpeed > 0 ? "\n+" + bluePrint.CompositeAttackSpeed * 100 + GameMultiLang.GetTraduction("SPEEDUP") : "";
        //intensifyTxt += bluePrint.CompositeSlowRate > 0 ? "\n+" + bluePrint.CompositeSlowRate + GameMultiLang.GetTraduction("SLOWUP") : "";
        //intensifyTxt += bluePrint.CompositeCriticalRate > 0 ? "\n+" + bluePrint.CompositeCriticalRate * 100 + GameMultiLang.GetTraduction("CRITICALUP") : "";
        //intensifyTxt += bluePrint.CompositeSputteringRange > 0 ? "\n+" + bluePrint.CompositeSputteringRange + GameMultiLang.GetTraduction("SPUTTERINGUP") : "";
        return intensifyTxt;
    }

    public static string GetTurretDes(TurretAttribute attribute)
    {
        string finalDes = "";
        if (attribute.Description != "")
            finalDes += GameMultiLang.GetTraduction(attribute.Description);
        //if (strategy.TurretSkill != null)
        //{
        //    finalDes += GameMultiLang.GetTraduction(strategy.TurretSkill.SkillDescription);
        //}
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
        text += GameMultiLang.GetTraduction("MODULELEVELINFO1") + ":\n";
        for (int x = 0; x < 5; x++)
        {
            text += GameMultiLang.GetTraduction("MODULELEVELINFO2") + (x + 1).ToString() + ": " + levelChances[x] * 100 + "%\n";
        }
        text += GameMultiLang.GetTraduction("MODULELEVELINFO3");
        return text;
    }

    public static string GetEnergyInfo()
    {
        string text = GameMultiLang.GetTraduction("ENERGYINFO");
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

    public void ShowJumpDamage(Vector2 pos, int amount)
    {
        JumpDamage obj = ObjectPool.Instance.Spawn(JumpDamagePrefab) as JumpDamage;
        obj.Jump(amount, pos);
    }


    //存档
    public static void SaveGame(Save save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Save");

    }

    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            Game.Instance.SaveData = save;

        }
        else
        {
            Save save = new Save();
            List<int> sElements = new List<int> { 0, 1, 2, 3 };
            save.SaveSelectedElement = sElements;
            Game.Instance.SaveData = save;
            //m_EventTrigger.SetRemainEvent();//初始化eventlist
        }

    }

    public static void C(List<int> lsArray, int selectCount)
    {
        int totolcount = lsArray.Count;
        int[] currectselect = new int[selectCount];
        int last = selectCount - 1;

        for (int i = 0; i < selectCount; i++)
        {
            currectselect[i] = i;
        }

        while (true)
        {
            for (int i = 0; i < selectCount; i++)
            {
                Debug.Log(lsArray[currectselect[i]]);
            }
            Debug.Log("");
            if (currectselect[last] < totolcount - 1)
            {
                currectselect[last]++;
            }
            else
            {
                int pos = last;
                while (pos > 0 && currectselect[pos - 1] == currectselect[pos] - 1)
                {
                    pos--;
                }
                if (pos == 0) return;
                currectselect[pos - 1]++;
                for (int i = pos; i < selectCount; i++)
                {
                    currectselect[i] = currectselect[i - 1] + 1;
                }
            }
        }
    }

    public static List<List<int>> GetAllC(List<int> dataList, int n, List<int> value = null)
    {
        List<List<int>> result = new List<List<int>>();
        for (int i = 0, count = dataList.Count; i < count; i++)
        {
            List<int> itemList = new List<int>();
            if (value != null && value.Count > 0)
            {
                itemList.AddRange(value);
            }
            itemList.Add(dataList[i]);

            if (itemList.Count == n)
            {
                result.Add(itemList);
            }
            else
            {
                result.AddRange(GetAllC(dataList, n, itemList));
            }
        }
        return result;
    }

    public static List<List<int>> GetAllCC(List<int> dataList)
    {
        List<List<int>> result = new List<List<int>>();
        for (int a = 0; a < dataList.Count; a++)
        {
            for (int b = a; b < dataList.Count; b++)
            {
                for (int c = b; c < dataList.Count; c++)
                {
                    List<int> itemList = new List<int>();
                    itemList.Add(dataList[a]);
                    itemList.Add(dataList[b]);
                    itemList.Add(dataList[c]);
                    result.Add(itemList);
                }
            }
        }
        return result;
    }

    public static List<List<int>> GetAllCC2(List<int> dataList)
    {
        List<List<int>> result = new List<List<int>>();
        for (int a = 0; a < dataList.Count; a++)//同名先加
        {
            List<int> itemList = new List<int>();
            itemList.Add(dataList[a]);
            itemList.Add(dataList[a]);
            itemList.Add(dataList[a]);
            result.Add(itemList);
        }
        for (int b = 0; b < dataList.Count; b++)//双同名
        {
            for (int c = 0; c < dataList.Count; c++)
            {
                if (c == b)
                    continue;
                List<int> itemList = new List<int>();
                itemList.Add(dataList[b]);
                itemList.Add(dataList[b]);
                itemList.Add(dataList[c]);
                result.Add(itemList);
            }
        }
        for (int a = 0; a < dataList.Count; a++)//混合
        {
            for (int b = a + 1; b < dataList.Count; b++)
            {
                for (int c = b + 1; c < dataList.Count; c++)
                {
                    List<int> itemList = new List<int>();
                    itemList.Add(dataList[a]);
                    itemList.Add(dataList[b]);
                    itemList.Add(dataList[c]);
                    result.Add(itemList);
                }
            }
        }


        //for (int a = 0; a < dataList.Count; a++)
        //{
        //    for (int b = a; b < dataList.Count; b++)
        //    {
        //        for (int c = b; c < dataList.Count; c++)
        //        {
        //            List<int> itemList = new List<int>();
        //            itemList.Add(dataList[a]);
        //            itemList.Add(dataList[b]);
        //            itemList.Add(dataList[c]);
        //            result.Add(itemList);
        //        }
        //    }
        //}
        return result;
    }

}
