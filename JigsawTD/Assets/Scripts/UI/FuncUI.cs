using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuncUI : IUserInterface
{
    static Animator BuildBtnAnim, SystemBtnAnim, NextWaveBtnAnim;



    [SerializeField] TextMeshProUGUI DrawBtnTxt = default;
    [SerializeField] TextMeshProUGUI SystemUpgradeCostTxt = default;
    [SerializeField] Text SystemLevelTxt = default;
    [SerializeField] Text DiscountTxt = default;
    [SerializeField] InfoBtn m_DrawInfo = default;
    //Animator m_Anim;

    public float DiscountRate
    {
        set
        {
            DiscountTxt.text = GameRes.BuildDiscount * 100 + "%";
            m_DrawInfo.SetContent(GameMultiLang.GetTraduction("DRAWINFO") + "<color=cyan>" + GameRes.BuildDiscount * 100 + "%</color>");

        }
    }


    public int BuyShapeCost
    {
        set => DrawBtnTxt.text = "<sprite=7>" + GameRes.BuildCost.ToString();
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
                SystemUpgradeCostTxt.text = "<sprite=7>" + value.ToString();
            }
            else
            {
                SystemUpgradeCostTxt.text = "MAX";
            }
        }
    }


    public override void Initialize()
    {
        base.Initialize();
        BuildBtnAnim = m_RootUI.transform.Find("Build").GetComponent<Animator>();
        NextWaveBtnAnim = m_RootUI.transform.Find("NextWave").GetComponent<Animator>();
        SystemBtnAnim = m_RootUI.transform.Find("System").GetComponent<Animator>();
    }


    public override void Show()
    {
        BuildBtnAnim.SetBool("Show", true);
        NextWaveBtnAnim.SetBool("Show", true);
        SystemBtnAnim.SetBool("Show", true);

    }

    public override void Hide()
    {
        BuildBtnAnim.SetBool("Show", false);
        NextWaveBtnAnim.SetBool("Show", false);
        SystemBtnAnim.SetBool("Show", false);
    }

    public static void PlayFuncUIAnim(int partID, string key, bool value)
    {
        switch (partID)
        {
            case 0:
                BuildBtnAnim.SetBool(key, value);
                break;
            case 1:
                NextWaveBtnAnim.SetBool(key, value);
                break;
            case 2:
                SystemBtnAnim.SetBool(key, value);
                break;
        }
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

}
