using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGroundTips : TileTips
{
    public void ReadInfo()
    {
        Name.text = GameManager.Instance.BuyOneGroundMoney.ToString();
        Description.text = "����һ��հ׵ذ�";
    }
}
