using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrapTips : TileTips
{
    [SerializeField] GameObject DamageAnalysisArea = default;
    [SerializeField] GameObject CoinAnalysisArea = default;
    [SerializeField] Text CoinAnalysisText = default;
    [SerializeField] Text AnalysisTxt = default;
    [SerializeField] TextMeshProUGUI switchTrapCostTxt = default;
    [SerializeField] GameObject switchTrapArea = default;
    TrapContent m_Trap;
    TrapAttribute m_TrapAtt;
    [SerializeField] Animator tileinfo_Anim = default;

    public override void Show()
    {
        base.Show();
        tileinfo_Anim.SetTrigger("Show");
    }
    public void ReadTrap(TrapContent trapContent, int cost)
    {
        m_Trap = trapContent;
        m_TrapAtt = m_Trap.TrapAttribute;
        if (trapContent.IsReveal)
        {
            BasicInfo();
            switchTrapArea.SetActive(!m_Trap.Important);
            switchTrapCostTxt.text = GameMultiLang.GetTraduction("SWITCHTRAP") + "<sprite=7>" + cost.ToString();
        }
        else
        {
            Icon.sprite = StaticData.Instance.UnrevealTrap;
            Name.text = GameMultiLang.GetTraduction("UNREVEAL");
            Description.text = GameMultiLang.GetTraduction("UNREVEALINFO");
            switchTrapArea.SetActive(false);
        }

        if (trapContent.DamageAnalysis > 0)
        {
            DamageAnalysisArea.SetActive(true);
            AnalysisTxt.text = m_Trap.DamageAnalysis.ToString();
        }
        else
        {
            DamageAnalysisArea.SetActive(false);
        }

        if (trapContent.CoinAnalysis > 0)
        {
            CoinAnalysisArea.SetActive(true);
            CoinAnalysisText.text = m_Trap.CoinAnalysis.ToString();
        }
        else
        {
            CoinAnalysisArea.SetActive(false);
        }


    }

    public void ReadTrapAtt(TrapAttribute trapAtt)
    {
        m_TrapAtt = trapAtt;
        BasicInfo();
        switchTrapArea.SetActive(false);
        DamageAnalysisArea.SetActive(false);
        CoinAnalysisArea.SetActive(false);
    }

    private void BasicInfo()
    {
        Icon.sprite = m_TrapAtt.Icon;
        Name.text = GameMultiLang.GetTraduction(m_TrapAtt.Name);
        Description.text = GameMultiLang.GetTraduction(m_TrapAtt.Description);
    }

    public void SwitchTrap()
    {
        GameManager.Instance.SwitchTrap(m_Trap);
    }

}
