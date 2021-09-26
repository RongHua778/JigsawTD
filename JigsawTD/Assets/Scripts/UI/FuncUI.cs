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
    [SerializeField] Text DiscountTxt = default;
    [SerializeField] InfoBtn m_LuckInfo = default;
    [SerializeField] InfoBtn m_LevelInfo = default;
    [SerializeField] InfoBtn m_DrawInfo = default;
    [SerializeField] LuckProgress m_LuckProgress = default;
    Animator m_Anim;

    float discountRate = 0.1f;
    public float DiscountRate { 
        get => discountRate; 
        set
        {
            discountRate = value;
            DiscountTxt.text = discountRate * 100 + "%";
        } 
    }

    bool drawThisTurn = true;
    public bool DrawThisTurn { get => drawThisTurn; set => drawThisTurn = value; }

    private int energyProgress = 0;
    public int EnergyProgress
    {
        get => energyProgress;
        set => energyProgress = Mathf.Min(5, value);
    }

    int freeShapeCount;
    public int FreeShapeCount
    {
        get => freeShapeCount;
        set
        {
            freeShapeCount = value;
            if (freeShapeCount > 0)
                DrawBtnTxt.text = 0.ToString();
            else
                DrawBtnTxt.text = buyShapeCost.ToString();
        }
    }

    int buyShapeCost;
    public int BuyShapeCost
    {
        get => buyShapeCost;
        set
        {
            buyShapeCost = Mathf.Max(0, value);
            DrawBtnTxt.text = buyShapeCost.ToString();
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
            m_LevelInfo.SetContent(StaticData.GetLevelInfo(moduleLevel));
            if (ModuleLevel == 2 || ModuleLevel == 4 || ModuleLevel == 6)//2，4,6级增加一个商店容量
            {
                GameManager.Instance.IncreaseShopCapacity(1);
            }
            StaticData.Instance.ContentFactory.PlayerLevelUp(moduleLevel);
        }
    }

    int playerLvUpMoney = 0;
    public int PlayerLvUpMoney
    {
        get => playerLvUpMoney;
        set
        {
            playerLvUpMoney = value;
            if (ModuleLevel < StaticData.Instance.PlayerMaxLevel)
            {
                LevelUpTxt.text = PlayerLvUpMoney.ToString();
            }
            else
            {
                LevelUpTxt.text = "MAX";
            }
        }
    }

    private int energy;
    public int Energy
    {
        get => energy;
        set
        {
            energy = Mathf.Min(5, value);
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
        DiscountRate = 0.1f;
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
        if (FreeShapeCount > 0)
        {
            FreeShapeCount--;
        }
        else if (GameManager.Instance.ConsumeMoney(BuyShapeCost))
        {
            BuyShapeCost += StaticData.Instance.MultipleShapeCost;
        }
        else
        {
            return;
        }
        DrawThisTurn = true;
        GameManager.Instance.DrawShapes();
        GameManager.Instance.CheckDrawSkill();

    }

    public void PrepareNextWave()
    {
        Show();
        m_DrawInfo.SetContent(GameMultiLang.GetTraduction("DRAWINFO") + "<color=cyan>" + discountRate * 100 + "%</color>");
        if (!DrawThisTurn)
        {
            //EnergyProgress++;
            //Energy++;
            //BuyShapeCost -= Energy;
            BuyShapeCost = Mathf.RoundToInt(BuyShapeCost * (1 - discountRate));//没抽就减5%的价格
        }
        else
        {
            //Energy = 0;
            //EnergyProgress = 1;
        }
        //Energy += EnergyProgress;
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
