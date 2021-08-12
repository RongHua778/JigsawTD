using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelManager : IUserInterface
{
    private bool gameStart = false;
    [SerializeField] BossSlot[] bossSlots = default;
    [SerializeField] Text levelInfo = default;
    [SerializeField] Text highScore = default;
    [SerializeField] GameObject tutorialPanel = default;
    private Animator m_Anim;

    public override void Initialize()
    {
        base.Initialize();
        m_Anim = this.GetComponent<Animator>();
    }
    public void SetLevelInfo()
    {
        LevelAttribute attribute = LevelManager.Instance.CurrentLevel;
        int maxPass = LevelManager.Instance.LevelMaxTurn;
        levelInfo.text = GameMultiLang.GetTraduction(attribute.LevelInfo);
        highScore.text = GameMultiLang.GetTraduction("HIGHSCORE")+":" + maxPass + GameMultiLang.GetTraduction("WAVE");
        for (int i = 0; i < bossSlots.Length; i++)
        {
            bossSlots[i].SetBossInfo(attribute.Boss[i], maxPass, (i + 1) * 10);
        }
    }

    public void StartGameBtnClick()
    {
        if (!gameStart && LevelManager.Instance.SelectedLevelID == 1)
        {
            tutorialPanel.SetActive(true);
            gameStart = true;
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
}
