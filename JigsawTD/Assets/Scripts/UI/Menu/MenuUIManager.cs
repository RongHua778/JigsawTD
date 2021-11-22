using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : Singleton<MenuUIManager>
{
    //界面系统
    [SerializeField] UILevelManager m_UILevelManager = default;
    [SerializeField] MenuMessage m_Message = default;
    [SerializeField] UIBattleSet m_UIBattleSet = default;
    [SerializeField] UITujian m_UITujian = default;
    [SerializeField] UISetting m_UISetting = default;
    //tips
    [SerializeField] TurretTips m_TurretTips = default;
    [SerializeField] TrapTips m_TrapTips = default;
    [SerializeField] EnemyInfoTips m_EnemyInfoTips = default;
    [SerializeField] ConfirmPanel m_ConfirmPanel = default;
    [SerializeField] GameObject m_ContinueGameBtn = default;

    public void Initinal()
    {
        Sound.Instance.PlayBg("menu");
        m_Message.Initialize();
        m_UILevelManager.Initialize();
        m_UITujian.Initialize();
        m_UISetting.Initialize();
        m_UIBattleSet.Initialize();
        m_TurretTips.Initialize();
        m_TrapTips.Initialize();
        m_ConfirmPanel.Initialize();
        LevelManager.Instance.LoadGame();//每次进入菜单页，就读取一次存档
        m_ContinueGameBtn.SetActive(LevelManager.Instance.HasLastGame);

    }

    public void Release()
    {
        m_Message.Release();
        m_UITujian.Release();
        m_UISetting.Release();
    }

    public void GameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST1"));
            LevelManager.Instance.SetUnlockAll(false);
            LevelManager.Instance.PremitDifficulty = 0;
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.J))//解锁全内容
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST2"));
            LevelManager.Instance.SetUnlockAll(true);
            LevelManager.Instance.PremitDifficulty = 6;
            PlayerPrefs.SetInt("MaxDifficulty", 6);
        }
    }

    public void StartGameBtnClick()
    {

        m_UILevelManager.Show();
        m_UILevelManager.SetLevelInfo();
    }

    public void ContinueGameBtnClick()
    {
        Game.Instance.Tutorial = false;
        Game.Instance.LoadScene(1);
    }



    public void TujianBtnClick()
    {
        m_UITujian.Show();
    }

    public void ShowTurretTips(TurretAttribute att, Vector2 pos)
    {
        m_TurretTips.transform.position = pos;
        m_TurretTips.Show();
        m_TurretTips.ReadAttribute(att);
    }

    public void ShowTrapTips(TrapAttribute att, Vector2 pos)
    {
        m_TrapTips.transform.position = pos;
        m_TrapTips.Show();
        m_TrapTips.ReadTrapAtt(att);
    }

    public void ShowEnemyInfoTips(EnemyAttribute att, Vector2 pos)
    {
        m_EnemyInfoTips.transform.position = pos;
        m_EnemyInfoTips.Show();
        m_EnemyInfoTips.ReadEnemyAtt(att);
    }

    public void SettingBtnClick()
    {
        m_UISetting.Show();
    }

    internal void HideTips()
    {
        m_TurretTips.Hide();
        m_TrapTips.Hide();
        m_EnemyInfoTips.Hide();
    }

    public void ShowMessage(string content)
    {
        m_Message.SetText(content);
    }


    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }

    public void UnlockAll()
    {

    }

    public void AddtoWishList()
    {
        Application.OpenURL("https://store.steampowered.com/app/1664670/_Refactor");
    }

    //public void DifficultBtnClick(int value)
    //{
    //    Game.Instance.Difficulty += value;
    //    if (Game.Instance.Difficulty > Game.Instance.MaxDifficulty)
    //    {
    //        Game.Instance.Difficulty = 1;
    //    }
    //    else if (Game.Instance.Difficulty < 1)
    //    {
    //        Game.Instance.Difficulty = Game.Instance.MaxDifficulty;
    //    }
    //}


}
