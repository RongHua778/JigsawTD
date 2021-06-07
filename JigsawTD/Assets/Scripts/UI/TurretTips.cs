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

    private TurretContent m_Turret;
    int upgradeCost;

    public override void Hide()
    {
        base.Hide();
        m_Turret = null;
    }

    public void ReadTurret(TurretContent turret)//ͨ�����Ϸ������鿴
    {
        this.m_Turret = turret;
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality - 1].TurretName;

        //���ù�����Χ����
        SetRangeType(turret.m_TurretAttribute);

        //��ʱ���¹��������٣��˺�ͳ�Ƶ�����
        UpdateInfo(turret);

        //���������İ�
        Description.text = StaticData.GetTurretDes(turret.m_TurretAttribute, turret.Quality);

        //���ݷ�����������ʾ
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
                Debug.Log("�����CONTENT����");
                break;
        }
    }

    private void SetRangeType(TurretAttribute attribute)
    {
        string rangeTypeTxt = "";
        switch (attribute.RangeType)
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
