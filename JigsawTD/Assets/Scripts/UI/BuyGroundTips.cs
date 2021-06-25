using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGroundTips : TileTips
{
    public void ReadInfo()
    {
        Name.text = GameManager.Instance.BuyOneGroundMoney.ToString();
        Description.text = "¹ºÂòÒ»¿é¿Õ°×µØ°å";
    }

    public override void Hide()
    {
        base.Hide();
        BoardSystem.SelectingGround = null;
    }
}
