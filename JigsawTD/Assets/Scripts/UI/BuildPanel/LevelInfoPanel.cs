using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanel : IUserInterface
{
    [SerializeField] QualitySlot[] qualitySlots = default;
    public override void Initialize()
    {
        base.Initialize();
        SetInfo();
    }
    public void SetInfo()
    {
        for (int i = 0; i < qualitySlots.Length; i++)
        {
            float chanceNow = StaticData.QualityChances[GameRes.SystemLevel - 1, i];
            float chanceAfter = chanceNow;
            if (GameRes.SystemLevel < StaticData.Instance.SystemMaxLevel)
                chanceAfter = StaticData.QualityChances[GameRes.SystemLevel, i];
            qualitySlots[i].SetSlotInfo(i + 1, chanceNow, chanceAfter);
        }
    }
}
