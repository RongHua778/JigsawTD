using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLevelInfo
{
    public int ExpRequire;
    public ContentAttribute[] UnlockItems;
}

public class LevelManager : Singleton<LevelManager>
{
    public GameLevelInfo[] GameLevels = default;

    private int gameLevel = 0;
    private int gameExp = 0;
    public int GameLevel { get => gameLevel; set => gameLevel = value; }
    public int GameExp { get => gameExp; set => gameExp = value; }

    public Dictionary<string, bool> UnlockInfoDIC;
    public List<ContentAttribute> AllContent;

    public LevelAttribute[] StandardLevels = default;
    public LevelAttribute EndlessLevel = default;

    public int PremitDifficulty = default;
    public int PassDiifcutly { get => PlayerPrefs.GetInt("MaxDifficulty", 0); set => PlayerPrefs.SetInt("MaxDifficulty", Mathf.Min(5, value)); }

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

    public bool LevelStart()
    {
        if (SelectDiffculty > PremitDifficulty)
        {
            MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("UNPERMIT"));
            return false;
        }
        else if (SelectDiffculty > PassDiifcutly)
        {
            MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("UNPASS"));
            return false;
        }
        CurrentLevel = StandardLevels[SelectDiffculty];
        if (CurrentLevel.Difficulty > 1)
            Game.Instance.Tutorial = false;
        return true;
    }


    public void LoadSaveData(Save save)
    {
        GameLevel = save.GameLevel;
        GameExp = save.GameExp;
        foreach (var item in AllContent)//根据存档解锁内容，如果不包含新内容，则默认锁定
        {
            if (save.UnlockInfoDIC.ContainsKey(item.Name))
            {
                item.isLock = save.UnlockInfoDIC[item.Name];
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

    public Save SetSaveData()
    {
        Save save = new Save();
        save.GameLevel = GameLevel;
        save.GameExp = GameExp;
        UnlockInfoDIC = new Dictionary<string, bool>();
        foreach (var item in AllContent)
        {
            UnlockInfoDIC.Add(item.Name, item.isLock);
        }
        save.UnlockInfoDIC = UnlockInfoDIC;
        return save;
    }

    public void FirstGameData()
    {
        GameLevel = 0;
        GameExp = 0;
        foreach (var content in AllContent)
        {
            content.isLock = content.initialLock;
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
        if (GameLevel >= GameLevels.Length)
            return;
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
}
