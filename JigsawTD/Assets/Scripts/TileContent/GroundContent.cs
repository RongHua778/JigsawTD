using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundContent : GameTileContent
{
    public override GameTileContentType ContentType => GameTileContentType.Ground;


    public override void OnContentSelected(bool value)
    {
        base.OnContentSelected(value);
        if (value)
        {
            GameManager.Instance.ShowBuyGroundTips();
        }
    }
}
