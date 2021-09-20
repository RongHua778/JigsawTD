using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : IUserInterface
{
    private bool gameStart = false;
    [SerializeField] BossSlot[] bossSlots = default;
    [SerializeField] Text highScore = default;
    [SerializeField] GameObject tutorialPanel = default;
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] Text difficultyInfo_Txt = default;
    private Animator m_Anim;
    [SerializeField] Text difficultyTxt = default;
    private int maxDifficulty;
    int difficulty;
    public int Difficulty
    {
        get => difficulty;
        set
        {
            difficulty = Mathf.Clamp(value, 0, maxDifficulty);
            LevelManager.Instance.SelectedLevelID = Difficulty;
            difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + difficulty);
            if (Difficulty == 0)
                difficultyTxt.text = GameMultiLang.GetTraduction("TUTORIAL");
            else
                difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + difficulty.ToString();
        }
    }


    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();

    }
    public void SetLevelInfo()
    {
        maxDifficulty = Mathf.Min(4, PlayerPrefs.GetInt("MaxDifficulty", 0));
        Difficulty = maxDifficulty;
        LevelAttribute attribute = LevelManager.Instance.CurrentLevel;
        //int maxPass = LevelManager.Instance.LevelMaxTurn;
        //highScore.text = GameMultiLang.GetTraduction("HIGHSCORE") + ":" + maxPass + GameMultiLang.GetTraduction("WAVE");
        //for (int i = 0; i < bossSlots.Length; i++)
        //{
        //    bossSlots[i].SetBossInfo(attribute.Boss[i], maxPass, (i + 1) * 10);
        //}


    }

    public void StartGameBtnClick()
    {
        if (!gameStart)
        {
            Game.Instance.SelectDifficulty = Difficulty;
            gameStart = true;
            if (Difficulty == 0)
            {
                Game.Instance.Tutorial = true;

                Game.Instance.SaveData.SaveSelectedElement = Game.Instance.SetTutorialElements();
            }
            else
            {
                Game.Instance.Tutorial = false;
            }
            Game.Instance.LoadScene(1);
        }
    }

    public void DifficultyBtnClick(int count)
    {
        Difficulty += count;
    }

    public override void Show()
    {
        base.Show();
        m_Anim.SetBool("OpenLevel", true);
    }

    public void ClosePanel()
    {
        m_Anim.SetBool("OpenLevel", false);
    }

    public void OnBattleSetBtnClick()
    {
        m_UIBattleSet.Show();
    }
}
