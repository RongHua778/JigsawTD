using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QualitySlot : MonoBehaviour
{
    [SerializeField] Text QualityTxt = default;
    [SerializeField] Text ChanceNow = default;
    [SerializeField] Text ChanceAfter = default;
    [SerializeField] Image ChanceProgress = default;
    [SerializeField] Color UpColor = default;
    [SerializeField] Color DownColor = default;
    public void SetSlotInfo(int quality, float chanceNow, float chanceAfter)
    {
        QualityTxt.text = GameMultiLang.GetTraduction("MODULELEVELINFO2") + quality;
        ChanceProgress.fillAmount = chanceNow / 1;
        ChanceNow.text = chanceNow * 100 + "%";
        ChanceAfter.text = chanceAfter * 100 + "%";
        ChanceAfter.color = chanceAfter >= chanceNow ? UpColor : DownColor;
    }

}
