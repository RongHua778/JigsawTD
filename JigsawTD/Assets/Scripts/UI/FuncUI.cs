using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncUI : IUserInterface
{
    bool drawThisTurn = false;
    public bool DrawThisTurn { get => drawThisTurn; set => drawThisTurn = value; }

    private int luckProgress = 0;
    public int LuckProgress { get => luckProgress; set => luckProgress = value; }

    private int drawRemain = 1;
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
        set 
        {
            playerLevel = value;
            PlayerLevelTxt.text = "当前等级：" + PlayerLevel.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[PlayerLevel];
            if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
            {
                LevelUpTxt.text = "升级: " + PlayerLvUpMoney.ToString();
            }
            else
            {
                LevelUpTxt.text = "已满级";
            }
        }
    }

    int playerLvUpMoney = 0;
    public int PlayerLvUpMoney
    {
        get => playerLvUpMoney;
        set
        {
            playerLvUpMoney = value;
        }
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

    [SerializeField] Text LuckPointTxt = default;
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpTxt = default;
    [SerializeField] Text PlayerLevelTxt = default;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        DrawRemain = 100;
    }
    public void DrawBtnClick()
    {
        if (DrawRemain > 0)
        {
            DrawRemain--;
            m_GameManager.DrawShapes();
        }
        else
        {
            GameEvents.Instance.Message("抽取次数不足");
        }
    }

    public void NextWaveBtnClick()
    {
        m_GameManager.StartNewWave();
    }
}
