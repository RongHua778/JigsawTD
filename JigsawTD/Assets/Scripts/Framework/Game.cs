using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    SceneStateController m_SceneStateController = new SceneStateController();
    public Animator transition;
    public float transitionTime = 0.8f;
    public bool Tutorial = false;
    public bool OnTransition=false;

    protected override void Awake()
    {
        base.Awake();
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
        TurretEffectFactory.Initialize();
    }

    private void Start()
    {
        LevelManager.Instance.Initialize();
        //判断当前初始场景在哪里，根据不同场景初始化当前State
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0://menu
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://battle
                LevelManager.Instance.LoadGame();//直接从战斗场景开始，直接读取存档
                LevelManager.Instance.NeedSaveGame = true;
                m_SceneStateController.SetState(new BattleState(m_SceneStateController));
                break;
        }
        Sound.Instance.BgVolume = 0.5f;
    }

    private void Update()
    {
        m_SceneStateController.StateUpdate();
        //test
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Debug.LogWarning(GameMultiLang.GetTraduction("TEST1"));
        //    LevelManager.Instance.SetUnlockAll(false);
        //    LevelManager.Instance.PremitDifficulty = 0;
        //    PlayerPrefs.DeleteAll();
        //}
        //if (Input.GetKeyDown(KeyCode.J))//解锁全内容
        //{
        //    Debug.LogWarning(GameMultiLang.GetTraduction("TEST2"));
        //    LevelManager.Instance.SetUnlockAll(true);
        //    LevelManager.Instance.PremitDifficulty = 6;
        //    PlayerPrefs.SetInt("MaxDifficulty", 6);
        //}
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
        OnTransition = true;
        transition.SetTrigger("Start");
        m_SceneStateController.EndState();
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
        OnTransition = false;
        yield return SceneManager.LoadSceneAsync(index);
        switch (index)
        {
            case 0://Menu
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://Battle
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
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
