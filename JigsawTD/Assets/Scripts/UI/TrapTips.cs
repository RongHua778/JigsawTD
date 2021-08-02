using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapTips : TileTips
{
    [SerializeField] GameObject AnalysisArea = default;
    [SerializeField] Text AnalysisTxt = default;
    [SerializeField] Text switchTrapCostTxt=default;
    [SerializeField] Button switchTrapButton;
    public void ReadTrap(TrapContent trapContent,int switchTrapCost,bool relocatable)
    {
        Icon.sprite = trapContent.m_TrapAttribute.Icon;
        Name.text = GameMultiLang.GetTraduction(trapContent.m_TrapAttribute.Name);
        Description.text = GameMultiLang.GetTraduction(trapContent.m_TrapAttribute.Description);
        switchTrapCostTxt.text = switchTrapCost.ToString();
        Collider2D[] attachedResult = new Collider2D[20];
        int hits = Physics2D.OverlapCircleNonAlloc(BoardSystem.SelectingTile.transform.position, 
            0.5f, attachedResult, LayerMask.GetMask(StaticData.ConcreteTileMask));
        if (relocatable&& hits>1)
        {
            switchTrapButton.gameObject.SetActive(true);
        }
        else
        {
            switchTrapButton.gameObject.SetActive(false);
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
