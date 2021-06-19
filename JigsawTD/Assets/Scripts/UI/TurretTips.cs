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
    [SerializeField] GameObject UpgradeArea = default;//�ϳ���������
    [SerializeField] GameObject IntensifyArea = default;//Ԫ�����ӳ�Ч����
    [SerializeField] TipsElementConstruct elementConstruct = default;//�ϳ������Ԫ����
    //�ϳ���������

    private BasicStrategy m_Strategy;
    int upgradeCost;

    public override void Hide()
    {
        base.Hide();
        m_Strategy = null;
    }

    public void ReadTurret(BasicStrategy Strategy)//ͨ�����Ϸ������鿴
    {
        BasicInfo(Strategy);

        //��ʱ���¹��������٣��˺�ͳ�Ƶ�����
        UpdateInfo();

        //���ݷ�����������ʾ
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
            default:
                Debug.Log("�����CONTENT����");
                break;
        }

    }

    public void ReadBluePrint(BasicStrategy strategy)
    {
        TurretAttribute attribute = strategy.m_Att;
        BasicInfo(strategy);
        UpdateBluePrintInfo();

    }

    private void BasicInfo(BasicStrategy Strategy)
    {
        this.m_Strategy = Strategy;
        Icon.sprite = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].Icon;
        Name.text = Strategy.m_Att.TurretLevels[Strategy.Quality - 1].TurretName;
        string rangeTypeTxt = "";
        switch (Strategy.m_Att.RangeType)
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
        //���������İ�
        Description.text = StaticData.GetTurretDes(Strategy.m_Att, Strategy.Quality);
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

    }

    private void UpdateLevelUpInfo()
    {

    }


    public void UpgradeBtnClick()
    {
        if (GameManager.Instance.ConsumeMoney(upgradeCost))
        {
            m_Strategy.Quality++;
            //m_Turret.SetQuality(m_Turret.Quality);
            Icon.sprite = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].Icon;
            Name.text = m_Strategy.m_Att.TurretLevels[m_Strategy.Quality - 1].TurretName;
            Description.text = StaticData.GetTurretDes(m_Strategy.m_Att, m_Strategy.Quality);
            if (m_Strategy.Quality > 2)
            {
                UpgradeArea.SetActive(false);
                return;
            }
            upgradeCost = StaticData.Instance.LevelUpCost[m_Strategy.m_Att.Rare - 1, m_Strategy.Quality - 1];
            UpgradeCostValue.text = upgradeCost.ToString();
        }
    }
    private void FixedUpdate()
    {
        if (m_Strategy != null)
            UpdateInfo();
    }

}
