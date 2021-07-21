using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncUI : IUserInterface
{
    public GameObject DrawBtnObj;
    public GameObject LuckyObj;
    public GameObject NextBtnObj;
    public GameObject LevelBtnObj;


    //[SerializeField] Text LuckPointTxt = default;
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpTxt = default;
    [SerializeField] Text PlayerLevelTxt = default;
    [SerializeField] InfoBtn m_LuckInfo = default;
    [SerializeField] InfoBtn m_LevelInfo = default;
    [SerializeField] LuckProgress m_LuckProgress = default;
    Animator m_Anim;

    bool drawThisTurn = true;
    public bool DrawThisTurn { get => drawThisTurn; set => drawThisTurn = value; }

    private int energyProgress = 0;
    public int EnergyProgress
    {
        get => energyProgress;
        set => energyProgress = Mathf.Min(5, value);
        //{
        //    if (value > 1)
        //    {
        //        energyProgress = 0;
        //        Energy++;
        //    }
        //    else
        //    {
        //        energyProgress = value;
        //    }
        //    m_LuckInfo.SetContent(StaticData.GetLuckyInfo(Energy, EnergyProgress));
        //}
    }

    private int drawRemain = 0;
    public int DrawRemain
    {
        get => drawRemain;
        set
        {
            drawRemain = value;
            DrawBtnTxt.text = /*GameMultiLang.GetTraduction("DRAWMODULE")*/ "X" + drawRemain.ToString();
            //if (drawRemain <= 0)
            //{
            //    DrawBtnTxt.text = "抽取模块(金币" + buyShapeCost + ")";
            //}
            //else
            //{
            //    DrawBtnTxt.text = "抽取模块X" + drawRemain.ToString();
            //}
        }
    }

    private int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set
        {
            playerLevel = value;
            PlayerLevelTxt.text = PlayerLevel.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[PlayerLevel];
            if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
            {
                LevelUpTxt.text = PlayerLvUpMoney.ToString();
            }
            else
            {
                LevelUpTxt.text = "MAX";
            }
            m_LevelInfo.SetContent(StaticData.GetLevelInfo(playerLevel));
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

    private int energy;
    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Min(10, value);
            if (energy >= 10)
            {
                energy = 0;
                DrawRemain++;
                //GameManager.Instance.GetRandomBluePrint();
            }
            m_LuckInfo.SetContent(StaticData.GetEnergyInfo());
            m_LuckProgress.SetProgress(energy);
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        DrawRemain = StaticData.Instance.StartLotteryDraw;
        Energy = 0;
        PlayerLevel = 1;

        m_Anim = this.GetComponent<Animator>();
    }



    public void PrepareForGuide()
    {
        DrawBtnObj.SetActive(false);
        NextBtnObj.SetActive(false);
        LuckyObj.SetActive(false);
        LevelBtnObj.SetActive(false);
    }

    public override void Show()
    {
        m_Anim.SetBool("Show", true);
    }

    public override void Hide()
    {
        m_Anim.SetBool("Show", false);
    }


    public void DrawBtnClick()
    {
        if (DrawRemain > 0)
        {
            Energy = 0;
            DrawRemain--;
            DrawThisTurn = true;
            m_GameManager.DrawShapes();
        }
        else
        {
            GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOENOUGHDRAW"));
        }
        //else if (GameManager.Instance.ConsumeMoney(buyShapeCost))
        //{
        //    LuckyCoin = 0;
        //    DrawThisTurn = true;
        //    m_GameManager.DrawShapes();
        //    buyShapeCost += 25;
        //    DrawRemain = 0;
        //}

    }

    public void PrepareNextWave()
    {
        if (!DrawThisTurn)
        {
            EnergyProgress++;
        }
        else
        {
            EnergyProgress = 1;
        }
        Energy += EnergyProgress;
        DrawThisTurn = false;
        m_GameManager.GainDraw(1);
    }

    public void NextWaveBtnClick()
    {
        m_GameManager.StartNewWave();
    }

    public void LevelUpBtnClick()
    {
        if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(PlayerLvUpMoney))
            {
                PlayerLevel++;
                if (PlayerLevel == 4 || PlayerLevel == 7)//4和7级增加一个商店容量
                {
                    m_GameManager.IncreaseShopCapacity();
                }
            }
        }
    }
}
