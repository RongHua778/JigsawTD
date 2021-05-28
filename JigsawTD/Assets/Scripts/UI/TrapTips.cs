using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTips : TileTips
{
    public void ReadAttribute(TrapTile trapTile)
    {
        //Icon.sprite = trapTile.m_TrapAttribute.Icon;
        //Name.text = trapTile.m_TrapAttribute.Name;
        Description.text = trapTile.m_TrapAttribute.Description;

        foreach (var slot in LevelSlots)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i <= trapTile.TrapLevel; i++)
        {
            LevelSlots[i].SetActive(true);
        }
    }
}
