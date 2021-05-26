using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTips : TileTips
{

    [SerializeField] Text AttackValue = default;
    [SerializeField] Text SpeedValue = default;
    [SerializeField] Text RangeValue = default;
    [SerializeField] Text CriticalValue = default;
    [SerializeField] Text SputteringValue = default;
    [SerializeField] GameObject BtnArea = default;
    [SerializeField] GameObject IntensifyArea = default;
    public void ReadTurret(Turret turret)
    {
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality-1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality-1].TurretName;
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        CriticalValue.text = turret.CriticalRate.ToString();
        SputteringValue.text = turret.SputteringRange.ToString();
        Description.text = turret.m_TurretAttribute.Description;
        BtnArea.SetActive(false);
        IntensifyArea.SetActive(true);
    }

    public void ReadAttribute(TurretAttribute attribute,bool isCompositing)
    {
        Icon.sprite = attribute.TurretLevels[0].Icon;
        Name.text = attribute.TurretLevels[0].TurretName;
        AttackValue.text = attribute.TurretLevels[0].AttackDamage.ToString();
        SpeedValue.text = attribute.TurretLevels[0].AttackSpeed.ToString();
        RangeValue.text = attribute.TurretLevels[0].AttackRange.ToString();
        CriticalValue.text = attribute.TurretLevels[0].CriticalRate.ToString();
        SputteringValue.text = attribute.TurretLevels[0].SputteringRange.ToString();
        Description.text = attribute.Description;
        IntensifyArea.SetActive(false);
        if (isCompositing)
        {
            BtnArea.SetActive(true);
        }
        else
        {
            BtnArea.SetActive(false);
        }
    }

    public void CloseTips()
    {
        this.gameObject.SetActive(false);
    }



}
