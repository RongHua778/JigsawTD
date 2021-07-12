using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : IUserInterface
{
    public GameObject LifeObj;
    public GameObject MoneyObj;
    public GameObject TopLeftArea = default;
    public GameObject WaveObj;
    private Animator m_Anim;
    //UI
    [SerializeField] Image GameSpeedImg = default;
    [SerializeField] Text PlayerLifeTxt = default;
    [SerializeField] Text coinTxt = default;
    [SerializeField] WaveInfoSetter m_WaveInfoSetter = default;
    [SerializeField] PausePanel m_PausePanel = default;
    [SerializeField] GuideBook m_GuideBook = default;


    [SerializeField] Text[] taskTexts= default;
    [SerializeField] Button[] taskButtons = default;

    List<Task> tasksInPocket = new List<Task>();
    List<Task> taksOutPocket = new List<Task>();
    public Task clickedTask;

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


    [SerializeField] Sprite[] GameSpeedSprites = default;
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
            GameSpeedImg.sprite = GameSpeedSprites[GameSpeed - 1];
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
        TopLeftArea.SetActive(false);
        MoneyObj.SetActive(false);

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
        for (int i = 0; i < tasksInPocket.Count; i++)
        {
            tasksInPocket[i].CountTask();
            if (tasksInPocket[i].TaskComplete)
            {
                tasksInPocket[i].Reclaim(tasksInPocket);
            }

        }
        foreach(Task t in taksOutPocket)
        {
            t.CountDisappear();
        }
        UpdateTaskInfo();
        clickedTask = null;
    }

    private void UpdateTaskInfo() 
    {
        for (int i = 0; i < tasksInPocket.Count; i++)
        {
            taskTexts[i].text = tasksInPocket[i].GetInfo();
            if (tasksInPocket[i].Actived)
            {
                taskButtons[i].gameObject.SetActive(false);
            }
            else
            {
                taskButtons[i].gameObject.SetActive(true);
            }
        }
        if (tasksInPocket.Count < 3)
        {
            for (int i = tasksInPocket.Count; i < 3; i++)
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
        if (tasksInPocket.Count < 3)
        {
            taksOutPocket.Remove(task);
            task.AddTo(tasksInPocket);
            UpdateTaskInfo();
        }
        //task.PlayTask();
    }

    public void ReplaceTask(int i)
    {
        if (clickedTask != null)
        {
            clickedTask.Replace(tasksInPocket,i);
            clickedTask = null;
            UpdateTaskInfo();
        }
    }

    public void PlayTask(int i)
    {
        tasksInPocket[i].PlayTask();
        taskButtons[i].gameObject.SetActive(false);
        taskTexts[i].text=tasksInPocket[0].GetInfo();
        m_WaveInfoSetter.SetWaveInfo(GameManager.Instance.WaveSystem.LevelSequence[0]);
    }
}
