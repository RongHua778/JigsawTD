using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuncUI : IUserInterface
{
    [SerializeField] Text LuckPointTxt = default;
    [SerializeField] Text DrawBtnTxt = default;
    [SerializeField] Text LevelUpTxt = default;
    [SerializeField] Text PlayerLevelTxt = default;

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
            DrawBtnTxt.text = "��ȡģ��X" + drawRemain.ToString();
        }
    }

    private int playerLevel = 1;
    public int PlayerLevel
    {
        get => playerLevel;
        set 
        {
            playerLevel = value;
            PlayerLevelTxt.text = "��ǰ�ȼ���" + PlayerLevel.ToString();
            PlayerLvUpMoney = StaticData.Instance.LevelUpMoney[PlayerLevel];
            if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
            {
                LevelUpTxt.text = "����: " + PlayerLvUpMoney.ToString();
            }
            else
            {
                LevelUpTxt.text = "������";
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
            if (luckPoint >= 10)
            {
                luckPoint = 0;
                DrawRemain++;
            }
            LuckPointTxt.text = "�ۻ���:" + luckPoint.ToString();
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        DrawRemain = StaticData.Instance.StartLotteryDraw;
        PlayerLevel = 1;
    }
    public void DrawBtnClick()
    {
        if (DrawRemain > 0)
        {
            DrawRemain--;
            DrawThisTurn = true;
            m_GameManager.DrawShapes();
        }
        else
        {
            GameManager.Instance.ShowMessage("��ȡ��������");
        }
    }

    public void PrepareNextWave()
    {
        if (!DrawThisTurn)
        {
            LuckPoint += LuckProgress;
            LuckProgress += 2;
        }
        DrawThisTurn = false;
        DrawRemain++;
    }

    public void NextWaveBtnClick()
    {
        m_GameManager.StartNewWave();
    }

    public void LevelUpBtnClick()
    {
        if (PlayerLevel < StaticData.Instance.PlayerMaxLevel)
        {
            if (GameManager.Instance.ConsumeMoney(PlayerLvUpMoney))
            {
                PlayerLevel++;
            }
        }
    }
}
