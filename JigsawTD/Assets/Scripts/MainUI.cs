using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : IUserInterface
{
    public GameObject LifeObj;
    public GameObject MoneyObj;
    public GameObject SpeedBtnObj;
    public GameObject GuideVideoBtnObj;
    public GameObject WaveObj;
    private Animator m_Anim;
    //UI
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] Text speedBtnTxt = default;
    [SerializeField] Text waveTxt = default;
    [SerializeField] Image enemyIcon = default;
    [SerializeField] InfoBtn enemyInfo = default;
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
        CurrentWave = 0;
        Life = StaticData.Instance.PlayerMaxHealth;
        Coin = StaticData.Instance.StartCoin;

        m_PausePanel.Initialize(m_GameManager);
        m_GuideBook.Initialize(m_GameManager);
        m_Anim = this.GetComponent<Animator>();

    }

    public override void Release()
    {
        base.Release();
        GameSpeed = 1;
        GameEvents.Instance.onEnemyReach -= EnemyReach;
    }

    public void PrepareForGuide()
    {
        MoneyObj.SetActive(false);
        SpeedBtnObj.SetActive(false);
        GuideVideoBtnObj.SetActive(false);
        LifeObj.SetActive(false);
        WaveObj.SetActive(false);
    }

    public override void Show()
    {
        m_Anim.SetBool("Show", true);
    }

    public void PlayAnim(string key,bool value)
    {
        m_Anim.SetBool(key, value);
    }

    private void EnemyReach(Enemy enemy)
    {
        Life--;
    }



    public void PrepareNextWave(EnemySequence sequence)
    {
        CurrentWave++;
        Coin += StaticData.Instance.BaseWaveIncome + StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1);
        enemyIcon.sprite = sequence.EnemyAttribute[0].EnemyIcon;
        enemyInfo.SetContent(sequence.EnemyAttribute[0].Description);
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
            GameManager.Instance.ShowMessage("拥有的金币不足");
            return false;
        }
    }


    public void GuideBookBtnClick()
    {
        m_GameManager.ShowGuideVideo(0);
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
