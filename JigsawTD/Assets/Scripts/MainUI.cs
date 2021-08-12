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
                GameManager.Instance.GameEnd(CurrentWave);
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


    public override void Initialize()
    {
        GameEvents.Instance.onEnemyReach += EnemyReach;
        GameSpeed = 1;
        CurrentWave = 0;
        Life = StaticData.Instance.PlayerMaxHealth[Game.Instance.Difficulty - 1];
        Coin = StaticData.Instance.StartCoin;

        m_PausePanel.Initialize();
        m_GuideBook.Initialize();
        m_Anim = GetComponent<Animator>();

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



    public void PrepareNextWave(List<EnemySequence> sequences)
    {
        //CountTasks();
        CurrentWave++;
        GameManager.Instance.GainInterest();
        GameManager.Instance.GainMoney((StaticData.Instance.BaseWaveIncome +
            StaticData.Instance.WaveMultiplyIncome * (CurrentWave - 1)));//最多一回合获得200金币
        m_WaveInfoSetter.SetWaveInfo(CurrentWave, sequences);
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
            GameManager.Instance.ShowMessage(GameMultiLang.GetTraduction("LACKMONEY"));
            return false;
        }
    }

    public void GuideBookBtnClick()
    {
        GameManager.Instance.ShowGuideVideo(0);
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
