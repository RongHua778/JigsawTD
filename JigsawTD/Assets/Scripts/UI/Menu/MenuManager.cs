using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    Canvas m_Canvas;
    //界面系统
    [SerializeField] UIMenu m_UIMenu = default;
    [SerializeField] UIMode m_UILevelManager = default;
    [SerializeField] MenuMessage m_UIMessage = default;
    [SerializeField] UITujian m_UITujian = default;
    [SerializeField] UISetting m_UISetting = default;
    //tips
    [SerializeField] TurretTips m_TurretTips = default;
    [SerializeField] TrapTips m_TrapTips = default;
    [SerializeField] EnemyInfoTips m_EnemyInfoTips = default;

    public void Initinal()
    {
        m_Canvas = this.GetComponent<Canvas>();
        LevelManager.Instance.LoadGame();//每次进入菜单页，就读取一次存档
        //UI
        m_UIMenu.Initialize();
        m_UIMessage.Initialize();
        m_UILevelManager.Initialize();
        m_UITujian.Initialize();
        m_UISetting.Initialize();

        //TIPS
        m_TurretTips.Initialize();
        m_TrapTips.Initialize();
        m_EnemyInfoTips.Initialize();


    }

    public void Release()
    {

    }

    public void StartGame()
    {
        m_UIMenu.ClosePanel();
        m_UILevelManager.Show();
    }

    public void ContinueGame()
    {
        LevelManager.Instance.ContinueLastGame();
    }

    public void ShowMenu()
    {
        m_UIMenu.Show();
    }

    public void GameUpdate()
    {

    }

    public void OpenTujian()
    {
        m_UIMenu.ClosePanel();
        m_UITujian.Show();
    }

    public void ShowTurretTips(TurretAttribute att, Vector2 pos)
    {
        SetCanvasPos(m_TurretTips.transform, pos);
        m_TurretTips.Show();
        m_TurretTips.ReadAttribute(att);
    }

    public void ShowTrapTips(TrapAttribute att, Vector2 pos)
    {
        SetCanvasPos(m_TrapTips.transform, pos);

        m_TrapTips.Show();
        m_TrapTips.ReadTrapAtt(att);
    }

    public void ShowEnemyInfoTips(EnemyAttribute att, Vector2 pos)
    {
        SetCanvasPos(m_EnemyInfoTips.transform, pos);
        m_EnemyInfoTips.Show();
        m_EnemyInfoTips.ReadEnemyAtt(att);
    }
    private void SetCanvasPos(Transform tr, Vector2 pos)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.transform as RectTransform, pos, m_Canvas.worldCamera, out newPos);
        tr.position = m_Canvas.transform.TransformPoint(newPos);
    }
    public void OpenSetting()
    {
        m_UIMenu.ClosePanel();
        m_UISetting.Show();
    }

    internal void HideTips()
    {
        m_TurretTips.CloseTips();
        m_TrapTips.CloseTips();
        m_EnemyInfoTips.CloseTips();
    }

    public void ShowMessage(string content)
    {
        m_UIMessage.SetText(content);
    }


    public void QuitGameBtnClick()
    {
        Game.Instance.QuitGame();
    }



}
