using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    #region 
    //888888888测试用88888888888888888888888888888888888888888888
    //public TurretFactory test;
    //[SerializeField] Text[] peifangTxt = default;
    //[SerializeField] Text[] blueprintsTxt = default;
    //List<Blueprint> blueprints;
    //Blueprint p1;
    //Blueprint p2;
    //Blueprint p3;

    //public void testXiezi(List<Blueprint> l, Text[] txt)
    //{
    //    for (int i = 0; i < txt.Length; i++)
    //    {
    //        txt[i].text = "";
    //    }
    //    for (int j = 0; j < l.Count; j++)
    //    {
    //        Blueprint t = l[j];
    //        string s = "";
    //        string buildable = "";
    //        for (int i = 0; i < t.Compositions.Count; i++)
    //        {
    //            string b;
    //            if (t.Compositions[i].obtained)
    //            {
    //                b = "已拥有";

    //            }
    //            else
    //            {
    //                b = "没有";

    //            }
    //            s = s + "配方" + (i + 1).ToString() + "  元素:" + t.Compositions[i].elementRequirement.ToString() +
    //                "  等级:" + t.Compositions[i].levelRequirement.ToString() + "  状态:" + b + "\n";
    //        }
    //        if (t.CheckBuildable())
    //        {
    //            buildable = "可建造";

    //        }
    //        else
    //        {
    //            buildable = "不可建造";

    //        }
    //        txt[j].text = s + " \n" + buildable;
    //    }
    //}
    //public void Test()
    //{
    //    //blueprints = GameManager.Instance.playerManager.GetBluePrints(3);
    //    testXiezi(blueprints, peifangTxt);
    //    p1 = blueprints[0];
    //    p2 = blueprints[1];
    //    p3 = blueprints[2];
    //}

    //public void test21()
    //{
    //    GameManager.Instance.playerManager.BuyBlueprint(p1);
    //    blueprints.Remove(p1);
    //    testXiezi(blueprints, peifangTxt);
    //    testXiezi(GameManager.Instance.playerManager.BlueprintsInPocket, blueprintsTxt);
    //}
    //public void test22()
    //{
    //    GameManager.Instance.playerManager.BuyBlueprint(p2);
    //    blueprints.Remove(p2);
    //    testXiezi(blueprints, peifangTxt);
    //    testXiezi(GameManager.Instance.playerManager.BlueprintsInPocket, blueprintsTxt);
    //}
    //public void test23()
    //{
    //    GameManager.Instance.playerManager.BuyBlueprint(p3);
    //    blueprints.Remove(p3);
    //    testXiezi(blueprints, peifangTxt);
    //    testXiezi(GameManager.Instance.playerManager.BlueprintsInPocket, blueprintsTxt);
    //}
    //public void testComposition()
    //{
    //    GameManager.Instance.playerManager.BlueprintInBuilding = p1;
    //    GetComposedShape();
    //}
    #endregion


    [SerializeField] TurretTips turretTips = default;
    [SerializeField] GameObject messagePanel = default;

    [SerializeField] TileTips turretTipPrefab, trapTipPrefab = default;

    [SerializeField] RoadPlacement _roadPlacament = default;

    [SerializeField] Text messageTxt = default;
    [SerializeField] Text healthTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] Text waveTxt = default;
    [SerializeField] Text playerLevelTxt = default;
    [SerializeField] Text playerLevelUpMoneyTxt = default;
    [SerializeField] Text lotteryDrawTxt = default;
    [SerializeField] Text luckyPointsTxt = default;

    #region 属性
    int currentWave;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = Mathf.Clamp(value, 0, StaticData.Instance.LevelMaxWave);
            waveTxt.text = "WAVE " + currentWave.ToString() + "/" + StaticData.Instance.LevelMaxWave.ToString();
        }
    }

    int enemyRemain = 0;
    public int EnemyRemain
    {
        get => enemyRemain;
        set
        {
            enemyRemain = value;
            if (enemyRemain <= 0)
            {
                enemyRemain = 0;
                GameManager.Instance.TransitionToState(StateName.BuildingState);
            }
        }
    }
    int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            playerLevelTxt.text = "当前等级：" + PlayerLevel.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[PlayerLevel];
            if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
            {
                playerLevelUpMoneyTxt.text = "升级: " + PlayerLvUpMoney.ToString();
            }
            else
            {
                playerLevelUpMoneyTxt.text = "已满级";
            }
        }
    }
    int playerLvUpMoney = 50;
    public int PlayerLvUpMoney
    {
        get => playerLvUpMoney;
        set
        {
            playerLvUpMoney = value;
        }
    }

    int lotteryDraw = 100;
    public int LotteryDraw
    {
        get => lotteryDraw;
        set
        {
            lotteryDraw = value;
            lotteryDrawTxt.text = "抽取模块 X " + LotteryDraw.ToString();
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
            luckyPointsTxt.text = "当前幸运点数:" + LuckyPoints.ToString();
        }
    }
    int playerCoin = 1000;
    public int PlayerCoin
    {
        get => playerCoin;
        set
        {
            playerCoin = value;
            coinTxt.text = PlayerCoin.ToString();
        }
    }

    float playerHealth;
    public float PlayerHealth
    {
        get => playerHealth;
        set
        {
            playerHealth = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth);
            healthTxt.text = PlayerHealth.ToString() + "/" + StaticData.Instance.PlayerMaxHealth.ToString();
        }
    }
    #endregion

    void Start()
    {
        GameEvents.Instance.onMessage += ShowMessage;
        GameEvents.Instance.onEnemyReach += EnemyReachDamge;
        GameEvents.Instance.onStartNewWave += NewWaveStart;
        GameEvents.Instance.onAddTiles += ConfirmShape;
        GameEvents.Instance.onEnemyDie += EnemyDie;
        GameEvents.Instance.onShowTileTips += ShowTileTips;
        GameEvents.Instance.onShowTurretTips += ShowTurretAttributeTips;
        GameEvents.Instance.onHideTips += HideTips;

        PlayerLevel = 1;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onMessage -= ShowMessage;
        GameEvents.Instance.onEnemyReach -= EnemyReachDamge;
        GameEvents.Instance.onStartNewWave -= NewWaveStart;
        GameEvents.Instance.onAddTiles -= ConfirmShape;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
        GameEvents.Instance.onShowTileTips -= ShowTileTips;
        GameEvents.Instance.onShowTurretTips -= ShowTurretAttributeTips;
        GameEvents.Instance.onHideTips -= HideTips;
    }

    //合成界面显示配方塔TIPS
    private void ShowTurretAttributeTips(BluePrintGrid bGrid)
    {
        turretTips.gameObject.SetActive(true);
        Vector2 size = turretTips.GetComponent<RectTransform>().sizeDelta;
        Vector2 pos = new Vector2(size.x / 2 + 500, Screen.height / 2);
        turretTips.transform.position = pos;
        turretTips.ReadAttribute(bGrid);
    }
    private void NewWaveStart(EnemySequence sequence)
    {
        CurrentWave = sequence.Wave;
        EnemyRemain = sequence.Amount;
    }

    private void EnemyDie(Enemy enemy)
    {
        EnemyRemain--;
    }
    private void EnemyReachDamge(Enemy enemy)
    {
        ChangePlayerHealth(-enemy.ReachDamage);
        EnemyRemain--;
    }

    public void ChangePlayerHealth(int value)
    {
        PlayerHealth += value;
    }

    public void GetNewBuildings()
    {
        DisplayShape(0, GameManager.Instance.GenerateRandomBasicShape());
        DisplayShape(1, GameManager.Instance.GenerateRandomBasicShape());
        DisplayShape(2, GameManager.Instance.GenerateRandomBasicShape());
        ShowArea(1);
    }


    //每回合开始前计算幸运点、抽取模块次数等逻辑。
    public void Preparing()
    {
        LotteryDraw++;
        if (luckPointsProcess < 0) 
            luckPointsProcess = 0;
        else
        {
            if (luckPointsProcess < 5) 
                luckPointsProcess++;
        }
        LuckyPoints += luckPointsProcess;
        if (LuckyPoints >= 10)
        {
            LuckyPoints -= 10;
            LotteryDraw++;
        }
        ShowArea(0);
    }

    public void DisplayShape(int displayID, TileShape shape)
    {
        _roadPlacament.DisplayShapeOnTileSelct(displayID, shape);
    }

    private void ShowMessage(string content)
    {
        StartCoroutine(MessageCor(content));
    }

    public void ShowTileTips(GameTile tile)
    {
        Vector2 size;
        Vector2 pos;
        switch (tile.BasicTileType)
        {
            case BasicTileType.SpawnPoint:
            case BasicTileType.Destination:
            case BasicTileType.Trap:
                TrapTips trapTip = Instantiate(trapTipPrefab, this.transform) as TrapTips;
                size = trapTip.GetComponent<RectTransform>().sizeDelta;
                pos = new Vector2(size.x / 2 + 50, Screen.height / 2);
                trapTip.transform.position = pos;
                trapTip.ReadAttribute(((TrapTile)tile));
                //tips.Add(trapTip);
                break;
            case BasicTileType.Turret:
                turretTips.gameObject.SetActive(true);
                size = turretTips.GetComponent<RectTransform>().sizeDelta;
                pos = new Vector2(size.x / 2, Screen.height / 2);
                turretTips.transform.position = pos;
                turretTips.ReadTurret(((TurretTile)tile).turret);
                break;
            default:
                break;
        }
    }

    public void HideTips()
    {
        turretTips.gameObject.SetActive(false);
    }

    IEnumerator MessageCor(string content)
    {
        messagePanel.SetActive(true);
        messageTxt.text = content;
        yield return new WaitForSeconds(3f);
        messagePanel.SetActive(false);
        messageTxt.text = "";
    }

    public void ShowArea(int id)
    {
        _roadPlacament.ShowArea(id);
    }
    public void HideArea(int id)
    {
        _roadPlacament.HideArea();
    }
    private void ConfirmShape(List<GameTile> tiles)
    {
        ShowArea(0);
    }

    public void ExtraDrawClick()
    {
        if (LotteryDraw > 0)
        {
            LotteryDraw--;
            luckPointsProcess = -1;
            GetNewBuildings();
        }
    }

    public void NextWaveClick()
    {
        HideArea(1);
        GameManager.Instance.TransitionToState(StateName.WaveState);
    }

    public bool ConsumeMoney(int amount)
    {
        if(PlayerCoin>= amount)
        {
            PlayerCoin -= amount;
            return true;
        }
        ShowMessage("金币不足");
        return false;
    }
    //player升级
    public void LevelUp()
    {
        if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (ConsumeMoney(PlayerLvUpMoney))
            {
                PlayerLevel++;
            }
        }
    }
}
