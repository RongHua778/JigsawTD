using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretTips : TileTips
{

    [SerializeField] TMP_Text AttackValue = default;
    [SerializeField] TMP_Text SpeedValue = default;
    [SerializeField] TMP_Text RangeValue = default;
    [SerializeField] TMP_Text CriticalValue = default;
    [SerializeField] TMP_Text SputteringValue = default;

    public void ReadAttribute(Turret turret)
    {
        Icon.sprite = turret.m_TurretAttribute.TurretLevels[turret.Quality-1].Icon;
        Name.text = turret.m_TurretAttribute.TurretLevels[turret.Quality-1].TurretName;
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        CriticalValue.text = turret.CriticalRate.ToString();
        SputteringValue.text = turret.SputteringRange.ToString();
        Description.text = turret.m_TurretAttribute.Description;

        //foreach (var slot in LevelSlots)
        //{
        //    slot.SetActive(false);
        //}
        //for (int i = 0; i <= turret.Level; i++)
        //{
        //    LevelSlots[i].SetActive(true);
        //}
    }




}
