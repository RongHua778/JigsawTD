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

    #region 基础保存
    public GameLevelInfo[] GameLevels = default;

    public int GameLevel { get => PlayerPrefs.GetInt("GameLevel", 0); set => PlayerPrefs.SetInt("GameLevel", Mathf.Clamp(value, 0, GameLevels.Length - 1)); }
    public int GameExp { get => PlayerPrefs.GetInt("GameExp", 0); set => PlayerPrefs.SetInt("GameExp", value); }

    public Dictionary<string, bool> UnlockInfoDIC;
    public List<ContentAttribute> AllContent;

    public LevelAttribute[] StandardLevels = default;
    //public LevelAttribute EndlessLevel = default;
    private Dictionary<int, LevelAttribute> LevelDIC;

    public int PremitDifficulty = default;
    public int PassDiifcutly
    {
        get => PlayerPrefs.GetInt("MaxDifficulty", 0);
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

    [SerializeField] private int maxDifficutly = default;
    public int MaxDifficulty
    {
        get => maxDifficutly;
        set => maxDifficutly = value;
    }
    [SerializeField] private int selectDifficulty = default;
    public int SelectDiffculty { get => selectDifficulty; set => selectDifficulty = Mathf.Clamp(value, 0, PassDiifcutly); }

    public LevelAttribute CurrentLevel;
    #endregion

    #region 临时游戏保存
    [Header("是否解锁全内容")]
    [SerializeField] bool UnlockAll = default;//测试用
    [Header("是否读取存档")]
    [SerializeField] bool NeedLoadGame = default;
    [Header("是否有未完成游戏")]
    public bool HasLastGame;
    public DataSave LastDataSave;
    public GameSave LastGameSave;
    public List<GameTileContent> GameContents;
    #endregion

    public void Initialize()
    {
        LitJsonRegister.Register();

        LevelDIC = new Dictionary<int, LevelAttribute>();
        GameContents = new List<GameTileContent>();
        foreach (var level in StandardLevels)
        {
            LevelDIC.Add(level.Mode, level);
        }
        SetUnlockAll(UnlockAll);

    }

    public LevelAttribute GetLevelAtt(int mode)
    {
        if (LevelDIC.ContainsKey(mode))
        {
            return LevelDIC[mode];
        }
        else
        {
            Debug.LogWarning("错误的模式代码");
            return null;
        }
    }


    public void SetUnlockAll(bool value)//测试用
    {
        if (value)//解锁全内容
        {
            GameLevel = GameLevels.Length - 1;
            GameExp = GameLevels[GameLevels.Length - 1].ExpRequire;
            foreach (var item in AllContent)
            {
                item.isLock = false;
            }
        }
        else
        {
            foreach (var item in AllContent)
            {
                item.isLock = item.initialLock;
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
        if (GameLevel >= GameLevels.Length - 1)//达到最大等级后，只加经验不升级
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

    //存档管理
    private void LoadSaveData()
    {
        foreach (var item in AllContent)//根据存档解锁内容，如果不包含新内容，则默认锁定
        {
            if (LastDataSave.UnlockInfoDIC.ContainsKey(item.Name))
            {
                item.isLock = LastDataSave.UnlockInfoDIC[item.Name];
            }
            else
            {
                item.isLock = true;
            }
        }
        for (int i = 0; i < GameLevel; i++)//解锁低于当前等级的奖励
        {
            foreach (var item in GameLevels[i].UnlockItems)
            {
                item.isLock = false;
            }
        }

    }

    private void SaveData()
    {
        //解锁情况
        UnlockInfoDIC = new Dictionary<string, bool>();
        foreach (var item in AllContent)
        {
            UnlockInfoDIC.Add(item.Name, item.isLock);
        }
        LastDataSave.UnlockInfoDIC = UnlockInfoDIC;
    }

    private void SaveGame()
    {
        if (GameContents.Count <= 0)
            return;
        LastGameSave.SaveRes = GameRes.SaveRes;
        LastGameSave.SaveBluePrints = SaveAllBlueprints();
        LastGameSave.SaveContents = SaveContens();

    }

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

    private void FirstGameData()
    {
        GameLevel = 0;
        GameExp = 0;
        foreach (var content in AllContent)
        {
            content.isLock = content.initialLock;
        }
    }

    private void LoadGameSave()
    {
        if (!NeedLoadGame)
            return;
        if (LastGameSave.SaveContents != null)
        {
            CurrentLevel = GetLevelAtt(LastGameSave.SaveRes.Mode);
            HasLastGame = true;
        }
        else
        {
            HasLastGame = false;
        }
    }


    public void LoadGame()
    {
        LoadByJson();
    }

    public void ClearLastData()//结束一局游戏和重新开始时，调用
    {
        HasLastGame = false;
        LastGameSave = new GameSave();
        GameContents.Clear();
    }
    private void SaveByJson()
    {
        try
        {
            SaveData();
            string filePath = Application.persistentDataPath + "/DataSave.json";
            Debug.Log(filePath);
            string saveJsonStr = JsonMapper.ToJson(LastDataSave);
            StreamWriter sw = new StreamWriter(filePath);
            sw.Write(saveJsonStr);
            sw.Close();
            Debug.Log("数据成功存档");

        }
        catch
        {
            Debug.LogWarning("数据存档失败");

        }
        SaveGame();
        string filePath2 = Application.persistentDataPath + "/GameSave.json";
        Debug.Log(filePath2);
        string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
        StreamWriter sw2 = new StreamWriter(filePath2);
        sw2.Write(saveJsonStr2);
        sw2.Close();
        Debug.Log("战斗成功存档");
        //try
        //{
        //    SaveGame();
        //    string filePath2 = Application.persistentDataPath + "/GameSave.json";
        //    Debug.Log(filePath2);
        //    string saveJsonStr2 = JsonMapper.ToJson(LastGameSave);
        //    StreamWriter sw2 = new StreamWriter(filePath2);
        //    sw2.Write(saveJsonStr2);
        //    sw2.Close();
        //    Debug.Log("战斗成功存档");

        //}
        //catch
        //{
        //    Debug.LogWarning("战斗存档失败");

        //}



    }

    private void LoadByJson()
    {
        string filePath = Application.persistentDataPath + "/DataSave.json";
        if (File.Exists(filePath))
        {
            try
            {
                StreamReader sr = new StreamReader(filePath);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                DataSave save = JsonMapper.ToObject<DataSave>(jsonStr);
                LastDataSave = save;
                LoadSaveData();
                Debug.Log("成功读取存档");
            }
            catch
            {
                LastDataSave = new DataSave();
                Debug.LogWarning("读取存档数据有错误");
            }

        }
        else
        {
            FirstGameData();
            Debug.Log("没有可读取存档");
        }

        string filePath2 = Application.persistentDataPath + "/GameSave.json";
        if (File.Exists(filePath2))
        {
            try
            {
                StreamReader sr = new StreamReader(filePath2);
                string jsonStr = sr.ReadToEnd();
                sr.Close();
                GameSave save = JsonMapper.ToObject<GameSave>(jsonStr);
                LastGameSave = save;
                LoadGameSave();
                Debug.Log("成功读取战斗");
            }
            catch
            {
                LastGameSave = new GameSave();
                Debug.LogWarning("读取战斗数据有错误");
            }

        }
        else
        {
            LastGameSave = new GameSave();
            Debug.Log("没有可读取战斗");
        }
    }

    public void SaveAll()
    {
        SaveByJson();
        ClearLastData();
    }


    private void OnApplicationQuit()
    {
        SaveByJson();
    }
}
