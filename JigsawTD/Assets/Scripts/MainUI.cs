using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : IUserInterface
{

    //UI
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] Text speedBtnTxt = default;
    [SerializeField] Text waveTxt = default;

    [SerializeField] PausePanel m_PausePanel = default;
    [SerializeField] GuideBook m_GuideBook = default;
    private int coin = 0;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            coinTxt.text = coin.ToString();
        }
    }

    private int life;
    public int Life
    {
        get => life;
        set
        {
            if (value <= 0)
            {
                m_GameManager.GameEnd(false);
            }
            life = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth);
            PlayerLifeTxt.text = life.ToString() + "/" + StaticData.Instance.PlayerMaxHealth.ToString();
        }
    }
    int currentWave;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
            waveTxt.text = "第" + currentWave.ToString() + "/" + StaticData.Instance.LevelMaxWave.ToString() + "波";
        }
    }

    //游戏速度
    private float gameSpeed = 1;
    public float GameSpeed
    {
        get => gameSpeed;
        set
        {
            if (value > 3)
            {
                gameSpeed = 1;
            }
            else
            {
                gameSpeed = value;
            }
            speedBtnTxt.text = "游戏速度X" + gameSpeed;
            Time.timeScale = gameSpeed;
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameSpeed = 1;
        CurrentWave = 1;
        Life = StaticData.Instance.PlayerMaxHealth;
        Coin = StaticData.Instance.StartCoin;

        m_PausePanel.Initialize(m_GameManager);
        m_GuideBook.Initialize(m_GameManager);
    }

    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
        GameEvents.Instance.onEnemyReach -= EnemyReach;
    }

    private void EnemyReach(Enemy enemy)
    {
        Life--;
    }



    public void PrepareNextWave()
    {
        CurrentWave++;
        Coin += StaticData.Instance.BaseWaveIncome + StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1);

    }

    public bool ConsumeMoney(int cost)
    {
        if (Coin >= cost)
        {
            Coin -= cost;
            return true;
        }
        else
        {
            GameEvents.Instance.Message("拥有的金币不足");
            return false;
        }
    }


    public void GuideBookBtnClick()
    {
        m_GuideBook.Show();
    }

    public void PauseBtnClick()
    {
        m_PausePanel.Show();
    }

    public void GameSpeedBtnClick()
    {
        GameSpeed++;
    }


}
