using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurretTips : TileTips
{

    [SerializeField] TMP_Text AttackValue = default;
    [SerializeField] TMP_Text SpeedValue = default;
    [SerializeField] TMP_Text RangeValue = default;

    public void ReadAttribute(Turret turret)
    {
        Icon.sprite = turret.m_TurretAttribute.Icon;
        Name.text = turret.m_TurretAttribute.Name;
        AttackValue.text = turret.AttackDamage.ToString();
        SpeedValue.text = turret.AttackSpeed.ToString();
        RangeValue.text = turret.AttackRange.ToString();
        Description.text = turret.m_TurretAttribute.Description;

        foreach(var slot in LevelSlots)
        {
            slot.SetActive(false);
        }
        for(int i = 0; i <= turret.Level; i++)
        {
            LevelSlots[i].SetActive(true);
        }
    }




}
