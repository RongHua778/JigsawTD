using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : Singleton<ResourcesManager>
{
    private int coin = 0;
    public int Coin { get => coin; set => coin = value; }

    private int life = 15;
    public int Life { get => life; set => life = value; }

    private int drawRemain = 100;
    public int DrawRemain
    {
        get => drawRemain;
        set
        {
            drawRemain = value;
            DrawBtnTxt.text = "抽取模块X" + drawRemain.ToString();
        }
    }

    private int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set => playerLevel = value;
    }

    private int luckPoint;
    public int LuckPoint
    {
        get => luckPoint;
        set
        {
            luckPoint = value;
            LuckPointTxt.text = "累积点:" + luckPoint.ToString();
        }
    }

    private int luckProgress = 0;
    public int LuckProgress { get => luckProgress; set => luckProgress = value; }

    //UI
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpBtnTxt = default;
    [SerializeField] Text LuckPointTxt = default;


}
