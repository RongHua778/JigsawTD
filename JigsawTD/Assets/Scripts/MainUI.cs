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


    [SerializeField] Text[] taskTexts= default;
    [SerializeField] Button[] taskButtons = default;

    List<Task> taksInPocket = new List<Task>();
    List<Task> taksOutPocket = new List<Task>();

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

    //游戏速度
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
        Life = StaticData.Instance.PlayerMaxHealth[Game.Instance.Difficulty - 1];
        Coin = StaticData.Instance.StartCoin;

        m_PausePanel.Initialize(m_GameManager);
        m_GuideBook.Initialize(m_GameManager);
        m_Anim = GetComponent<Animator>();
        for (int i = 0; i < taskButtons.Length; i++)
        {
            taskButtons[i].gameObject.SetActive(false);
        }
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
            GameManager.Instance.ShowMessage("拥有的金币不足");
            return false;
        }
    }

    private void CountTasks()
    {
        for (int i = 0; i < taksInPocket.Count; i++)
        {
            taksInPocket[i].CountTask();
            if (taksInPocket[i].TaskComplete)
            {
                taksInPocket[i].Reclaim(taksInPocket);
            }

        }
        foreach(Task t in taksOutPocket)
        {
            t.CountDisappear();
        }
        UpdateTaskInfo();
    }

    private void UpdateTaskInfo() 
    {
        for (int i = 0; i < taksInPocket.Count; i++)
        {
            taskTexts[i].text = taksInPocket[i].GetInfo();
            if (taksInPocket[i].Actived)
            {
                taskButtons[i].gameObject.SetActive(false);
            }
            else
            {
                taskButtons[i].gameObject.SetActive(true);
            }
        }
        if (taksInPocket.Count < 3)
        {
            for (int i = taksInPocket.Count; i < 3; i++)
            {
                taskTexts[i].text = "空白";
                taskButtons[i].gameObject.SetActive(false);
            }
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

    public void GetTask(Transform t)
    {
        Task task = GameManager.Instance.TaskFactory.GetRandomTask();
        task.transform.position = t.position;
        taksOutPocket.Add(task);
        if (taksInPocket.Count < 3)
        {
            taksOutPocket.Remove(task);
            task.AddTo(taksInPocket);
            UpdateTaskInfo();
        }
        //task.PlayTask();
    }

    public void PlayTask1()
    {
        taksInPocket[0].PlayTask();
        taskButtons[0].gameObject.SetActive(false);
        taskTexts[0].text=taksInPocket[0].GetInfo();
        m_WaveInfoSetter.SetWaveInfo(GameManager.Instance.WaveSystem.LevelSequence[0]);
    }

    public void PlayTask2()
    {
        taksInPocket[1].PlayTask();
        taskButtons[1].gameObject.SetActive(false);
        taskTexts[1].text = taksInPocket[1].GetInfo();
        m_WaveInfoSetter.SetWaveInfo(GameManager.Instance.WaveSystem.LevelSequence[0]);
    }

    public void PlayTask3()
    {
        taksInPocket[2].PlayTask();
        taskButtons[2].gameObject.SetActive(false);
        taskTexts[2].text = taksInPocket[2].GetInfo();
        m_WaveInfoSetter.SetWaveInfo(GameManager.Instance.WaveSystem.LevelSequence[0]);
    }
}
