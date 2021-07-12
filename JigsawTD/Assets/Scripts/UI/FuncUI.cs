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

    private int luckProgress = 0;
    public int LuckProgress
    {
        get => luckProgress;
        set
        {
            if (value > 2)
            {
                luckProgress = 0;
                LuckyCoin++;
            }
            else
            {
                luckProgress = value;
            }
            m_LuckInfo.SetContent(StaticData.GetLuckyInfo(LuckyCoin, LuckProgress));
        }
    }

    private int drawRemain = 0;
    public int DrawRemain
    {
        get => drawRemain;
        set
        {
            drawRemain = value;
            DrawBtnTxt.text = "抽取模块X" + drawRemain.ToString();
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

    private int luckyCoin;
    public int LuckyCoin
    {
        get => luckyCoin;
        set
        {
            luckyCoin = Mathf.Min(5, value);
            //if (luckyCoin >= 10)
            //{
            //    luckyCoin = 0;
            //    DrawRemain++;
            //    //GameManager.Instance.GetRandomBluePrint();
            //}
            //LuckPointTxt.text = "累积点:" + luckyCoin.ToString();
            m_LuckInfo.SetContent(StaticData.GetLuckyInfo(LuckyCoin,LuckProgress));
            m_LuckProgress.SetProgress(luckyCoin);
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        DrawRemain = StaticData.Instance.StartLotteryDraw;
        LuckyCoin = 1;
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
            LuckyCoin = 0;
            DrawRemain--;
            DrawThisTurn = true;
            m_GameManager.DrawShapes();
        }
        else
        {
            GameManager.Instance.ShowMessage("抽取次数不足");
        }
    }

    public void PrepareNextWave()
    {
        if (!DrawThisTurn)
        {
            LuckyCoin += 1;
            //LuckProgress += 1;
        }
        else
        {
            LuckyCoin = 1;
            //LuckProgress = 1;
        }
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
            }
        }
    }
}
