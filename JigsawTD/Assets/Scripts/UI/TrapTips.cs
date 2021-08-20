using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapTips : TileTips
{
    [SerializeField] GameObject AnalysisArea = default;
    [SerializeField] Text AnalysisTxt = default;
    [SerializeField] Text switchTrapCostTxt = default;
    [SerializeField] GameObject changePosArea = default;
    public void ReadTrap(TrapContent trapContent, int cost)
    {
        if (trapContent.IsReveal)
        {
            Icon.sprite = trapContent.TrapAttribute.Icon;
            Name.text = GameMultiLang.GetTraduction(trapContent.TrapAttribute.Name);
            Description.text = GameMultiLang.GetTraduction(trapContent.TrapAttribute.Description);
            switchTrapCostTxt.text = cost.ToString();
            changePosArea.SetActive(true);
        }
        else
        {
            Icon.sprite = StaticData.Instance.UnrevealTrap;
            Name.text = GameMultiLang.GetTraduction("UNREVEAL");
            Description.text = GameMultiLang.GetTraduction("UNREVEALINFO");
            changePosArea.SetActive(false);
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

}
