using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject messagePanel;
    [SerializeField]
    TMP_Text messageTxt;
    [SerializeField] TileTips turretTipPrefab, trapTipPrefab = default;

    [SerializeField] RoadPlacement _roadPlacament = default;

    [SerializeField] TMP_Text healthTxt = default;
    [SerializeField] TMP_Text coinTxt = default;
    [SerializeField] TMP_Text waveTxt = default;
    [SerializeField] TMP_Text playerLevelTxt = default;
    [SerializeField] TMP_Text playerLevelUpMoneyTxt = default;
    [SerializeField] TMP_Text lotteryDrawTxt = default;
    [SerializeField] TMP_Text luckyPointsTxt = default;

    List<TileTips> tips = new List<TileTips>();

    int playerLevel=1;
    public int PlayerLevel 
    {   get => playerLevel; 
        set 
        { 
            playerLevel = value;
            playerLevelTxt.text = "LV"+ playerLevel.ToString();
        } 
    }
    int playerLvUpMoney;
    public int PlayerLvUpMoney 
    { 
        get => playerLvUpMoney;
        set
        {
            playerLvUpMoney = value;
            playerLevelUpMoneyTxt.text = "升级(金币): "+playerLvUpMoney.ToString();
        }
    }

    int lotteryDraw=2;
    public int LotteryDraw
    {
        get => lotteryDraw;
        set
        {
            lotteryDraw = value;
            lotteryDrawTxt.text = "抽取模块 X " + lotteryDraw.ToString();
        }
    }
    //控制每回合加的幸运点数
    int luckPointsProcess = 0;
    int luckyPoints = 5;
    public int LuckyPoints
    {
        get => luckyPoints;
        set
        {
            luckyPoints = value;
            luckyPointsTxt.text = "当前幸运点数:" + luckyPoints.ToString();
        }
    }
    int playerCoin = 10;
    public int PlayerCoin
    {
        get => playerCoin;
        set
        {
            playerCoin = value;
            coinTxt.text = playerCoin.ToString();
        }
    }

    float playerHealth;
    public float PlayerHealth
    {
        get => playerHealth;
        set
        {
            playerHealth = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth);
            healthTxt.text = playerLevel.ToString() + "/" + StaticData.Instance.PlayerMaxHealth.ToString();
        }
    }

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



    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.onMessage += ShowMessage;
        GameEvents.Instance.onEnemyReach += EnemyReachDamge;
        GameEvents.Instance.onStartNewWave += NewWaveStart;
        GameEvents.Instance.onAddTiles += ConfirmShape;
        GameEvents.Instance.onEnemyDie += EnemyDie;
        PlayerHealth = StaticData.Instance.PlayerMaxHealth;
        PlayerLevel = playerLevel;
        PlayerLvUpMoney = playerLvUpMoney;
        LotteryDraw = lotteryDraw;
        LuckyPoints = luckyPoints;
        PlayerCoin = playerCoin;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onMessage -= ShowMessage;
        GameEvents.Instance.onEnemyReach -= EnemyReachDamge;
        GameEvents.Instance.onStartNewWave -= NewWaveStart;
        GameEvents.Instance.onAddTiles -= ConfirmShape;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
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
        DisplayShape(0, GameManager.Instance.GetRandomNewShape());
        DisplayShape(1, GameManager.Instance.GetRandomNewShape());
        DisplayShape(2, GameManager.Instance.GetRandomNewShape());
        ShowArea(1);
    }
    //每回合开始前计算幸运点、抽取模块次数等逻辑。
    public void Preparing() 
    {
        LotteryDraw++;
        if (luckPointsProcess < 0) luckPointsProcess = 0;
        else 
        { 
            if(luckPointsProcess<5)luckPointsProcess++; 
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
                tips.Add(trapTip);
                break;
            case BasicTileType.Turret:
                TurretTips turretTip = Instantiate(turretTipPrefab, this.transform) as TurretTips;
                size = turretTip.GetComponent<RectTransform>().sizeDelta;
                pos = new Vector2(size.x / 2, Screen.height / 2);
                turretTip.transform.position = pos;
                turretTip.ReadAttribute(((TurretTile)tile).tile);
                tips.Add(turretTip);
                break;
            default:
                break;
        }
    }

    public void HideTips()
    {
        if (tips.Count > 0)
        {
            foreach (var tip in tips.ToList())
            {
                tips.Remove(tip);
                tip.Hide();
            }
        }
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


}
