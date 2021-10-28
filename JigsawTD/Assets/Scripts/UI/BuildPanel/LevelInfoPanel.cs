using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanel : IUserInterface
{
    [SerializeField] QualitySlot[] qualitySlots = default;
    public void SetInfo()
    {
        for (int i = 0; i < qualitySlots.Length; i++)
        {
            float chanceNow = StaticData.QualityChances[GameRes.ModuleLevel - 1, i];
            float chanceAfter = chanceNow;
            if (GameRes.ModuleLevel < StaticData.Instance.PlayerMaxLevel)
                chanceAfter = StaticData.QualityChances[GameRes.ModuleLevel, i];
            qualitySlots[i].SetSlotInfo(i + 1, chanceNow, chanceAfter);
        }
    }
}
