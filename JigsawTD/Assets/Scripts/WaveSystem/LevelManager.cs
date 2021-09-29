using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelAttribute[] Levels = default;
    public int PremitDifficulty = default;

    public int PassDiifcutly => PlayerPrefs.GetInt("MaxDifficulty", 0);
    [SerializeField] private int maxDifficutly = default;
    public int MaxDifficulty
    {
        get => maxDifficutly;
        set => maxDifficutly = value;
    }
    [SerializeField] private int selectedLevelId = default;
    public int SelectedLevelID { get => selectedLevelId; set => selectedLevelId = Mathf.Clamp(value, 0, MaxDifficulty); }
    public LevelAttribute CurrentLevel { get => Levels[SelectedLevelID]; }


    public void UnlockBonus()
    {
        for (int i = 0; i < PassDiifcutly; i++)
        {
            foreach (var bo in Levels[i].Bonus)
            {
                Game.Instance.SaveData.UnlockBonus(bo.Name);
            }
        }
    }
}
