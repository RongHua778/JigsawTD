using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class TurretTips : TileTips
{

    [SerializeField] Text RangeTypeValue = default;
    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text SplashRangeValue = default;
    [SerializeField] Text SlowRateValue = default;
    [SerializeField] Text IntensifyValue = default;
    [SerializeField] Text AnalysisValue = default;
    [SerializeField] TextMeshProUGUI UpgradeCostValue = default;

    [SerializeField] TextMeshProUGUI defaultSkillTxt = default;
    [SerializeField] GameObject AnalysisArea = default;//伤害统计区
    [SerializeField] GameObject UpgradeArea = default;//合成塔升级区
    [SerializeField] Image IntensifyIcon = default;
    [SerializeField] GameObject IntensifyArea = default;//元素塔加成效果区

    [SerializeField] ElementHolder ElementsHolder = default;//元素数量
    [SerializeField] GameObject ElementSkillArea = default;//元素技能区
    //[SerializeField] Transform ElementSkillContent = default;//元素技能滑动区
    //[SerializeField] TipsElementConstruct ElementConstructPrefab = default;
    [SerializeField] List<TipsElementConstruct> elementConstructs;//合成塔组成元素区
    //[SerializeField] TipsElementConstruct elementConstruct2 = default;//第二元素技能区
    //合成塔升级区

    //加成值
    [SerializeField] Text AttackChangeTxt = default;
    [SerializeField] Text SpeedChangeTxt = default;
    [SerializeField] Text RangeChangeTxt = default;
    [SerializeField] Text CriticalChangeTxt = default;
    [SerializeField] Text SplashChangeTxt = default;
    [SerializeField] Text SlowRateChangeTxt = default;

    //配方tips
    [SerializeField] GameObject BluePrintArea = default;

    [SerializeField] TurretInfoSetter QualitySetter = default;
    //infoBtn
    [SerializeField] InfoBtn CriticalInfo = default;
    [SerializeField] InfoBtn SplashInfo = default;
    [SerializeField] InfoBtn SlowInfo = default;


    private StrategyBase m_Strategy;
    private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    //public bool showingBlueprint = false;
    int upgradeCost;

    [SerializeField] Animator TileInfo_Anim = default;

    public override void Initialize()
    {
        base.Initialize();
        //mainCam = Camera.main;
        CriticalInfo.SetContent(GameMultiLang.GetTraduction("CRITICALINFO"));
        SplashInfo.SetContent(GameMultiLang.GetTraduction("SPLASHINFO"));
        SlowInfo.SetContent(GameMultiLang.GetTraduction("SLOWINFO"));
    }

    public override void Show()
    {
        base.Show();
        TileInfo_Anim.SetTrigger("Show");
    }

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
        m_Strategy = null;
        showingTurret = false;
    }

    public override void CloseTips()
    {
        base.CloseTips();
        if (BluePrintGrid.SelectingBluePrint != null)
        {
            BluePrintGrid.SelectingBluePrint.OnBluePrintDeselect();
        }
    }

    public void ReadTurret(StrategyBase Strategy)//通过场上防御塔查看
    {
        m_Strategy = Strategy;
        BasicInfo(m_Strategy.Attribute, m_Strategy.Quality);
        UpdateRealTimeValue();
        showingTurret = true;

        AnalysisArea.SetActive(true);
        BluePrintArea.SetActive(false);
        //根据防御塔类型显示
        switch (Strategy.Attribute.StrategyType)
        {
            case StrategyType.Element:
                QualitySetter.gameObject.SetActive(false);
                UpgradeArea.SetActive(false);
                IntensifyIcon.sprite = StaticData.Instance.ElementSprites[(int)Strategy.Attribute.element];
                IntensifyArea.SetActive(true);
                ElementSkillArea.SetActive(false);
                IntensifyValue.text = StaticData.ElementDIC[Strategy.Attribute.element].GetIntensifyText(1);
                break;
            case StrategyType.Composite:
                ElementSkillArea.SetActive(true);
                UpgradeArea.SetActive(true);
                UpgradeAreaSet(Strategy);
                IntensifyArea.SetActive(false);

                //元素技能
                SetElementSkill();

                break;
        }

    }

    public void ReadBluePrint(BluePrintGrid grid)
    {
        //showingBlueprint = true;
        m_Grid = grid;
        m_Strategy = grid.Strategy;

        BasicInfo(m_Strategy.Attribute, m_Strategy.Quality);
        UpdateBluePrintInfo();


        ElementSkillArea.SetActive(true);
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        IntensifyArea.SetActive(false);
        BluePrintArea.SetActive(true);
        SetElementSkill();
    }//通过配方查看

    public void ReadAttribute(TurretAttribute att)//通过Attribute查看
    {
        int quality = att.StrategyType == StrategyType.Element ? 5 : 3;
        BasicInfo(att, quality);//满级状态
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        ElementSkillArea.SetActive(false);
        switch (att.StrategyType)
        {
            case StrategyType.Element:
                QualitySetter.gameObject.SetActive(false);
                IntensifyIcon.sprite = StaticData.Instance.ElementSprites[(int)att.element];
                IntensifyArea.SetActive(true);
                IntensifyValue.text = StaticData.ElementDIC[att.element].GetIntensifyText(1);
                break;
            case StrategyType.Composite:
                QualitySetter.gameObject.SetActive(true);
                //QualitySetter.SetRare(att.Rare);
                IntensifyArea.SetActive(false);
                break;
        }

        UpdatePreviewValue(att, quality - 1);
    }

    private void UpgradeAreaSet(StrategyBase Strategy)
    {
        if (Strategy.Quality < 3)
        {
            upgradeCost = StaticData.Instance.LevelUpCostPerRare[m_Strategy.Attribute.Rare - 1, m_Strategy.Quality - 1];// * m_Strategy.m_Att.Rare * m_Strategy.Quality;
            upgradeCost = (int)(upgradeCost * (1 - m_Strategy.UpgradeDiscount));
            UpgradeCostValue.text = GameMultiLang.GetTraduction("UPGRADE") + "<sprite=7>" + upgradeCost.ToString();
        }
        else
        {
            UpgradeCostValue.text = "MAX";
        }
    }
    private void SetElementSkill()
    {
        foreach (var ecom in elementConstructs)
        {
            ecom.gameObject.SetActive(false);
        }
        for (int i = 0; i < m_Strategy.ElementSKillSlot; i++)
        {
            elementConstructs[i].gameObject.SetActive(true);
            if (i < m_Strategy.TurretSkills.Count - 1)
                elementConstructs[i].SetElements((ElementSkill)m_Strategy.TurretSkills[i + 1]);//第一个是被动技能
            else
                elementConstructs[i].SetEmpty();
        }
    }

    private void UpdateSkillValues()
    {
        for (int i = 0; i < m_Strategy.ElementSKillSlot; i++)
        {
            elementConstructs[i].UpdateDes();
        }
    }

    private void BasicInfo(TurretAttribute att, int quality)
    {
        Icon.sprite = att.TurretLevels[quality - 1].TurretIcon;

        switch (att.StrategyType)
        {
            case StrategyType.Element:
                string element = StaticData.FormElementName(att.element, quality);
                Name.text = element + " " + GameMultiLang.GetTraduction(att.Name);
                QualitySetter.gameObject.SetActive(false);
                defaultSkillTxt.text = GameMultiLang.GetTraduction("DESCRIPTION");
                break;
            case StrategyType.Composite:
                Name.text = GameMultiLang.GetTraduction(att.Name);
                QualitySetter.gameObject.SetActive(true);
                //QualitySetter.SetRare(att.Rare);
                QualitySetter.SetLevel(quality);

                defaultSkillTxt.text = GameMultiLang.GetTraduction(att.Name + "SN");
                break;
        }

        string rangeTypeTxt = "";
        switch (att.RangeType)
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
        Description.text = StaticData.GetTurretDes(att);
    }

    private void UpdatePreviewValue(TurretAttribute att, int quality)
    {
        AttackValue.text = att.TurretLevels[quality].AttackDamage.ToString();
        AttackChangeTxt.text = "";

        SpeedValue.text = att.TurretLevels[quality].AttackSpeed.ToString();
        SpeedChangeTxt.text = "";

        RangeValue.text = att.TurretLevels[quality].AttackRange.ToString();
        RangeChangeTxt.text = "";

        CriticalValue.text = (att.TurretLevels[quality].CriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = "";

        SplashRangeValue.text = att.TurretLevels[quality].SplashRange.ToString();
        SplashChangeTxt.text = "";

        SlowRateValue.text = att.TurretLevels[quality].SlowRate.ToString();
        SlowRateChangeTxt.text = "";
    }

    private void UpdateRealTimeValue()
    {
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = "";

        SpeedValue.text = m_Strategy.FinalFireRate.ToString();
        SpeedChangeTxt.text = "";

        RangeValue.text = m_Strategy.FinalRange.ToString();
        RangeChangeTxt.text = "";

        CriticalValue.text = (Mathf.RoundToInt(m_Strategy.FinalCriticalRate * 100)).ToString() + "%";
        CriticalChangeTxt.text = "";

        SplashRangeValue.text = m_Strategy.FinalSplashRange.ToString();
        SplashChangeTxt.text = "";

        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = "";

        AnalysisValue.text = m_Strategy.TurnDamage.ToString();

        ElementsHolder.SetElementCount(m_Strategy);
    }

    private void UpdateBluePrintInfo()
    {
        AttackValue.text = m_Strategy.InitAttack.ToString();
        AttackChangeTxt.text = (m_Strategy.FinalAttack > m_Strategy.InitAttack ?
            "+" + (m_Strategy.FinalAttack - m_Strategy.InitAttack) : "");

        SpeedValue.text = m_Strategy.InitFireRate.ToString();
        SpeedChangeTxt.text = (m_Strategy.FinalFireRate > m_Strategy.InitFireRate ?
            "+" + (m_Strategy.FinalFireRate - m_Strategy.InitFireRate) : "");

        RangeValue.text = m_Strategy.InitRange.ToString();
        RangeChangeTxt.text = (m_Strategy.FinalRange > m_Strategy.InitRange ?
            "+" + (m_Strategy.FinalRange - m_Strategy.InitRange) : "");

        CriticalValue.text = (m_Strategy.InitCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (m_Strategy.FinalCriticalRate > m_Strategy.InitCriticalRate ?
            "+" + (m_Strategy.FinalCriticalRate - m_Strategy.InitCriticalRate) * 100 + "%" : "");

        SplashRangeValue.text = m_Strategy.InitSplashRange.ToString();
        SplashChangeTxt.text = (m_Strategy.FinalSplashRange > m_Strategy.InitSplashRange ?
            "+" + (m_Strategy.FinalSplashRange - m_Strategy.InitSplashRange) : "");

        SlowRateValue.text = m_Strategy.InitSlowRate.ToString();
        SlowRateChangeTxt.text = (m_Strategy.FinalSlowRate > m_Strategy.InitSlowRate ?
            "+" + (m_Strategy.FinalSlowRate - m_Strategy.InitSlowRate) : "");

        ElementsHolder.SetElementCount(m_Strategy);
    }

    public void UpdateLevelUpInfo()
    {
        if (m_Strategy.Quality >= 3)//满级不预览
            return;
        float attackIncrease = m_Strategy.NextAttack - m_Strategy.BaseAttack;
        AttackValue.text = m_Strategy.FinalAttack.ToString();
        AttackChangeTxt.text = (attackIncrease > 0 ? "+" + attackIncrease : "");

        float speedIncrease = m_Strategy.NextSpeed - m_Strategy.BaseSpeed;
        SpeedValue.text = m_Strategy.FinalFireRate.ToString();
        SpeedChangeTxt.text = (speedIncrease > 0 ? "+" + speedIncrease : "");

        float criticalIncrease = m_Strategy.NextCriticalRate - m_Strategy.BaseCriticalRate;
        CriticalValue.text = (m_Strategy.FinalCriticalRate * 100).ToString() + "%";
        CriticalChangeTxt.text = (criticalIncrease > 0 ? "+" + criticalIncrease * 100 + "%" : "");

        float sputteringIncrease = m_Strategy.NextSplashRange - m_Strategy.BaseSplashRange;
        SplashRangeValue.text = m_Strategy.FinalSplashRange.ToString();
        SplashChangeTxt.text = (sputteringIncrease > 0 ? "+" + sputteringIncrease : "");

        float slowRateIncrease = m_Strategy.NextSlowRate - m_Strategy.BaseSlowRate;
        SlowRateValue.text = m_Strategy.FinalSlowRate.ToString();
        SlowRateChangeTxt.text = (slowRateIncrease > 0 ? "+" + slowRateIncrease : "");
    }


    public void UpgradeBtnClick()
    {
        if (m_Strategy.Quality < 3 && GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            m_Strategy.SetQualityValue();
            m_Strategy.Turret.SetGraphic();
            m_Strategy.Turret.m_ContentStruct.Quality = m_Strategy.Quality;
            BasicInfo(m_Strategy.Attribute, m_Strategy.Quality);
            UpdateRealTimeValue();
            UpgradeAreaSet(m_Strategy);

            ((RefactorTurret)(m_Strategy.Turret)).ShowLandedEffect();

        }
    }
    private void FixedUpdate()
    {
        if (showingTurret)
        {
            UpdateRealTimeValue();
            UpdateSkillValues();
        }
        //if (showingBlueprint)
        //{
        //    UpdateBluePrintInfo();
        //}


    }


    public void CompositeBtnClick()
    {
        GameManager.Instance.CompositeShape(m_Grid);
    }
}
