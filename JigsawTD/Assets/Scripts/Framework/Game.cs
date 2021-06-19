using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    SceneStateController m_SceneStateController = new SceneStateController();

    public int Difficulty = 1;
    //******** 增加了一个新难度用来测试关卡
    public int MaxDifficulty = 4;

    public Animator transition;
    public float transitionTime = 0.8f;
    public bool Tutorial = false;


    protected override void Awake()
    {
        base.Awake();
        Application.runInBackground = true;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //判断当前初始场景在哪里，根据不同场景初始化当前State
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0://menu
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://battle
                m_SceneStateController.SetState(new BattleState(m_SceneStateController));
                break;
        }
        Sound.Instance.BgVolume = 0.5f;
    }

    private void Update()
    {
        m_SceneStateController.StateUpdate();
    }



    #region 场景读取及转场动画
    //根据ID读取场景
    public void LoadScene(int index)
    {
        StartCoroutine(Transition(index));
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator Transition(int index)
    {
        transition.SetTrigger("Start");
        m_SceneStateController.EndState();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        yield return SceneManager.LoadSceneAsync(index);
        switch (index)
        {
            case 0://Menu
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://Battle
                m_SceneStateController.SetState(new BattleState(m_SceneStateController));
                break;
        }
        transition.SetTrigger("End");

    }
    #endregion


    public void QuitGame()
    {
        Application.Quit();
    }

}
