using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGroundTips : TileTips
{
    public void ReadInfo(int cost)
    {
        Name.text = cost.ToString();
    }
}
