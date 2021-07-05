using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTips : TileTips
{

    Camera mainCam;
    [SerializeField] Canvas myCanvas = default;
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
    [SerializeField] GameObject AnalysisArea = default;//伤害统计区
    [SerializeField] GameObject UpgradeArea = default;//合成塔升级区
    [SerializeField] GameObject IntensifyArea = default;//元素塔加成效果区
    [SerializeField] TipsElementConstruct elementConstruct = default;//合成塔组成元素区
    //合成塔升级区
    //配方tips
    [SerializeField] GameObject BluePrintArea = default;
    [SerializeField] GameObject BuyBtn = default;


    private StrategyBase m_Strategy;
    private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    int upgradeCost;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        mainCam = Camera.main;
    }

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
        m_Strategy = null;
        showingTurret = false;
    }

    public void ReadTurret(StrategyBase Strategy)//通过场上防御塔查看
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, new Vector2(300, Screen.height / 2), myCanvas.worldCamera, out newPos);
        transform.position = myCanvas.transform.TransformPoint(newPos);

        m_Strategy = Strategy;
        BasicInfo();
        UpdateInfo();
        showingTurret = true;
        AnalysisArea.SetActive(true);
        BluePrintArea.SetActive(false);
        //根据防御塔类型显示
        switch (Strategy.strategyType)
        {
            case StrategyType.Element:
                UpgradeArea.SetActive(false);
                IntensifyArea.SetActive(true);
                elementConstruct.gameObject.SetActive(false);
                IntensifyValue.text = StaticData.GetElementIntensifyText(((StrategyElement)Strategy).Element, Strategy.Quality);
                break;
            case StrategyType.Composite:
                if (Strategy.Quality < 3)
                {
                    UpgradeArea.SetActive(true);
                    upgradeCost = StaticData.Instance.LevelUpCost[Strategy.m_Att.Rare - 1, Strategy.Quality - 1];
                    upgradeCost = (int)(upgradeCost * (1 - m_Strategy.UpgradeDiscount));
                    UpgradeCostValue.text = upgradeCost.ToString();
                }
                else
                {
                    UpgradeArea.SetActive(false);
                }
                IntensifyArea.SetActive(false);
                elementConstruct.gameObject.SetActive(true);
                elementConstruct.SetElements((StrategyComposite)Strategy);
                break;
        }

    }

    public void ReadBluePrint(BluePrintGrid grid)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, new Vector2(800, Screen.height / 2), myCanvas.worldCamera, out newPos);
        transform.position = myCanvas.transform.TransformPoint(newPos);

        m_Grid = grid;
        m_Strategy = grid.BluePrint.ComStrategy;
        BasicInfo();
        UpdateBluePrintInfo();
        BuyBtn.SetActive(grid.InShop);
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        IntensifyArea.SetActive(false);
        BluePrintArea.SetActive(true);
        elementConstruct.gameObject.SetActive(true);
        elementConstruct.SetElements((StrategyComposite)m_Strategy);
    }

    private void BasicInfo()
    {
        Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].CannonSprite;
        Name.text = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName;
        string rangeTypeTxt = "";
        switch (m_Strategy.m_Att.RangeType)
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
        //设置描述文案
        Description.text = StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy.Quality);
        //StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy.Quality);
    }

    private void UpdateInfo()
    {
        AttackValue.text = m_Strategy.FinalAttack.ToString("f0");
        SpeedValue.text = m_Strategy.FinalSpeed.ToString("f2");
        RangeValue.text = m_Strategy.FinalRange.ToString();
        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString() + "%";
        SputteringValue.text = m_Strategy.FinalSputteringRange.ToString();
        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        AnalysisValue.text = m_Strategy.DamageAnalysis.ToString();
    }

    private void UpdateBluePrintInfo()
    {
        StrategyComposite strategy = m_Strategy as StrategyComposite;
        AttackValue.text = strategy.FinalAttack.ToString() + (strategy.BaseAttackIntensify > 0 ?
            "<color=cyan>(+" + strategy.InitAttack * strategy.BaseAttackIntensify + ")</color>" : "");
        SpeedValue.text = strategy.FinalSpeed.ToString() + (strategy.BaseSpeedIntensify > 0 ?
            "<color=cyan>(+" + strategy.InitSpeed * strategy.BaseSpeedIntensify + ")</color>" : "");
        RangeValue.text = strategy.FinalRange.ToString() + (strategy.BaseRangeIntensify > 0 ?
            "<color=cyan>(+" + strategy.BaseRangeIntensify + ")</color>" : "");
        CriticalValue.text = (strategy.FinalCriticalRate * 100).ToString() + (strategy.BaseCriticalRateIntensify > 0 ?
            "<color=cyan>(+" + strategy.BaseCriticalRateIntensify * 100 + ")</color>" : "") + "%";
        SputteringValue.text = strategy.FinalSputteringRange.ToString() + (strategy.BaseSputteringRangeIntensify > 0 ?
            "<color=cyan>(+" + strategy.BaseSputteringRangeIntensify + ")</color>" : "");
        SlowRateValue.text = strategy.FinalSlowRate.ToString() + (strategy.BaseSlowRateIntensify > 0 ?
            "<color=cyan>(+" + strategy.BaseSlowRateIntensify + ")</color>" : "");
    }

    public void UpdateLevelUpInfo()
    {
        StrategyComposite strategy = m_Strategy as StrategyComposite;

        float attackIncrease = strategy.NextAttack - strategy.BaseAttack;
        AttackValue.text = strategy.FinalAttack.ToString() + (attackIncrease > 0 ?
            "<color=cyan>(+" + attackIncrease + ")</color>" : "");

        float speedIncrease = strategy.NextSpeed - strategy.BaseSpeed;
        SpeedValue.text = strategy.FinalSpeed.ToString() + (speedIncrease > 0 ?
            "<color=cyan>(+" + speedIncrease + ")</color>" : "");

        float criticalIncrease = strategy.NextCriticalRate - strategy.BaseCriticalRate;
        CriticalValue.text = (strategy.FinalCriticalRate * 100).ToString() + (criticalIncrease > 0 ?
            "<color=cyan>(+" + criticalIncrease * 100 + ")</color>" : "") + "%";

        float sputteringIncrease = strategy.NextSputteringRange - strategy.BaseSputteringRange;
        SputteringValue.text = strategy.FinalSputteringRange.ToString() + (sputteringIncrease > 0 ?
            "<color=cyan>(+" + sputteringIncrease + ")</color>" : "");

        float slowRateIncrease = strategy.NextSlowRate - strategy.BaseSlowRate;
        SlowRateValue.text = strategy.FinalSlowRate.ToString() + (slowRateIncrease > 0 ?
            "<color=cyan>(+" + slowRateIncrease + ")</color>" : "");
    }


    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            m_Strategy.SetQualityValue();
            m_Strategy.BuildTurretEffects();
            m_Strategy.m_Turret.SetGraphic();
            Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].CannonSprite;
            Name.text = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName;
            //Description.text = m_Strategy.m_Att.TurretEffects[0].EffectDescription;

            //StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy.Quality);
            UpdateInfo();
            if (m_Strategy.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            UpdateLevelUpInfo();
            upgradeCost = StaticData.Instance.LevelUpCost[m_Strategy.m_Att.Rare - 1, m_Strategy.Quality - 1];
            upgradeCost = (int)(upgradeCost * (1 - m_Strategy.UpgradeDiscount));
            UpgradeCostValue.text = upgradeCost.ToString();
        }
    }
    private void FixedUpdate()
    {
        if (showingTurret)
            UpdateInfo();
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
