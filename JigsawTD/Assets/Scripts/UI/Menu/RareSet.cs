using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RareSet : SetHolder
{
    BluePrintPanel m_BluePrintSet;
    public int RareIndex;

    public void InitializeSlots(BluePrintPanel blueprintSet)
    {
        SaveList = Game.Instance.SaveData.SaveRareDIC[RareIndex];
        SelectedCount = MaxSelectCount;
        m_BluePrintSet = blueprintSet;
        for (int i = 0; i < turretSlots.Length; i++)
        {
            turretSlots[i].Initialize(this, SaveList[i]);
        }
    }



}
