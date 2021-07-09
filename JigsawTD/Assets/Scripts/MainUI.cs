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
    [SerializeField] WaveInfoSetter m_WaveInfoSetter = default;
    [SerializeField] PausePanel m_PausePanel = default;
    [SerializeField] GuideBook m_GuideBook = default;


    [SerializeField] Text[] taskText= default;

    List<Task> tasks = new List<Task>();

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
            life = Mathf.Clamp(value, 0, StaticData.Instance.PlayerMaxHealth[Game.Instance.Difficulty - 1]);
            PlayerLifeTxt.text = life.ToString() + "/" + StaticData.Instance.PlayerMaxHealth[Game.Instance.Difficulty - 1].ToString();
        }
    }
    int currentWave;
    public int CurrentWave
    {
        get => currentWave;
        set
        {
            currentWave = value;
        }
    }

    //��Ϸ�ٶ�
    private int gameSpeed = 1;
    public int GameSpeed
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
            speedBtnTxt.text = "��Ϸ�ٶ�X" + gameSpeed;
            Time.timeScale = gameSpeed;
        }
    }


    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameSpeed = 1;
        CurrentWave = 0;
        Life = StaticData.Instance.PlayerMaxHealth[Game.Instance.Difficulty - 1];
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

    public void PlayAnim(string key, bool value)
    {
        m_Anim.SetBool(key, value);

    }

    private void EnemyReach(Enemy enemy)
    {
        Life -= enemy.ReachDamage;
    }



    public void PrepareNextWave(EnemySequence sequence, int luckCoin)
    {
        CountTasks();
        CurrentWave++;
        m_GameManager.GainMoney((int)((StaticData.Instance.BaseWaveIncome + StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1)) * (1 + luckCoin * 0.1f)));
        m_WaveInfoSetter.SetWaveInfo(sequence);
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
            GameManager.Instance.ShowMessage("ӵ�еĽ�Ҳ���");
            return false;
        }
    }

    private void CountTasks()
    {
        foreach(Task t in tasks)
        {
            t.CountTask();
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

    public void GetTask()
    {
        Task task = GameManager.Instance.TaskFactory.GetRandomTask();
        //task.PlayTask();
        //m_WaveInfoSetter.SetWaveInfo(GameManager.Instance.WaveSystem.LevelSequence[0]);
    }


}
