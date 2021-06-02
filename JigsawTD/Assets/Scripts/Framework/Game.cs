﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    SceneStateController m_SceneStateController = new SceneStateController();

    public int Difficulty = 1;
    public Animator transition;
    public float transitionTime = 0.8f;


    protected override void Awake()
    {
        base.Awake();
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //判断当前初始场景在哪里，根据不同场景初始化当前State
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0://menu
                m_SceneStateController.SetState(new MenuState(m_SceneStateController), "MenuState");
                break;
            case 1://battle
                m_SceneStateController.SetState(new BattleState(m_SceneStateController), "BattleState");
                break;
        }
        Sound.Instance.BgVolume = 0.5f;
    }

    private void Update()
    {
        m_SceneStateController.StateUpdate();
    }



    #region 场景读取及转场动画
    //根据名字读取场景
    public void LoadScene(string sceneName)
    {
        if (sceneName == SceneManager.GetActiveScene().name)
        {
            //DebugLog.Logger("请求跳转到了相同场景");
            return;
        }

        StartCoroutine(Transition(sceneName));
    }
    //根据ID读取场景
    public void LoadScene(int index)
    {
        if (index == SceneManager.GetActiveScene().buildIndex)
        {
            //DebugLog.Logger("请求跳转到了相同场景");
            return;
        }
        StartCoroutine(Transition(index));
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Transition(int index)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        transition.SetTrigger("End");

    }
    IEnumerator Transition(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        transition.SetTrigger("End");

    }
    #endregion


    public void QuitGame()
    {
        Application.Quit();
    }

}
