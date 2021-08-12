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

    }

    int buyShapeCost=25;
    public int BuyShapeCost
    {
        get => buyShapeCost;
        set
        {
            buyShapeCost = value;
            DrawBtnTxt.text = /*GameMultiLang.GetTraduction("DRAWMODULE")*/ buyShapeCost.ToString();
        }
    }

    private int drawRemain = 0;
    public int DrawRemain
    {
        get => drawRemain;
        set
        {
            drawRemain = value;

        }
    }

    private int moduleLevel = 1;
    public int ModuleLevel
    {
        get => moduleLevel;
        set
        {
            moduleLevel = value;
            PlayerLevelTxt.text = ModuleLevel.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[ModuleLevel];
            if (ModuleLevel < StaticData.Instance.PlayerMaxLevel)
            {
                LevelUpTxt.text = PlayerLvUpMoney.ToString();
            }
            else
            {
                LevelUpTxt.text = "MAX";
            }
            m_LevelInfo.SetContent(StaticData.GetLevelInfo(moduleLevel));
            if (ModuleLevel == 3 || ModuleLevel == 5 || ModuleLevel == 7)//3,5和7级增加一个商店容量
            {
                GameManager.Instance.IncreaseShopCapacity();
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
            }
            m_LuckInfo.SetContent(StaticData.GetEnergyInfo());
            m_LuckProgress.SetProgress(energy);
        }
    }





    public override void Initialize()
    {
        BuyShapeCost = StaticData.Instance.BaseShapeCost;
        Energy = 0;
        ModuleLevel = 1;

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
        //if (DrawRemain > 0)
        //{
        //    Energy = 0;
        //    DrawRemain--;
        //    DrawThisTurn = true;
        //    m_GameManager.DrawShapes();
        //}
        //else
        //{
        //    GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("NOENOUGHDRAW"));
        //}
        if(GameManager.Instance.ConsumeMoney(BuyShapeCost))
        {
            //DrawThisTurn = true;
            GameManager.Instance.DrawShapes();
            BuyShapeCost += StaticData.Instance.MultipleShapeCost;
            GameManager.Instance.CheckDrawSkill();
            //DrawRemain = 0;
        }

    }

    public void PrepareNextWave()
    {
        Show();
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
        GameManager.Instance.GainDraw(1);
    }

    public void NextWaveBtnClick()
    {
        GameManager.Instance.StartNewWave();
    }

    public void LevelUpBtnClick()
    {
        if (ModuleLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(PlayerLvUpMoney))
            {
                ModuleLevel++;
            }
        }
    }
}
