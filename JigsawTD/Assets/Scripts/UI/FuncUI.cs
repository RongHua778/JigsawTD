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

    [SerializeField] LevelInfoPanel m_LevelInfoPanel = default;

    //[SerializeField] Text LuckPointTxt = default;
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpTxt = default;
    [SerializeField] Text PlayerLevelTxt = default;
    [SerializeField] Text DiscountTxt = default;
    [SerializeField] InfoBtn m_LuckInfo = default;
    [SerializeField] InfoBtn m_DrawInfo = default;
    [SerializeField] LuckProgress m_LuckProgress = default;
    Animator m_Anim;

    public float DiscountRate { 
        set
        {
            DiscountTxt.text = GameRes.DiscountRate * 100 + "%";
            m_DrawInfo.SetContent(GameMultiLang.GetTraduction("DRAWINFO") + "<color=cyan>" + GameRes.DiscountRate * 100 + "%</color>");

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
                DrawBtnTxt.text = GameRes.BuyShapeCost.ToString();
        }
    }

    public int BuyShapeCost
    {
        set=> DrawBtnTxt.text = GameRes.BuyShapeCost.ToString();
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

    public int ModuleLevel
    {
        set
        {
            PlayerLevelTxt.text = value.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[value];
            if (value == 2 || value == 4 || value == 6)//2��4,6������һ���̵�����
            {
                GameRes.ShopCapacity++;
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
            if (GameRes.ModuleLevel < StaticData.Instance.PlayerMaxLevel)
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
        m_LevelInfoPanel.Initialize();
        m_LevelInfoPanel.SetInfo();
        Energy = 0;
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
        else if (GameManager.Instance.ConsumeMoney(GameRes.BuyShapeCost))
        {
            GameRes.BuyShapeCost += StaticData.Instance.MultipleShapeCost;
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
        if (!DrawThisTurn)
        {
            GameRes.BuyShapeCost = Mathf.RoundToInt(GameRes.BuyShapeCost * (1 - GameRes.DiscountRate));//û��ͼ�5%�ļ۸�
        }

        DrawThisTurn = false;
        GameManager.Instance.GainDraw(1);
    }

    public void NextWaveBtnClick()
    {
        GameManager.Instance.StartNewWave();
    }

    public void LevelUpBtnClick()
    {
        if (GameRes.ModuleLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(PlayerLvUpMoney))
            {
                GameRes.ModuleLevel++;
                m_LevelInfoPanel.SetInfo();
            }
        }
    }
}
