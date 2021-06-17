using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BluePrintTips : TileTips
{
    [SerializeField] Text RangeTypeValue = default;
    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text SputteringValue = default;
    [SerializeField] Text SlowRateValue = default;
    [SerializeField] TipsElementConstruct elementConstruct = default;//合成塔组成元素区
    [SerializeField] GameObject BuyBtn = default;

    private BluePrintGrid m_Grid;

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
    }

    public void ReadBluePrint(BluePrintGrid grid)//通过场上防御塔查看
    {
        this.m_Grid = grid;
        TurretAttribute attribute = grid.BluePrint.CompositeTurretAttribute;

        BuyBtn.SetActive(grid.InShop);

        Icon.sprite = attribute.TurretLevels[0].Icon;
        Name.text = attribute.TurretLevels[0].TurretName;

        //设置攻击范围类型
        SetRangeType(attribute);

        //即时更新攻击，攻速，伤害统计等数据
        UpdateInfo(grid.BluePrint);

        elementConstruct.SetElements(grid.BluePrint);
        //设置描述文案
        Description.text = StaticData.GetTurretDes(attribute, 1);
    }

    private void SetRangeType(TurretAttribute attribute)
    {
        string rangeTypeTxt = "";
        switch (attribute.RangeType)
        {
            case RangeType.Circle:
                rangeTypeTxt = "圆型";
                break;
            case RangeType.HalfCircle:
                rangeTypeTxt = "半圆型";
                break;
            case RangeType.Line:
                rangeTypeTxt = "直线型";
                break;
        }
        this.RangeTypeValue.text = rangeTypeTxt;
    }

    private void UpdateInfo(Blueprint bluePrint)
    {
        TurretAttribute attribute = bluePrint.CompositeTurretAttribute;
        float damage = attribute.TurretLevels[0].AttackDamage;
        if (attribute.Name == "G1")
        {
            damage *= 0.5f;//G1攻击力减半
        }
        AttackValue.text = damage.ToString() + (bluePrint.CompositeAttackDamage > 0 ?
            "<color=cyan>+" + damage * bluePrint.CompositeAttackDamage + "</color>" : "");

        SpeedValue.text = attribute.TurretLevels[0].AttackSpeed.ToString() + (bluePrint.CompositeAttackSpeed > 0 ?
            "<color=cyan>+" + attribute.TurretLevels[0].AttackSpeed * bluePrint.CompositeAttackSpeed + "</color>" : "");

        RangeValue.text = attribute.TurretLevels[0].AttackRange.ToString();

        CriticalValue.text = (attribute.TurretLevels[0].CriticalRate * 100).ToString() + (bluePrint.CompositeCriticalRate > 0 ?
            "<color=cyan>+" + bluePrint.CompositeCriticalRate * 100 + "</color>" : "") + "%";

        SputteringValue.text = attribute.TurretLevels[0].SputteringRange.ToString() + (bluePrint.CompositeSputteringRange > 0 ?
            "<color=cyan>+" + bluePrint.CompositeSputteringRange + "</color>" : "");

        SlowRateValue.text = attribute.TurretLevels[0].SlowRate.ToString() + (bluePrint.CompositeSlowRate > 0 ?
            "<color=cyan>+" + bluePrint.CompositeSlowRate + "</color>" : "");
    }

    public void BuyBluePrintBtnClick() 
    {
        GameManager.Instance.BuyBluePrint(m_Grid, StaticData.BuyBluePrintCost);
        BuyBtn.SetActive(m_Grid.InShop);
    }

    public void CompositeBtnClick()
    {
        GameManager.Instance.CompositeShape(m_Grid);
    }



}
