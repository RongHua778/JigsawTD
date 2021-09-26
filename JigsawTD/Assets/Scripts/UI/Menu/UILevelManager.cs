using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : IUserInterface
{
    private bool gameStart = false;
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] Text difficultyInfo_Txt = default;
    private Animator m_Anim;
    [SerializeField] Text difficultyTxt = default;



    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();

    }
    public void SetLevelInfo()
    {
        DifficultyBtnClick(0);
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
            gameStart = true;
            if (LevelManager.Instance.SelectedLevelID == 0)
            {
                Game.Instance.Tutorial = true;

                //Game.Instance.SaveData.SetTutorialElements();
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
        LevelManager.Instance.SelectedLevelID += count;
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + LevelManager.Instance.SelectedLevelID);
        if (LevelManager.Instance.SelectedLevelID == 0)
            difficultyTxt.text = GameMultiLang.GetTraduction("TUTORIAL");
        else
            difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + LevelManager.Instance.SelectedLevelID.ToString();
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
