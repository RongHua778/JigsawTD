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
    //tips
    [SerializeField] TurretTips m_TurretTips = default;
    [SerializeField] TrapTips m_TrapTips = default;
    [SerializeField] EnemyInfoTips m_EnemyInfoTips = default;

    public void Initinal()
    {
        Sound.Instance.PlayBg("menu");
        m_Message.Initialize();
        m_UILevelManager.Initialize();
        m_UITujian.Initialize();
        m_UIBattleSet.Initialize();
        m_TurretTips.Initialize();
        m_TrapTips.Initialize();
    }
    public void Release()
    {
        m_Message.Release();
        m_UITujian.Release();
    }

    public void GameUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST1"));
            LevelManager.Instance.PremitDifficulty = 0;
            PlayerPrefs.DeleteAll();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            ShowMessage(GameMultiLang.GetTraduction("TEST2"));
            LevelManager.Instance.PremitDifficulty = 4;
            PlayerPrefs.SetInt("MaxDifficulty", 4);
        }
    }

    public void StartGameBtnClick()
    {
        m_UILevelManager.Show();
        m_UILevelManager.SetLevelInfo();
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

    public void ShowTrapTips(TrapAttribute att,Vector2 pos)
    {
        m_TrapTips.transform.position = pos;
        m_TrapTips.Show();
        m_TrapTips.ReadTrapAtt(att);
    }

    public void ShowEnemyInfoTips(EnemyAttribute att,Vector2 pos)
    {
        m_EnemyInfoTips.transform.position = pos;
        m_EnemyInfoTips.Show();
        m_EnemyInfoTips.ReadEnemyAtt(att);
    }

    public void BluePrintBtnClick()
    {
        ShowMessage("暂未开放");
    }

    public void SettingBtnClick()
    {
        ShowMessage("暂未开放");
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
