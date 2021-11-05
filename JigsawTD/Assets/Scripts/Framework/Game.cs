﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using LitJson;

[RequireComponent(typeof(Sound))]
public class Game : Singleton<Game>
{
    SceneStateController m_SceneStateController = new SceneStateController();
    public Animator transition;
    public float transitionTime = 0.8f;
    public bool Tutorial = false;


    protected override void Awake()
    {
        base.Awake();
        //LoadByJson();
        Application.runInBackground = true;
        DontDestroyOnLoad(this.gameObject);
        TurretEffectFactory.Initialize();
    }

    private void Start()
    {
        LoadByJson();
        //判断当前初始场景在哪里，根据不同场景初始化当前State
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex)
        {
            case 0://menu
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(1);
                m_SceneStateController.SetState(new MenuState(m_SceneStateController));
                break;
            case 1://battle
                //Camera.main.GetComponent<UniversalAdditionalCameraData>().SetRenderer(0);
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


    private void OnApplicationQuit()
    {
        SaveByJson();
    }

    public void SaveGame()
    {
        SaveByJson();
    }
    private void SaveByJson()
    {
        Save save = LevelManager.Instance.SetSaveData();
        string filePath = Application.persistentDataPath + "/JsonSave.json";
        Debug.Log(filePath);
        string saveJsonStr = JsonMapper.ToJson(save);
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        sw.Close();

        Debug.Log("Saved!");
    }

    private void LoadByJson()
    {
        string filePath = Application.persistentDataPath + "/JsonSave.json";
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            Save save = JsonMapper.ToObject<Save>(jsonStr);
            LevelManager.Instance.LoadSaveData(save);
        }
        else
        {
            LevelManager.Instance.FirstGameData();
            Debug.Log("NoSaveData.");
        }
    }


    //存档
    //public void SaveGame(Save save)
    //{
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
    //    bf.Serialize(file, save);
    //    file.Close();
    //    Debug.Log("Game Save");

    //}

    //public void LoadGame()
    //{
    //    if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    //    {
    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    //        Save save = (Save)bf.Deserialize(file);
    //        file.Close();
    //        SaveData = save;

    //    }
    //    else
    //    {
    //        Save save = new Save();
    //        save.Initialize();
    //        SaveData = save;
    //    }
    //}



}
