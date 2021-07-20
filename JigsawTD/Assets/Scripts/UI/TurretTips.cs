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

    //加成值
    [SerializeField] Text AttackChangeTxt = default;
    [SerializeField] Text SpeedChangeTxt = default;
    [SerializeField] Text RangeChangeTxt = default;
    [SerializeField] Text CriticalChangeTxt = default;
    [SerializeField] Text SputteringChangeTxt = default;
    [SerializeField] Text SlowRateChangeTxt = default;

    //配方tips
    [SerializeField] GameObject BluePrintArea = default;
    [SerializeField] GameObject BuyBtn = default;

    //地形加成
    [SerializeField] GameObject TileSkillArea = default;
    [SerializeField] Text TileSkillTxt = default;

    //infoBtn
    [SerializeField] InfoBtn CriticalInfo = default;
    [SerializeField] InfoBtn SplashInfo = default;


    private StrategyBase m_Strategy;
    private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    bool showingBlueprint = false;
    int upgradeCost;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        mainCam = Camera.main;
        CriticalInfo.SetContent(GameMultiLang.GetTraduction("CRITICALINFO"));
        SplashInfo.SetContent(GameMultiLang.GetTraduction("SPLASHINFO"));
    }

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
        m_Strategy = null;
        showingBlueprint = false;
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
        TileSkillArea.SetActive(false);
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
                if (((StrategyComposite)Strategy).TileSkill != null)
                {
                    TileSkillArea.SetActive(true);
                    TileSkillTxt.text = ((StrategyComposite)Strategy).TileSkill.SkillDescription;
                }

                break;
        }

    }

    public void ReadBluePrint(BluePrintGrid grid)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, new Vector2(800, Screen.height / 2), myCanvas.worldCamera, out newPos);
        transform.position = myCanvas.transform.TransformPoint(newPos);

        showingBlueprint = true;

        m_Grid = grid;
        m_Strategy = grid.BluePrint.ComStrategy;
        BasicInfo();
        UpdateBluePrintInfo();
        BuyBtn.SetActive(grid.InShop);
        TileSkillArea.SetActive(false);

        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        IntensifyArea.SetActive(false);
        BluePrintArea.SetActive(true);
        elementConstruct.gameObject.SetActive(true);
        elementConstruct.SetElements((StrategyComposite)m_Strategy);

    }

    private void BasicInfo()
    {
        Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretIcon;
        Name.text = GameMultiLang.GetTraduction(m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName);
        string rangeTypeTxt = "";
        switch (m_Strategy.m_Att.RangeType)
        {
            case RangeType.Circle:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE1");
                break;
            case RangeType.HalfCircle:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE2");
                break;
            case RangeType.Line:
                rangeTypeTxt = GameMultiLang.GetTraduction("RANGETYPE3");
                break;
        }
        this.RangeTypeValue.text = rangeTypeTxt;
        //设置描述文案
        Description.text = StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy);
    }

    private void UpdateInfo()
    {
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = "";

        SpeedValue.text = m_Strategy.FinalSpeed.ToString();
        SpeedChangeTxt.text = "";

        RangeValue.text = m_Strategy.FinalRange.ToString();
        RangeChangeTxt.text = "";

        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = "";

        SputteringValue.text = m_Strategy.FinalSputteringRange.ToString();
        SputteringChangeTxt.text = "";

        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = "";

        AnalysisValue.text = m_Strategy.DamageAnalysis.ToString();
    }

    private void UpdateBluePrintInfo()
    {
        StrategyComposite strategy = m_Strategy as StrategyComposite;
        AttackValue.text = strategy.InitAttack.ToString();
        AttackChangeTxt.text = (strategy.BaseAttackIntensify > 0 ?
            "+" + strategy.InitAttack * strategy.BaseAttackIntensify : "");

        SpeedValue.text = strategy.InitSpeed.ToString();
        SpeedChangeTxt.text = (strategy.BaseSpeedIntensify > 0 ?
            "+" + strategy.InitSpeed * strategy.BaseSpeedIntensify : "");

        RangeValue.text = strategy.InitRange.ToString();
        RangeChangeTxt.text = (strategy.BaseRangeIntensify > 0 ?
            "+" + strategy.BaseRangeIntensify : "");

        CriticalValue.text = (strategy.InitCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (strategy.BaseCriticalRateIntensify > 0 ?
            "+" + strategy.BaseCriticalRateIntensify * 100 + "%" : "");

        SputteringValue.text = strategy.InitSputteringRange.ToString();
        SputteringChangeTxt.text = (strategy.BaseSputteringRangeIntensify > 0 ?
            "+" + strategy.BaseSputteringRangeIntensify : "");

        SlowRateValue.text = strategy.InitSlowRate.ToString();
        SlowRateChangeTxt.text = (strategy.BaseSlowRateIntensify > 0 ?
            "+" + strategy.BaseSlowRateIntensify : "");
    }

    public void UpdateLevelUpInfo()
    {
        StrategyComposite strategy = m_Strategy as StrategyComposite;

        float attackIncrease = strategy.NextAttack - strategy.BaseAttack;
        AttackValue.text = strategy.FinalAttack.ToString();
        AttackChangeTxt.text = (attackIncrease > 0 ? "+" + attackIncrease : "");

        float speedIncrease = strategy.NextSpeed - strategy.BaseSpeed;
        SpeedValue.text = strategy.FinalSpeed.ToString();
        SpeedChangeTxt.text = (speedIncrease > 0 ? "+" + speedIncrease : "");

        float criticalIncrease = strategy.NextCriticalRate - strategy.BaseCriticalRate;
        CriticalValue.text = (strategy.FinalCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (criticalIncrease > 0 ? "+" + criticalIncrease * 100 + "%" : "");

        float sputteringIncrease = strategy.NextSputteringRange - strategy.BaseSputteringRange;
        SputteringValue.text = strategy.FinalSputteringRange.ToString();
        SputteringChangeTxt.text = (sputteringIncrease > 0 ? "+" + sputteringIncrease : "");

        float slowRateIncrease = strategy.NextSlowRate - strategy.BaseSlowRate;
        SlowRateValue.text = strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = (slowRateIncrease > 0 ? "+" + slowRateIncrease : "");
    }


    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            m_Strategy.SetQualityValue();
            //m_Strategy.BuildTurretEffects();
            m_Strategy.m_Turret.SetGraphic();
            Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretIcon;
            Name.text = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName;
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
        {
            UpdateInfo();
        }
        if (showingBlueprint)
        {
            UpdateBluePrintInfo();
        }
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
