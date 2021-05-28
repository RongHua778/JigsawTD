using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTips : TileTips
{
    [SerializeField] Text RangeTypeValue = default;
    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text SputteringValue = default;
    [SerializeField] Text SlowRateValue = default;
    [SerializeField] Text AnalysisValue = default;
    [SerializeField] GameObject BtnArea = default;
    [SerializeField] GameObject IntensifyArea = default;
    [SerializeField] GameObject AnalysisArea = default;
    [SerializeField] TipsElementConstruct elementConstruct = default;

    private Turret m_Turret;
    private BluePrintGrid m_BGrid;
    public void ReadTurret(Turret turret)
    {

        this.m_Turret = turret;
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;
        string rangeTypeTxt = "";
        switch (turret.m_TurretAttribute.RangeType)
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

        UpdateInfo(turret);

        if (turret.m_TurretAttribute.TurretLevels[turret.Quality-1].TurretEffects.Count > 0)
        {
            string finalDes = turret.m_TurretAttribute.Description;
            foreach (TurretEffectInfo effect in turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretEffects)
            {
                finalDes += effect.EffectDescription;
                finalDes += "\n";
            }
            Description.text = finalDes;
        }
        else
        {
            Description.text = turret.m_TurretAttribute.Description;
        }
        if (turret.Element == Element.None)
        {
            elementConstruct.gameObject.SetActive(true);
            elementConstruct.SetElements(turret.Compositions);
            IntensifyArea.SetActive(false);
        }
        else
        {
            elementConstruct.gameObject.SetActive(false);
            IntensifyArea.SetActive(true);
        }
        BtnArea.SetActive(false);
        AnalysisArea.SetActive(true);
    }

    private void UpdateInfo(Turret turret)
    {
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        CriticalValue.text = turret.CriticalRate.ToString();
        SputteringValue.text = turret.SputteringRange.ToString();
        SlowRateValue.text = turret.SlowRate.ToString();
        AnalysisValue.text = turret.DamageAnalysis.ToString();
    }

    public void ReadAttribute(BluePrintGrid bGrid)
    {
        m_BGrid = bGrid;
        TurretAttribute attribute = bGrid.BluePrint.CompositeTurretAttribute;
        Icon.sprite = attribute.TurretLevels[0].Icon;
        Name.text = attribute.Name;
        AttackValue.text = attribute.TurretLevels[0].AttackDamage.ToString();
        SpeedValue.text = attribute.TurretLevels[0].AttackSpeed.ToString();
        RangeValue.text = attribute.TurretLevels[0].AttackRange.ToString();
        CriticalValue.text = attribute.TurretLevels[0].CriticalRate.ToString();
        SputteringValue.text = attribute.TurretLevels[0].SputteringRange.ToString();
        SlowRateValue.text = attribute.TurretLevels[0].SlowRate.ToString();

        if (attribute.TurretLevels[0].TurretEffects.Count > 0)
        {
            string finalDes = attribute.Description+ "\n";
            foreach (TurretEffectInfo effect in attribute.TurretLevels[0].TurretEffects)
            {
                finalDes += effect.EffectDescription;
                finalDes += "\n";
            }
            Description.text = finalDes;
        }
        else
        {
            Description.text = attribute.Description;
        }
        elementConstruct.gameObject.SetActive(true);
        elementConstruct.SetElements(bGrid.BluePrint.Compositions);
        IntensifyArea.SetActive(false);
        BtnArea.SetActive(true);
        AnalysisArea.SetActive(false);
    }

    public void CloseTips()
    {
        this.gameObject.SetActive(false);
    }

    public void BuyBluePrintBtnClick()
    {
        if (LevelUIManager.Instance.ConsumeMoney(StaticData.BuyBluePrintCost))
        {
            m_BGrid.MoveToPocket();
        }
    }

    public void CompositeBtnClick()
    {
        if (!m_BGrid.BuildAble)
        {
            GameEvents.Instance.Message("缺少必要素材");
            return;
        }
        GameManager.Instance.GenerateDShape(m_BGrid.BluePrint.CompositeTurretAttribute);
        CloseTips();
        m_BGrid.RemoveBuildPrint();
    }

    private void FixedUpdate()
    {
        if (m_Turret != null)
            UpdateInfo(m_Turret);
    }

}
