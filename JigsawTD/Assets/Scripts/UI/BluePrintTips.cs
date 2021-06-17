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
    [SerializeField] TipsElementConstruct elementConstruct = default;//�ϳ������Ԫ����
    [SerializeField] GameObject BuyBtn = default;

    private BluePrintGrid m_Grid;

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
    }

    public void ReadBluePrint(BluePrintGrid grid)//ͨ�����Ϸ������鿴
    {
        this.m_Grid = grid;
        TurretAttribute attribute = grid.BluePrint.CompositeTurretAttribute;

        BuyBtn.SetActive(grid.InShop);

        Icon.sprite = attribute.TurretLevels[0].Icon;
        Name.text = attribute.TurretLevels[0].TurretName;

        //���ù�����Χ����
        SetRangeType(attribute);

        //��ʱ���¹��������٣��˺�ͳ�Ƶ�����
        UpdateInfo(grid.BluePrint);

        elementConstruct.SetElements(grid.BluePrint);
        //���������İ�
        Description.text = StaticData.GetTurretDes(attribute, 1);
    }

    private void SetRangeType(TurretAttribute attribute)
    {
        string rangeTypeTxt = "";
        switch (attribute.RangeType)
        {
            case RangeType.Circle:
                rangeTypeTxt = "Բ��";
                break;
            case RangeType.HalfCircle:
                rangeTypeTxt = "��Բ��";
                break;
            case RangeType.Line:
                rangeTypeTxt = "ֱ����";
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
            damage *= 0.5f;//G1����������
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
