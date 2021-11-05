using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TurretTips : TileTips
{

    //Camera mainCam;
    //[SerializeField] Canvas myCanvas = default;
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
    [SerializeField] GameObject AnalysisArea = default;//�˺�ͳ����
    [SerializeField] GameObject UpgradeArea = default;//�ϳ���������
    [SerializeField] GameObject IntensifyArea = default;//Ԫ�����ӳ�Ч����

    [SerializeField] GameObject ElementSkillArea = default;//Ԫ�ؼ�����
    [SerializeField] Transform ElementSkillContent = default;//Ԫ�ؼ��ܻ�����
    [SerializeField] TipsElementConstruct ElementConstructPrefab = default;
    List<TipsElementConstruct> elementConstructs = new List<TipsElementConstruct>();//�ϳ������Ԫ����
    //[SerializeField] TipsElementConstruct elementConstruct2 = default;//�ڶ�Ԫ�ؼ�����
    //�ϳ���������

    //�ӳ�ֵ
    [SerializeField] Text AttackChangeTxt = default;
    [SerializeField] Text SpeedChangeTxt = default;
    [SerializeField] Text RangeChangeTxt = default;
    [SerializeField] Text CriticalChangeTxt = default;
    [SerializeField] Text SputteringChangeTxt = default;
    [SerializeField] Text SlowRateChangeTxt = default;

    //�䷽tips
    [SerializeField] GameObject BluePrintArea = default;
    [SerializeField] GameObject BuyBtn = default;

    ////���μӳ�
    //[SerializeField] GameObject ElementSkill2Area = default;
    //[SerializeField] Text ElementSkill2Txt = default;

    [SerializeField] TurretInfoSetter QualitySetter = default;
    //infoBtn
    [SerializeField] InfoBtn CriticalInfo = default;
    [SerializeField] InfoBtn SplashInfo = default;
    [SerializeField] InfoBtn SlowInfo = default;


    private StrategyBase m_Strategy;
    private BluePrintGrid m_Grid;
    public bool showingTurret = false;
    bool showingBlueprint = false;
    int upgradeCost;

    public override void Initialize()
    {
        //mainCam = Camera.main;
        CriticalInfo.SetContent(GameMultiLang.GetTraduction("CRITICALINFO"));
        SplashInfo.SetContent(GameMultiLang.GetTraduction("SPLASHINFO"));
        SlowInfo.SetContent(GameMultiLang.GetTraduction("SLOWINFO"));
    }

    public override void Hide()
    {
        base.Hide();
        m_Grid = null;
        m_Strategy = null;
        showingBlueprint = false;
        showingTurret = false;
    }

    public void ReadTurret(StrategyBase Strategy)//ͨ�����Ϸ������鿴
    {
        //Vector2 newPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, pos, myCanvas.worldCamera, out newPos);//new Vector2(300, Screen.height / 2)
        //transform.position = myCanvas.transform.TransformPoint(newPos);

        m_Strategy = Strategy;
        BasicInfo(m_Strategy.m_Att, m_Strategy.Quality);
        UpdateRealTimeValue();
        showingTurret = true;

        AnalysisArea.SetActive(true);
        BluePrintArea.SetActive(false);
        //���ݷ�����������ʾ
        switch (Strategy.m_Att.StrategyType)
        {
            case StrategyType.Element:
                QualitySetter.gameObject.SetActive(false);
                UpgradeArea.SetActive(false);
                IntensifyArea.SetActive(true);
                ElementSkillArea.SetActive(false);
                IntensifyValue.text = StaticData.GetElementIntensifyText(Strategy.m_Att.element);
                break;
            case StrategyType.Composite:
                ElementSkillArea.SetActive(true);
                UpgradeAreaSet(Strategy);
                IntensifyArea.SetActive(false);

                //Ԫ�ؼ���
                SetElementSkill();

                break;
        }

    }

    public void ReadBluePrint(BluePrintGrid grid)
    {
        //Vector2 newPos;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, new Vector2(900, Screen.height / 2), myCanvas.worldCamera, out newPos);
        //transform.position = myCanvas.transform.TransformPoint(newPos);

        showingBlueprint = true;
        m_Grid = grid;
        m_Strategy = grid.BluePrint.ComStrategy;

        BasicInfo(m_Strategy.m_Att, m_Strategy.Quality);
        UpdateBluePrintInfo();

        ElementSkillArea.SetActive(true);
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        IntensifyArea.SetActive(false);
        BluePrintArea.SetActive(true);
        SetElementSkill();
    }//ͨ���䷽�鿴

    public void ReadAttribute(TurretAttribute att)//ͨ��Attribute�鿴
    {
        int quality = att.StrategyType == StrategyType.Element ? 5 : 3;
        BasicInfo(att, quality);//����״̬
        AnalysisArea.SetActive(false);
        UpgradeArea.SetActive(false);
        ElementSkillArea.SetActive(false);
        switch (att.StrategyType)
        {
            case StrategyType.Element:
                QualitySetter.gameObject.SetActive(false);
                IntensifyArea.SetActive(true);
                IntensifyValue.text = StaticData.GetElementIntensifyText(att.element);
                break;
            case StrategyType.Composite:
                QualitySetter.gameObject.SetActive(true);
                QualitySetter.SetRare(att.Rare);
                IntensifyArea.SetActive(false);
                break;
        }

        UpdatePreviewValue(att, quality - 1);
    }

    private void UpgradeAreaSet(StrategyBase Strategy)
    {
        if (Strategy.Quality < 3)
        {
            UpgradeArea.SetActive(true);
            upgradeCost = StaticData.Instance.LevelUpCostPerRare * m_Strategy.m_Att.Rare * m_Strategy.Quality;
            upgradeCost = (int)(upgradeCost * (1 - m_Strategy.UpgradeDiscount));
            UpgradeCostValue.text = upgradeCost.ToString();
            //UpdateLevelUpInfo();
        }
        else
        {
            UpgradeArea.SetActive(false);
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
            if (i > elementConstructs.Count - 1)
            {
                TipsElementConstruct construct = Instantiate(ElementConstructPrefab, ElementSkillContent);
                elementConstructs.Add(construct);
            }
            elementConstructs[i].gameObject.SetActive(true);
            if (i < m_Strategy.TurretSkills.Count - 1)
                elementConstructs[i].SetElements((ElementSkill)m_Strategy.TurretSkills[i + 1]);//��һ���Ǳ�������
            else
                elementConstructs[i].SetEmpty();
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
                break;
            case StrategyType.Composite:
                Name.text = GameMultiLang.GetTraduction(att.Name);
                QualitySetter.gameObject.SetActive(true);
                QualitySetter.SetRare(att.Rare);
                QualitySetter.SetLevel(quality);
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
        //���������İ�
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

        SputteringValue.text = att.TurretLevels[quality].SputteringRange.ToString();
        SputteringChangeTxt.text = "";

        SlowRateValue.text = att.TurretLevels[quality].SlowRate.ToString();
        SlowRateChangeTxt.text = "";
    }

    private void UpdateRealTimeValue()
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

        AnalysisValue.text = m_Strategy.TurnDamage.ToString();
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
            BasicInfo(m_Strategy.m_Att, m_Strategy.Quality);
            UpdateRealTimeValue();
            UpgradeAreaSet(m_Strategy);

        }
    }
    private void FixedUpdate()
    {
        if (showingTurret)
        {
            UpdateRealTimeValue();
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
