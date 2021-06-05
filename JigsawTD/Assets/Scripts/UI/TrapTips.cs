using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapTips : TileTips
{
    [SerializeField] GameObject AnalysisArea = default;
    [SerializeField] Text AnalysisTxt = default;
    public void ReadTrap(TrapContent trapContent)
    {
        Icon.sprite = trapContent.m_TrapAttribute.Icon;
        Name.text = trapContent.m_TrapAttribute.Name;
        Description.text = trapContent.m_TrapAttribute.Description;
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
