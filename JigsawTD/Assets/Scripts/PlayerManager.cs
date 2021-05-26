using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public LevelUIManager levelUIManager;
    public int[] levelMoney;
    public GameManager gameManager;
    public BlueprintFactory blueprintFactory;
    List<Blueprint> blueprintsInPocket=new List<Blueprint>();
    Blueprint blueprintInBuilding; 
    PlayerWish playerWish;
    public PlayerWish PlayerWish { get => playerWish; set => playerWish = value; }
    public Blueprint BlueprintInBuilding { get => blueprintInBuilding; set => blueprintInBuilding = value; }
    public List<Blueprint> BlueprintsInPocket { get => blueprintsInPocket; set => blueprintsInPocket = value; }
    public List<Blueprint> GetBluePrints(int bluePrintsN)
    {
        List<Blueprint> bluePrints = blueprintFactory.GetBluePrints(bluePrintsN);
        for (int j = 0; j < bluePrints.Count; j++)
        {
            bluePrints[j].CheckElement();
        }
        return bluePrints;
    }
    public void BuildBlueprint(Blueprint blueprint)
    {

    }
    public void BuyBlueprint(Blueprint blueprint)
    {
        BlueprintsInPocket.Add(blueprint);
    }
    public void deleteBlueprint(Blueprint blueprint)
    {
        BlueprintsInPocket.Remove(blueprint);
    }

    int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            //gameManager..PlayerLevel = value;
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

    int lotteryDraw = 100;
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

//玩家想要啥
public enum PlayerWish
{
    Element,Composition
}
