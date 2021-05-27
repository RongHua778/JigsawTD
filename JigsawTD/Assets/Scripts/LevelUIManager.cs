using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
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
    int playerLvUpMoney = 0;
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
    int luckyPoints = 0;
    public int LuckyPoints
    {
        get => luckyPoints;
        set
        {
            luckyPoints = value;
            luckyPointsTxt.text = "幸运点:" + LuckyPoints.ToString();
        }
    }
    bool drawThisTurn = false;
    public bool DrawThisTurn { get => drawThisTurn;
        set
        {
            drawThisTurn = value;
            if (drawThisTurn)
            {
                luckPointsProcess = 0;//如果抽了卡，清空幸运点进度
            }
        } 
    }

    int playerCoin = 0;
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

        CurrentWave = 0;
        PlayerLevel = 1;
        PlayerHealth = StaticData.Instance.PlayerMaxHealth;
        PlayerCoin = StaticData.Instance.StartCoin;
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
        CurrentWave++;
        //回合收入
        PlayerCoin += StaticData.Instance.BaseWaveIncome + StaticData.Instance.WaveMultiplyIncome * CurrentWave;
        //抽取次数及幸运点
        if (!DrawThisTurn)
        {
            LuckyPoints += luckPointsProcess;
            luckPointsProcess++;
            if (LuckyPoints >= 10)
            {
                LuckyPoints -= 10;
                LotteryDraw++;
            }
        }

        DrawThisTurn = false;
        LotteryDraw++;
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
            DrawThisTurn = true;
            GetNewBuildings();
        }
        else
        {
            ShowMessage("抽取次数不足");
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
