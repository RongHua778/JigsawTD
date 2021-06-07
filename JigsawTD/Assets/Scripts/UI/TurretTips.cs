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
    [SerializeField] GameObject IntensifyArea = default;//元素塔加成效果区
    [SerializeField] TipsElementConstruct elementConstruct = default;//合成塔组成元素区
    //合成塔升级区

    private TurretContent m_Turret;
    int upgradeCost;

    public override void Hide()
    {
        base.Hide();
        m_Turret = null;
    }

    public void ReadTurret(TurretContent turret)//通过场上防御塔查看
    {
        this.m_Turret = turret;
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;

        //设置攻击范围类型
        SetRangeType(turret.m_TurretAttribute);

        //即时更新攻击，攻速，伤害统计等数据
        UpdateInfo(turret);

        //设置描述文案
        Description.text = StaticData.GetTurretDes(turret.m_TurretAttribute, turret.Quality);

        //根据防御塔类型显示
        switch (turret.ContentType)
        {
            case GameTileContentType.ElementTurret:
                UpgradeArea.SetActive(false);
                IntensifyArea.SetActive(true);
                elementConstruct.gameObject.SetActive(false);
                IntensifyValue.text = StaticData.GetElementIntensifyText(((ElementTurret)turret).Element, turret.Quality);
                break;
            case GameTileContentType.CompositeTurret:
                if (turret.Quality < 3)
                {
                    UpgradeArea.SetActive(true);
                    upgradeCost = StaticData.Instance.LevelUpCost[turret.m_TurretAttribute.Rare - 1, turret.Quality - 1];
                    UpgradeCostValue.text = upgradeCost.ToString();
                }
                else
                {
                    UpgradeArea.SetActive(false);
                }
                IntensifyArea.SetActive(false);
                elementConstruct.gameObject.SetActive(true);
                elementConstruct.SetElements(((CompositeTurret)turret).CompositeBluePrint);
                break;
            default:
                Debug.Log("错误的CONTENT类型");
                break;
        }
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



    private void UpdateInfo(TurretContent turret)
    {
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        CriticalValue.text = (turret.CriticalRate * 100).ToString() + "%";
        SputteringValue.text = turret.SputteringRange.ToString();
        SlowRateValue.text = turret.SlowRate.ToString();
        AnalysisValue.text = turret.DamageAnalysis.ToString();
    }




    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Turret.Quality++;
            Icon.sprite = m_Turret.m_TurretAttribute.TurretLevels[m_Turret.Quality - 1].Icon;
            Name.text = m_Turret.m_TurretAttribute.TurretLevels[m_Turret.Quality - 1].TurretName;
            Description.text = StaticData.GetTurretDes(m_Turret.m_TurretAttribute, m_Turret.Quality);
            if (m_Turret.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            upgradeCost = StaticData.Instance.LevelUpCost[m_Turret.m_TurretAttribute.Rare - 1, m_Turret.Quality - 1];
            UpgradeCostValue.text = upgradeCost.ToString();
        }
    }
    private void FixedUpdate()
    {
        if (m_Turret != null)
            UpdateInfo(m_Turret);
    }

}
