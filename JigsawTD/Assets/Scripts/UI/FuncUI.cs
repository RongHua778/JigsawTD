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

    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text SystemUpgradeCostTxt = default;
    [SerializeField] Text SystemLevelTxt = default;
    [SerializeField] Text DiscountTxt = default;
    [SerializeField] InfoBtn m_DrawInfo = default;
    Animator m_Anim;

    public float DiscountRate { 
        set
        {
            DiscountTxt.text = GameRes.BuildDiscount * 100 + "%";
            m_DrawInfo.SetContent(GameMultiLang.GetTraduction("DRAWINFO") + "<color=cyan>" + GameRes.BuildDiscount * 100 + "%</color>");

        }
    }

    bool drawThisTurn = true;
    public bool DrawThisTurn { get => drawThisTurn; set => drawThisTurn = value; }

    public int BuyShapeCost
    {
        set=> DrawBtnTxt.text = GameRes.BuildCost.ToString();
    }


    public int SystemLevel
    {
        set
        {
            SystemLevelTxt.text = value.ToString();
        }
    }

    public int SystemUpgradeCost
    {
        set
        {
            if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
            {
                SystemUpgradeCostTxt.text = value.ToString();
            }
            else
            {
                SystemUpgradeCostTxt.text = "MAX";
            }
        }
    }


    public override void Initialize()
    {
        m_LevelInfoPanel.Initialize();
        m_LevelInfoPanel.SetInfo();
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
        if (GameManager.Instance.ConsumeMoney(GameRes.BuildCost))
        {
            GameRes.BuildCost += StaticData.Instance.MultipleShapeCost;
        }
        else
        {
            return;
        }
        GameRes.DrawThisTurn = true;
        GameManager.Instance.DrawShapes();
        GameManager.Instance.CheckDrawSkill();

    }



    public void NextWaveBtnClick()
    {
        GameManager.Instance.StartNewWave();
    }

    public void LevelUpBtnClick()
    {
        if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(GameRes.SystemUpgradeCost))
            {
                GameRes.SystemLevel++;
                m_LevelInfoPanel.SetInfo();
            }
        }
    }
}
