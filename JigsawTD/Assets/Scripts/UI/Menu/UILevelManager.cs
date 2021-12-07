using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : IUserInterface
{
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] Text difficultyInfo_Txt = default;
    private Animator m_Anim;
    [SerializeField] Text difficultyTxt = default;
    [SerializeField] Toggle tutorialCheck = default;
    [SerializeField] Text endlessHighScore = default;
    [SerializeField] Text endlessUnlockText = default;
    [SerializeField] GameObject endlessStartBtnObj = default;

    private int maxDifficulty => LevelManager.Instance.PassDiifcutly;
    private int selectDifficulty;

    public int SelectDifficulty
    {
        get => selectDifficulty;
        set
        {
            selectDifficulty = Mathf.Clamp(value, 0, maxDifficulty);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        SelectDifficulty = LevelManager.Instance.PassDiifcutly;
        m_Anim = this.GetComponent<Animator>();
        tutorialCheck.isOn = false;
        endlessHighScore.text = LevelManager.Instance.EndlessHighScore + GameMultiLang.GetTraduction("WAVE");
    }
    public void SetLevelInfo()
    {
        DifficultyBtnClick(0);
        //是否已解锁无尽模式
        endlessStartBtnObj.SetActive(maxDifficulty > 4);
        endlessUnlockText.gameObject.SetActive(maxDifficulty <= 4);
    }

    public void StandardModeStart()
    {
        if (!Game.Instance.OnTransition)
        {
            LevelManager.Instance.StartNewGame(SelectDifficulty);
            Game.Instance.LoadScene(1);
        }
    }

    public void EndlessModeStart()
    {
        if (!Game.Instance.OnTransition)
        {
            LevelManager.Instance.StartNewGame(11);
            Game.Instance.LoadScene(1);
        }
    }



    public void DifficultyBtnClick(int count)
    {
        SelectDifficulty += count;
        difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + SelectDifficulty);
        if (SelectDifficulty == 0)//设置教程显示
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("TUTORIAL");
            tutorialCheck.gameObject.SetActive(true);
            tutorialCheck.isOn = false;
        }
        else
        {
            difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + SelectDifficulty.ToString();
            tutorialCheck.isOn = true;
            tutorialCheck.gameObject.SetActive(false);
        }
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

    //public void OnTutorialCheck(bool value)
    //{
    //    Game.Instance.Tutorial = !value;
    //}
}
