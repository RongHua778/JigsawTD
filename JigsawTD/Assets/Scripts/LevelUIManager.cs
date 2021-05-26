using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    //888888888测试用88888888888888888888888888888888888888888888
    public TurretFactory test;
    [SerializeField] TMP_Text[] peifangTxt = default;
    [SerializeField] TMP_Text[] blueprintsTxt = default;
    List<Blueprint> blueprints;
    Blueprint p1;
    Blueprint p2;
    Blueprint p3;

    public void testXiezi(List<Blueprint> l, TMP_Text[] txt)
    {
        for(int i=0;i<txt.Length;i++)
        {
            txt[i].text = "";
        }
        for (int j = 0; j < l.Count; j++)
        {
            Blueprint t = l[j];
            string s = "";
            string buildable = "";
            for (int i = 0; i < t.Compositions.Count; i++)
            {
                string b;
                if (t.Compositions[i].obtained)
                {
                    b = "已拥有";

                }
                else
                {
                    b = "没有";

                }
                s = s + "配方" + (i + 1).ToString() + "  元素:" + t.Compositions[i].elementRequirement.ToString() +
                    "  等级:" + t.Compositions[i].levelRequirement.ToString() + "  状态:" + b + "\n";
            }
            if (t.CheckBuildable())
            {
                buildable = "可建造";

            }
            else
            {
                buildable = "不可建造";

            }
            txt[j].text = s + " \n" + buildable;
        }
    }
    public void Test()
    {
        blueprints = playerManager.GetBluePrints(3);
        testXiezi(blueprints, peifangTxt);
        p1=blueprints[0];
        p2=blueprints[1];
        p3=blueprints[2];
    }

    public void test21()
    {
        playerManager.BuyBlueprint(p1);
        blueprints.Remove(p1);
        testXiezi(blueprints, peifangTxt);
        testXiezi(playerManager.BlueprintsInPocket, blueprintsTxt);
    }
    public void test22()
    {
        playerManager.BuyBlueprint(p2);
        blueprints.Remove(p2);
        testXiezi(blueprints, peifangTxt);
        testXiezi(playerManager.BlueprintsInPocket, blueprintsTxt);
    }
    public void test23()
    {
        playerManager.BuyBlueprint(p3);
        blueprints.Remove(p3);
        testXiezi(blueprints, peifangTxt);
        testXiezi(playerManager.BlueprintsInPocket, blueprintsTxt);
    }
    public void testComposition()
    {
        playerManager.BlueprintInBuilding=p1;
        GetComposedShape();
    }
    //888888888888888888888888888888888888888888888888888888888

    [SerializeField]
    GameObject messagePanel;
    
    [SerializeField] TileTips turretTipPrefab, trapTipPrefab = default;

    [SerializeField] RoadPlacement _roadPlacament = default;

    [SerializeField] Text messageTxt=default;
    [SerializeField] Text healthTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] Text waveTxt = default;
    [SerializeField] Text playerLevelTxt = default;
    [SerializeField] Text playerLevelUpMoneyTxt = default;
    [SerializeField] Text lotteryDrawTxt = default;
    [SerializeField] Text luckyPointsTxt = default;

    List<TileTips> tips = new List<TileTips>();
    public PlayerManager playerManager;

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
        GameEvents.Instance.onShowTileTips += ShowTileTips;
    }

    private void OnDisable()
    {
        GameEvents.Instance.onMessage -= ShowMessage;
        GameEvents.Instance.onEnemyReach -= EnemyReachDamge;
        GameEvents.Instance.onStartNewWave -= NewWaveStart;
        GameEvents.Instance.onAddTiles -= ConfirmShape;
        GameEvents.Instance.onEnemyDie -= EnemyDie;
        GameEvents.Instance.onShowTileTips -= ShowTileTips;
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
        playerManager.PlayerHealth += value;
    }

    public void GetNewBuildings()
    {
        DisplayShape(0, GameManager.Instance.GetRandomNewShape());
        DisplayShape(1, GameManager.Instance.GetRandomNewShape());
        DisplayShape(2, GameManager.Instance.GetRandomNewShape());
        ShowArea(1);
    }

    public void GetComposedShape()
    {
        playerManager.PlayerWish = PlayerWish.Composition;
        TileShape shape = GameManager.Instance.ShapeFactory.GetShape(ShapeType.D);
        shape.InitializeShape();
    }
    //每回合开始前计算幸运点、抽取模块次数等逻辑。
    public void Preparing() 
    {
        playerManager.LotteryDraw++;
        if (playerManager.luckPointsProcess < 0) playerManager.luckPointsProcess = 0;
        else 
        { 
            if(playerManager.luckPointsProcess <5) playerManager.luckPointsProcess++; 
        }
        playerManager.LuckyPoints += playerManager.luckPointsProcess;
        if (playerManager.LuckyPoints >= 10)
        {
            playerManager.LuckyPoints -= 10;
            playerManager.LotteryDraw++;
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
                turretTip.ReadAttribute(((TurretTile)tile).turret);
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
        if (playerManager.LotteryDraw > 0)
        {
            playerManager.LotteryDraw--;
            playerManager.luckPointsProcess = -1;
            GetNewBuildings();
        }
    }

    public void NextWaveClick()
    {
        HideArea(1);
        GameManager.Instance.TransitionToState(StateName.WaveState);
    }

    //把player的数据同步到UI上
    public void SynchronizeLabels()
    {
        playerLevelTxt.text = "LV" + playerManager.PlayerLevel.ToString();
        if (playerManager.PlayerLevel < StaticData.Instance.PlayerMaxLevel)
        {
            playerLevelUpMoneyTxt.text = "升级(金币): " + playerManager.PlayerLvUpMoney.ToString();
        }
        else
        {
            playerLevelUpMoneyTxt.text = "已达到最大等级";
        }
        lotteryDrawTxt.text = "抽取模块 X " + playerManager.LotteryDraw.ToString();
        luckyPointsTxt.text = "当前幸运点数:" + playerManager.LuckyPoints.ToString();
        coinTxt.text = playerManager.PlayerCoin.ToString();
        healthTxt.text = playerManager.PlayerHealth.ToString() + "/" + StaticData.Instance.PlayerMaxHealth.ToString();
    }
    //player升级
    public void LevelUp()
    {
        if (playerManager.PlayerLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (playerManager.PlayerCoin >= playerManager.PlayerLvUpMoney)
            {
                playerManager.PlayerLevel++;
                playerManager.PlayerCoin -= playerManager.PlayerLvUpMoney;
                playerManager.PlayerLvUpMoney = playerManager.levelMoney[playerManager.PlayerLevel];
                SynchronizeLabels();
            }
        }
    }
}
