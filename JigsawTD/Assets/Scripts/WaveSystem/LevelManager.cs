using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using LitJson;

[System.Serializable]
public class GameLevelInfo
{
    public int ExpRequire;
    public ContentAttribute[] UnlockItems;
}

public class LevelManager : Singleton<LevelManager>
{
    [HideInInspector] public List<GameTileContent> GameContents;
    #region ��������
    [SerializeField] private GameLevelInfo[] GameLevels = default;
    [SerializeField] int permitGameLevel = default;
    public int MaxGameLevel => Mathf.Min(GameLevels.Length - 1, permitGameLevel);
    public int GameLevel { get => PlayerPrefs.GetInt("GameLevel", 0); set => PlayerPrefs.SetInt("GameLevel", Mathf.Min(value, MaxGameLevel)); }
    public int GameExp { get => PlayerPrefs.GetInt("GameExp", 0); set => PlayerPrefs.SetInt("GameExp", value); }

    public List<ContentAttribute> AllContent;

    public LevelAttribute[] StandardLevels = default;
    private Dictionary<int, LevelAttribute> LevelDIC;

    public int PremitDifficulty = default;
    public int PassDiifcutly
    {
        get => Mathf.Min(PremitDifficulty, PlayerPrefs.GetInt("MaxDifficulty", 0));
        set
        {
            if (value > PlayerPrefs.GetInt("MaxDifficulty", 0))
                PlayerPrefs.SetInt("MaxDifficulty", Mathf.Min(5, value));
        }
    }

    public int EndlessHighScore
    {
        get => PlayerPrefs.GetInt("EndlessHighscore", 0);
        set
        {
            if (value > PlayerPrefs.GetInt("EndlessHighscore", 0))
                PlayerPrefs.SetInt("EndlessHighscore", value);
        }
    }
    public LevelAttribute CurrentLevel;
    #endregion

    private string SaveGameFilePath;


    #region ��ʱ��Ϸ����
    [Header("�Ƿ��ȡ�浵")]
    [SerializeField] bool NeedLoadGame = default;
    //[SerializeField] bool NeedLoadData = default;
    [Header("�Ƿ���δ�����Ϸ")]
    //public DataSave LastDataSave;
    public GameSave LastGameSave;
    public bool LevelEnd = true;//��Ϸ�Ƿ����
    #endregion

    public void Initialize()
    {
        LitJsonRegister.Register();
        SaveGameFilePath = Application.persistentDataPath + "/GameSave.json";

        LevelDIC = new Dictionary<int, LevelAttribute>();
        GameContents = new List<GameTileContent>();

        foreach (var level in StandardLevels)
        {
            LevelDIC.Add(level.Mode, level);
        }
    }

    private void DeleteGameSave()//ɾ���Ծִ浵�ļ�
    {
        if (File.Exists(SaveGameFilePath))
            File.Delete(SaveGameFilePath);
    }

    public LevelAttribute GetLevelAtt(int mode)
    {
        if (LevelDIC.ContainsKey(mode))
        {
            return LevelDIC[mode];
        }
        else
        {
            Debug.LogWarning("�����ģʽ����");
            return null;
        }
    }


    public void SetGameLevel(int level)
    {
        GameLevel = level;
        GameExp = 0;
        foreach (var item in AllContent)
        {
            item.isLock = item.initialLock;
        }
        for (int i = 0; i < GameLevel + 1; i++)//�������ڵ�ǰ�ȼ��Ľ���
        {
            foreach (var item in GameLevels[i].UnlockItems)
            {
                item.isLock = false;
            }
        }


    }




    public int GainExp(int wave)
    {
        int exp = Mathf.RoundToInt(CurrentLevel.ExpIntensify * 5 * wave * (1 + wave / 10 * 0.25f));
        AddExp(exp);
        return exp;
    }


    private void UnlockBonus(string bo)
    {
        foreach (var item in AllContent)
        {
            if (item.Name == bo)
            {
                item.isLock = false;
                break;
            }
        }

    }

    private void AddExp(int exp)
    {
        if (GameLevel >= MaxGameLevel)//�ﵽ���ȼ���ֻ�Ӿ��鲻����
        {
            GameExp += exp;
            return;
        }
        int need = GameLevels[GameLevel].ExpRequire - GameExp;
        if (exp >= need)
        {
            foreach (var item in GameLevels[GameLevel].UnlockItems)
            {
                UnlockBonus(item.Name);
            }
            GameLevel++;
            GameExp = 0;
            AddExp(exp - need);
        }
        else
        {
            GameExp += exp;
        }
    }

    public GameLevelInfo GetLevelInfo(int level)
    {
        return GameLevels[level];
    }

    //�浵����

    public void StartNewGame(int modeID)
    {
        LevelEnd = false;
        LastGameSave.ClearGame();
        CurrentLevel = GetLevelAtt(modeID);
        DeleteGameSave();
    }
    //private void LoadSaveData()
    //{
    //    foreach (var item in AllContent)//���ݴ浵�������ݣ���������������ݣ���Ĭ������
    //    {
    //        if (LastDataSave.UnlockInfoDIC.ContainsKey(item.Name))
    //        {
    //            item.isLock = LastDataSave.UnlockInfoDIC[item.Name];
    //        }
    //        else
    //        {
    //            item.isLock = true;
    //        }
    //    }
    //    for (int i = 0; i < GameLevel; i++)//�������ڵ�ǰ�ȼ��Ľ���
    //    {
    //        foreach (var item in GameLevels[i].UnlockItems)
    //        {
    //            item.isLock = false;
    //        }
    //    }

    //}

    private List<ContentStruct> SaveContens()
    {
        List<ContentStruct> SaveContents = new List<ContentStruct>();
        ContentStruct contentStruct;
        foreach (var content in GameContents)
        {
            content.SaveContent(out contentStruct);
            SaveContents.Add(contentStruct);
        }
        return SaveContents;
    }

    private List<EnemySequenceStruct> SaveSequences()
    {
        List<EnemySequenceStruct> EStructs = new List<EnemySequenceStruct>();
        foreach (var sequences in WaveSystem.LevelSequence)
        {
            EnemySequenceStruct eStruct = new EnemySequenceStruct();
            eStruct.SequencesList = sequences;
            EStructs.Add(eStruct);
        }
        return EStructs;
    }

    private List<BlueprintStruct> SaveAllBlueprints()
    {
        List<BlueprintStruct> bluePrintStructList = new List<BlueprintStruct>();
        foreach (var grid in BluePrintShopUI.ShopBluePrints)
        {

            BlueprintStruct blueprintStruct = new BlueprintStruct();
            blueprintStruct.Name = grid.Strategy.Attribute.Name;
            blueprintStruct.ElementRequirements = new List<int>();
            blueprintStruct.QualityRequirements = new List<int>();
            for (int i = 0; i < grid.Strategy.Compositions.Count; i++)
            {
                blueprintStruct.ElementRequirements.Add(grid.Strategy.Compositions[i].elementRequirement);
                blueprintStruct.QualityRequirements.Add(grid.Strategy.Compositions[i].qualityRequeirement);

            }

            bluePrintStructList.Add(blueprintStruct);
        }
        return bluePrintStructList;
    }


    private void LoadGameSave()
    {
        if (LastGameSave.HasLastGame)
        {
            CurrentLevel = GetLevelAtt(LastGameSave.SaveRes.Mode);
            DeleteGameSave();
        }
    }


    public void LoadGame()
    {
        LoadByJson();
    }


    private void SaveByJson()
    {
        //if (NeedLoadData)
        //{
        //    try
        //    {
        //        LastDataSave.SaveData(SaveUnlockDIC());

        //        string filePath = Application.persistentDataPath + "/DataSave.json";
        //        Debug.Log(filePath);
        //        string saveJsonStr = JsonMapper.ToJson(LastDataSave);
        //        StreamWriter sw = new StreamWriter(filePath);
        //        sw.Write(saveJsonStr);
        //        sw.Close();
        //        Debug.Log("���ݳɹ��浵");

        //    }
        //    catch
        //    {
        //        Debug.LogWarning("���ݴ浵ʧ��");

        //    }
        //}

        if (NeedLoadGame)
        {
            if (!LevelEnd && CurrentLevel.CanSaveGame)
            {
                LastGameSave.SaveGame(SaveAllBlueprints(), GameRes.SaveRes, SaveContens(), SaveSequences());
                LevelEnd = true;//�Ƿ�����Ϸ״̬�ʹ浵��FLAG

                string filePath2 = Application.persistentDataPath + "/GameSave.json";
                Debug.Log(filePath2);
                string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
                StreamWriter sw2 = new StreamWriter(filePath2);
                sw2.Write(saveJsonStr2);
                sw2.Close();
                Debug.Log("ս���ɹ��浵");
            }
            else
            {
                LastGameSave.ClearGame();
            }
            GameContents.Clear();



            //try
            //{
            //    if (CurrentLevel.CanSaveGame && !LevelEnd)
            //        LastGameSave.SaveGame(SaveAllBlueprints(), GameRes.SaveRes, SaveContens(), SaveSequences());
            //    else
            //        LastGameSave.ClearGame();
            //    GameContents.Clear();

            //    string filePath2 = Application.persistentDataPath + "/GameSave.json";
            //    Debug.Log(filePath2);
            //    string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
            //    StreamWriter sw2 = new StreamWriter(filePath2);
            //    sw2.Write(saveJsonStr2);
            //    sw2.Close();
            //    Debug.Log("ս���ɹ��浵");
            //}
            //catch
            //{
            //    Debug.LogWarning("ս���浵ʧ��");
            //}

        }

    }

    private void LoadByJson()
    {
        //string filePath = Application.persistentDataPath + "/DataSave.json";
        //if (NeedLoadData && File.Exists(filePath))
        //{
        //    try
        //    {
        //        StreamReader sr = new StreamReader(filePath);
        //        string jsonStr = sr.ReadToEnd();
        //        sr.Close();
        //        SetGameLevel(GameLevel);
        //        //DataSave save = JsonMapper.ToObject<DataSave>(jsonStr);
        //        //LastDataSave = save;
        //        //LoadSaveData();
        //        Debug.Log("�ɹ���ȡ�浵");
        //    }
        //    catch
        //    {
        //        SetGameLevel(0);
        //        Debug.LogWarning("��ȡ�浵�����д���");
        //    }
        //}
        //else
        //{
        //    SetGameLevel(0);
        //    Debug.Log("û�пɶ�ȡ�浵");
        //}
        SetGameLevel(GameLevel);//���ڵȼ���������

        if (NeedLoadGame && File.Exists(SaveGameFilePath))
        {
            try
            {
                StreamReader sr = new StreamReader(SaveGameFilePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                GameSave save = JsonMapper.ToObject<GameSave>(jsonStr);
                LastGameSave = save;
                LoadGameSave();
                Debug.Log("�ɹ���ȡս��");
            }
            catch
            {
                LastGameSave = new GameSave();
                Debug.LogWarning("��ȡս�������д���");
            }

        }
        else
        {
            LastGameSave = new GameSave();
            Debug.Log("û�пɶ�ȡս��");
        }


    }

    public void SaveAll()
    {
        SaveByJson();
    }


    private void OnApplicationQuit()//��Ϸ����ʱ�˳���յ�
    {
        SaveAll();
    }
}
