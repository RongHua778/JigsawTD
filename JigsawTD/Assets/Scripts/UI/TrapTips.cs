using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapTips : TileTips
{
    [SerializeField] GameObject AnalysisArea = default;
    [SerializeField] Text AnalysisTxt = default;
    [SerializeField] Text switchTrapCostTxt = default;
    [SerializeField] GameObject switchTrapArea = default;
    TrapContent m_Trap;
    public void ReadTrap(TrapContent trapContent, int cost)
    {
        m_Trap = trapContent;
        if (trapContent.IsReveal)
        {
            Icon.sprite = trapContent.TrapAttribute.Icon;
            Name.text = GameMultiLang.GetTraduction(trapContent.TrapAttribute.Name);
            Description.text = GameMultiLang.GetTraduction(trapContent.TrapAttribute.Description);
            switchTrapCostTxt.text = cost.ToString();
            switchTrapArea.SetActive(true);
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
            AnalysisArea.SetActive(true);
            AnalysisTxt.text = trapContent.DamageAnalysis.ToString();
        }
        else
        {
            AnalysisArea.SetActive(false);
        }

    }

    public void SwitchTrap()
    {
        GameManager.Instance.SwitchTrap(m_Trap);
    }

}
