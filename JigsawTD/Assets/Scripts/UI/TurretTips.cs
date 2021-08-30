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

    [SerializeField] GameObject ElementSkillArea = default;//元素技能区
    [SerializeField] TipsElementConstruct[] elementConstruct = default;//合成塔组成元素区
    //[SerializeField] TipsElementConstruct elementConstruct2 = default;//第二元素技能区
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

    ////地形加成
    //[SerializeField] GameObject ElementSkill2Area = default;
    //[SerializeField] Text ElementSkill2Txt = default;

    [SerializeField] RareInfoSetter QualitySetter = default;
    //infoBtn
    [SerializeField] InfoBtn CriticalInfo = default;
    [SerializeField] InfoBtn SplashInfo = default;


    private StrategyBase m_Strategy;
    private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    bool showingBlueprint = false;
    int upgradeCost;

    public override void Initialize()
    {
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
        //根据防御塔类型显示
        switch (Strategy.strategyType)
        {
            case StrategyType.Element:
                QualitySetter.gameObject.SetActive(false);
                UpgradeArea.SetActive(false);
                IntensifyArea.SetActive(true);
                ElementSkillArea.SetActive(false);
                IntensifyValue.text = StaticData.GetElementIntensifyText(Strategy.Element, Strategy.Quality);
                break;
            case StrategyType.Composite:
                ElementSkillArea.SetActive(true);
                if (Strategy.Quality < 3)
                {
                    UpgradeArea.SetActive(true);
                    //upgradeCost = StaticData.Instance.LevelUpCost[Strategy.m_Att.Rare - 1, Strategy.Quality - 1];
                    upgradeCost = StaticData.Instance.LevelUpCostPerRare * m_Strategy.m_Att.Rare * m_Strategy.Quality;
                    upgradeCost = (int)(upgradeCost * (1 - m_Strategy.UpgradeDiscount));
                    UpgradeCostValue.text = upgradeCost.ToString();
                }
                else
                {
                    UpgradeArea.SetActive(false);
                }
                IntensifyArea.SetActive(false);

                //元素技能
                SetElementSkill();

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

        ElementSkillArea.SetActive(true);
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        IntensifyArea.SetActive(false);
        BluePrintArea.SetActive(true);
        SetElementSkill();
    }

    private void SetElementSkill()
    {
        foreach (var ecom in elementConstruct)
        {
            ecom.gameObject.SetActive(false);
        }
        for (int i = 0; i < m_Strategy.ElementSKillSlot; i++)
        {
            elementConstruct[i].gameObject.SetActive(true);
            if (i < m_Strategy.TurretSkills.Count - 1)
                elementConstruct[i].SetElements((ElementSkill)m_Strategy.TurretSkills[i + 1]);//第一个是被动技能
            else
                elementConstruct[i].SetEmpty();
        }
    }

    private void BasicInfo()
    {
        Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretIcon;

        switch (m_Strategy.strategyType)
        {
            case StrategyType.Element:
                string element = "";
                switch (m_Strategy.Element)
                {
                    case Element.Gold:
                        element = "A";
                        break;
                    case Element.Wood:
                        element = "B";
                        break;
                    case Element.Water:
                        element = "C";
                        break;
                    case Element.Fire:
                        element = "D";
                        break;
                    case Element.Dust:
                        element = "E";
                        break;
                }
                element += m_Strategy.Quality;
                Name.text = element + " " + GameMultiLang.GetTraduction(m_Strategy.m_Att.Name);
                QualitySetter.gameObject.SetActive(false);
                break;
            case StrategyType.Composite:
                Name.text = GameMultiLang.GetTraduction(m_Strategy.m_Att.Name) + " Lv." + m_Strategy.Quality;
                QualitySetter.gameObject.SetActive(true);
                QualitySetter.SetRare(m_Strategy.m_Att.Rare);
                break;
        }

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
        AttackValue.text = m_Strategy.InitAttack.ToString();
        AttackChangeTxt.text = (m_Strategy.BaseAttackIntensify > 0 ?
            "+" + m_Strategy.InitAttack * m_Strategy.BaseAttackIntensify : "");

        SpeedValue.text = m_Strategy.InitSpeed.ToString();
        SpeedChangeTxt.text = (m_Strategy.BaseSpeedIntensify > 0 ?
            "+" + m_Strategy.InitSpeed * m_Strategy.BaseSpeedIntensify : "");

        RangeValue.text = m_Strategy.InitRange.ToString();
        RangeChangeTxt.text = (m_Strategy.BaseRangeIntensify > 0 ?
            "+" + m_Strategy.BaseRangeIntensify : "");

        CriticalValue.text = (m_Strategy.InitCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (m_Strategy.BaseCriticalRateIntensify > 0 ?
            "+" + m_Strategy.BaseCriticalRateIntensify * 100 + "%" : "");

        SputteringValue.text = m_Strategy.InitSputteringRange.ToString();
        SputteringChangeTxt.text = (m_Strategy.BaseSputteringRangeIntensify > 0 ?
            "+" + m_Strategy.BaseSputteringRangeIntensify : "");

        SlowRateValue.text = m_Strategy.InitSlowRate.ToString();
        SlowRateChangeTxt.text = (m_Strategy.BaseSlowRateIntensify > 0 ?
            "+" + m_Strategy.BaseSlowRateIntensify : "");
    }

    public void UpdateLevelUpInfo()
    {

        float attackIncrease = m_Strategy.NextAttack - m_Strategy.BaseAttack;
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = (attackIncrease > 0 ? "+" + attackIncrease : "");

        float speedIncrease = m_Strategy.NextSpeed - m_Strategy.BaseSpeed;
        SpeedValue.text = m_Strategy.FinalSpeed.ToString();
        SpeedChangeTxt.text = (speedIncrease > 0 ? "+" + speedIncrease : "");

        float criticalIncrease = m_Strategy.NextCriticalRate - m_Strategy.BaseCriticalRate;
        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (criticalIncrease > 0 ? "+" + criticalIncrease * 100 + "%" : "");

        float sputteringIncrease = m_Strategy.NextSputteringRange - m_Strategy.BaseSputteringRange;
        SputteringValue.text = m_Strategy.FinalSputteringRange.ToString();
        SputteringChangeTxt.text = (sputteringIncrease > 0 ? "+" + sputteringIncrease : "");

        float slowRateIncrease = m_Strategy.NextSlowRate - m_Strategy.BaseSlowRate;
        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = (slowRateIncrease > 0 ? "+" + slowRateIncrease : "");
    }


    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            m_Strategy.SetQualityValue();
            m_Strategy.m_Turret.SetGraphic();
            BasicInfo();
            UpdateInfo();
            if (m_Strategy.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            UpdateLevelUpInfo();
            upgradeCost = StaticData.Instance.LevelUpCostPerRare * m_Strategy.m_Att.Rare * m_Strategy.Quality;
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
        GameManager.Instance.BuyBluePrint(m_Grid, StaticData.Instance.BuyBluePrintCost);
        BuyBtn.SetActive(m_Grid.InShop);
    }

    public void CompositeBtnClick()
    {
        GameManager.Instance.CompositeShape(m_Grid);
    }
}
