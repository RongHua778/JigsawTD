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
    [SerializeField] Text IntensifyValue = default;
    [SerializeField] Text AnalysisValue = default;
    [SerializeField] Text UpgradeCostValue = default;
    [SerializeField] GameObject UpgradeArea = default;//合成塔升级区
    [SerializeField] GameObject BtnArea = default;//购买/合成按钮区
    [SerializeField] GameObject IntensifyArea = default;//元素塔加成效果区
    [SerializeField] GameObject AnalysisArea = default;//伤害统计区
    [SerializeField] TipsElementConstruct elementConstruct = default;//合成塔组成元素区
    //合成塔升级区

    private Turret m_Turret;
    private BluePrintGrid m_BGrid;
    int upgradeCost;


    public void ReadTurret(Turret turret)//通过场上防御塔查看
    {
        anim.SetBool("isOpen", true);


        this.m_Turret = turret;
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;
        //设置攻击范围类型
        SetRangeType(turret.m_TurretAttribute);
        //即时更新攻击，攻速，伤害统计等数据
        UpdateInfo(turret);
        //设置描述文案
        SetDescription(turret);
        //控制区块显示
        if (turret.TurretType == TurretType.CompositeTurret)
        {
            elementConstruct.gameObject.SetActive(true);
            elementConstruct.SetElements(((CompositeTurret)turret).CompositeBluePrint.Compositions);
            IntensifyArea.SetActive(false);
            if (turret.Quality < 3)
            {
                UpgradeArea.SetActive(true);
                upgradeCost = StaticData.Instance.LevelUpCost[turret.m_TurretAttribute.Rare, turret.Quality - 1];
                UpgradeCostValue.text = upgradeCost.ToString();
            }
            else
                UpgradeArea.SetActive(false);

        }
        else
        {
            elementConstruct.gameObject.SetActive(false);
            IntensifyArea.SetActive(true);
            string intensifyType = "";//根据元素及品质设置显示加成效果
            //intensifyType += "作为合成素材时，使合成塔";
            switch (turret.Element)
            {
                case Element.Gold:
                    intensifyType += StaticData.Instance.GoldAttackIntensify * 100 * turret.Quality + "%攻击";
                    break;
                case Element.Wood:
                    intensifyType += StaticData.Instance.WoodSpeedIntensify * 100 * turret.Quality + "%攻速";
                    break;
                case Element.Water:
                    intensifyType += StaticData.Instance.WaterSlowIntensify * turret.Quality + "减速";
                    break;
                case Element.Fire:
                    intensifyType += StaticData.Instance.FireCriticalIntensify * 100 * turret.Quality + "%暴击率";
                    break;
                case Element.Dust:
                    intensifyType += StaticData.Instance.FireCriticalIntensify * turret.Quality + "溅射";
                    break;
                case Element.None:
                    break;
            }
            IntensifyValue.text = intensifyType;
            UpgradeArea.SetActive(false);
        }
        BtnArea.SetActive(false);
        AnalysisArea.SetActive(true);
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

    private void SetDescription(Turret turret)
    {
        if (turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretEffects.Count > 0)
        {
            string finalDes = "";
            if (turret.m_TurretAttribute.Description != "")
                finalDes += turret.m_TurretAttribute.Description + "\n";
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
    }

    private void UpdateInfo(Turret turret)
    {
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        CriticalValue.text = (turret.CriticalRate * 100).ToString() + "%";
        SputteringValue.text = turret.SputteringRange.ToString();
        SlowRateValue.text = turret.SlowRate.ToString();
        AnalysisValue.text = turret.DamageAnalysis.ToString();
    }

    public void ReadAttribute(BluePrintGrid bGrid)//通过配方查看
    {
        anim.SetBool("isOpen", true);


        m_Turret = null;
        m_BGrid = bGrid;
        TurretAttribute attribute = bGrid.BluePrint.CompositeTurretAttribute;
        Icon.sprite = attribute.TurretLevels[0].Icon;
        Name.text = attribute.TurretLevels[0].TurretName;

        SetRangeType(attribute);

        string damageIntensify = bGrid.BluePrint.CompositeAttackDamage <= 0 ? "" : "<color=#00ffffff> +" + (attribute.TurretLevels[0].AttackDamage * bGrid.BluePrint.CompositeAttackDamage).ToString() + "</color>";
        AttackValue.text = attribute.TurretLevels[0].AttackDamage.ToString() + damageIntensify;

        string speedIntensify = bGrid.BluePrint.CompositeAttackSpeed <= 0 ? "" : "<color=#00ffffff> +" + (attribute.TurretLevels[0].AttackSpeed * bGrid.BluePrint.CompositeAttackSpeed).ToString() + "</color>";
        SpeedValue.text = attribute.TurretLevels[0].AttackSpeed.ToString() + speedIntensify;

        RangeValue.text = attribute.TurretLevels[0].AttackRange.ToString();

        string criticalIntensify = bGrid.BluePrint.CompositeCriticalRate <= 0 ? "" : "<color=#00ffffff> +" + (bGrid.BluePrint.CompositeCriticalRate * 100).ToString() + "</color>";
        CriticalValue.text = (attribute.TurretLevels[0].CriticalRate * 100).ToString() + criticalIntensify + "%";

        string sputteringIntensify = bGrid.BluePrint.CompositeSputteringRange <= 0 ? "" : "<color=#00ffffff> +" + bGrid.BluePrint.CompositeSputteringRange.ToString() + "</color>";
        SputteringValue.text = attribute.TurretLevels[0].SputteringRange.ToString() + sputteringIntensify;

        string slowIntensify = bGrid.BluePrint.CompositeSlowRate <= 0 ? "" : "<color=#00ffffff> +" + bGrid.BluePrint.CompositeSlowRate.ToString() + "</color>";
        SlowRateValue.text = attribute.TurretLevels[0].SlowRate.ToString() + slowIntensify;

        if (attribute.TurretLevels[0].TurretEffects.Count > 0)
        {
            string finalDes = "";
            if (attribute.Description != "")
                finalDes += attribute.Description + "\n";
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
        UpgradeArea.SetActive(false);
    }


    public void BuyBluePrintBtnClick()
    {
        if (LevelUIManager.Instance.ConsumeMoney(StaticData.BuyBluePrintCost))
        {
            LevelUIManager.Instance.LuckyPoints++;
            m_BGrid.MoveToPocket();
        }
        else
        {
            GameEvents.Instance.Message("金币不足");
        }
    }

    public void CompositeBtnClick()
    {
        if (GameManager.Instance.State.StateName != StateName.BuildingState)
        {
            GameEvents.Instance.Message("战斗中不可合成");
            return;
        }
        if (!m_BGrid.BuildAble)
        {
            GameEvents.Instance.Message("缺少必要素材");
            return;
        }
        CloseTips();
        LevelUIManager.Instance.HideArea();
        m_BGrid.Shop.CompositeBluePrint(m_BGrid);
    }

    public void UpgradeBtnClick()
    {
        if (LevelUIManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Turret.Quality++;
            if (m_Turret.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            SetDescription(m_Turret);
            upgradeCost = StaticData.Instance.LevelUpCost[m_Turret.m_TurretAttribute.Rare, m_Turret.Quality - 1];
            UpgradeCostValue.text = upgradeCost.ToString();
        }
    }
    private void FixedUpdate()
    {
        if (m_Turret != null)
            UpdateInfo(m_Turret);
    }

}
