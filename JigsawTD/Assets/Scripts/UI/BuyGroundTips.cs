using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyGroundTips : TileTips
{
    [SerializeField] TextMeshProUGUI costTxt = default;
    public void ReadInfo(int cost)
    {
        costTxt.text = GameMultiLang.GetTraduction("BUY") + "<sprite=7>" + cost.ToString();
    }
}
