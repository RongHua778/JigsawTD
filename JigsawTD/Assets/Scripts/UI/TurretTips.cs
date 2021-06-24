using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTips : TileTips
{

    Camera mainCam;
    [SerializeField] Canvas myCanvas;
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


    private BasicStrategy m_Strategy;
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

    public void ReadTurret(BasicStrategy Strategy)//通过场上防御塔查看
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
                IntensifyValue.text = StaticData.GetElementIntensifyText(((ElementStrategy)Strategy).Element, Strategy.Quality);
                break;
            case StrategyType.Composite:
                if (Strategy.Quality < 3)
                {
                    UpgradeArea.SetActive(true);
                    upgradeCost = StaticData.Instance.LevelUpCost[Strategy.m_Att.Rare - 1, Strategy.Quality - 1];
                    UpgradeCostValue.text = upgradeCost.ToString();
                }
                else
                {
                    UpgradeArea.SetActive(false);
                }
                IntensifyArea.SetActive(false);
                elementConstruct.gameObject.SetActive(true);
                elementConstruct.SetElements(((CompositeStrategy)Strategy).CompositeBluePrint);
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
        elementConstruct.SetElements(grid.BluePrint);
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
    }

    private void UpdateInfo()
    {
        AttackValue.text = m_Strategy.AttackDamage.ToString("f0");
        SpeedValue.text = m_Strategy.AttackSpeed.ToString("f2");
        RangeValue.text = m_Strategy.AttackRange.ToString();
        CriticalValue.text = (m_Strategy.CriticalRate * 100).ToString() + "%";
        SputteringValue.text = m_Strategy.SputteringRange.ToString();
        SlowRateValue.text = m_Strategy.SlowRate.ToString();
        AnalysisValue.text = m_Strategy.DamageAnalysis.ToString();
    }

    private void UpdateBluePrintInfo()
    {
        CompositeStrategy strategy = m_Strategy as CompositeStrategy;
        AttackValue.text = strategy.AttackDamage.ToString() + (strategy.CompositeBluePrint.CompositeAttackDamage > 0 ?
            "<color=cyan>(+" + strategy.m_Att.TurretLevels[0].AttackDamage * strategy.CompositeBluePrint.CompositeAttackDamage + ")</color>" : "");
        SpeedValue.text = strategy.AttackSpeed.ToString() + (strategy.CompositeBluePrint.CompositeAttackSpeed > 0 ?
            "<color=cyan>(+" + strategy.m_Att.TurretLevels[0].AttackSpeed * strategy.CompositeBluePrint.CompositeAttackSpeed + ")</color>" : "");
        RangeValue.text = strategy.AttackRange.ToString();
        CriticalValue.text = (strategy.CriticalRate * 100).ToString() + (strategy.CompositeBluePrint.CompositeCriticalRate > 0 ?
            "<color=cyan>(+" + strategy.CompositeBluePrint.CompositeCriticalRate * 100 + ")</color>" : "") + "%";
        SputteringValue.text = strategy.SputteringRange.ToString() + (strategy.CompositeBluePrint.CompositeSputteringRange > 0 ?
            "<color=cyan>(+" + strategy.CompositeBluePrint.CompositeSputteringRange + ")</color>" : "");
        SlowRateValue.text = strategy.SlowRate.ToString() + (strategy.CompositeBluePrint.CompositeSlowRate > 0 ?
            "<color=cyan>(+" + strategy.CompositeBluePrint.CompositeSlowRate + ")</color>" : "");
    }

    public void UpdateLevelUpInfo()
    {
        CompositeStrategy strategy = m_Strategy as CompositeStrategy;

        float attackIncrease = strategy.m_Att.TurretLevels[strategy.Quality].AttackDamage - strategy.m_Att.TurretLevels[strategy.Quality - 1].AttackDamage;
        AttackValue.text = strategy.AttackDamage.ToString() + (attackIncrease > 0 ?
            "<color=cyan>(+" + attackIncrease + ")</color>" : "");

        float speedIncrease = strategy.m_Att.TurretLevels[strategy.Quality].AttackSpeed - strategy.m_Att.TurretLevels[strategy.Quality - 1].AttackSpeed;
        SpeedValue.text = strategy.AttackSpeed.ToString() + (speedIncrease > 0 ?
            "<color=cyan>(+" + speedIncrease + ")</color>" : "");

        int rangeIncrease = strategy.m_Att.TurretLevels[strategy.Quality].AttackRange - strategy.m_Att.TurretLevels[strategy.Quality - 1].AttackRange;
        RangeValue.text = strategy.AttackRange.ToString() + (rangeIncrease > 0 ?
            "<color=cyan>(+" + rangeIncrease + ")</color>" : "");

        float criticalIncrease = strategy.m_Att.TurretLevels[strategy.Quality].CriticalRate - strategy.m_Att.TurretLevels[strategy.Quality - 1].CriticalRate;
        CriticalValue.text = (strategy.CriticalRate * 100).ToString() + (criticalIncrease > 0 ?
            "<color=cyan>(+" + criticalIncrease * 100 + ")</color>" : "") + "%";

        float sputteringIncrease = strategy.m_Att.TurretLevels[strategy.Quality].SputteringRange - strategy.m_Att.TurretLevels[strategy.Quality - 1].SputteringRange;
        SputteringValue.text = strategy.SputteringRange.ToString() + (sputteringIncrease > 0 ?
            "<color=cyan>(+" + sputteringIncrease + ")</color>" : "");

        float slowRateIncrease= strategy.m_Att.TurretLevels[strategy.Quality].SlowRate - strategy.m_Att.TurretLevels[strategy.Quality - 1].SlowRate;
        SlowRateValue.text = strategy.SlowRate.ToString() + (slowRateIncrease > 0 ?
            "<color=cyan>(+" + slowRateIncrease + ")</color>" : "");
    }


    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            //m_Turret.SetQuality(m_Turret.Quality);
            Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].CannonSprite;
            Name.text = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName;
            Description.text = StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy.Quality);
            UpdateInfo();
            if (m_Strategy.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            UpdateLevelUpInfo();
            upgradeCost = StaticData.Instance.LevelUpCost[m_Strategy.m_Att.Rare - 1, m_Strategy.Quality - 1];
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
