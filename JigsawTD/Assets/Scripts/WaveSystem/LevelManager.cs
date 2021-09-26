using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelAttribute[] Levels = default;
    [SerializeField] private int maxDifficutly = default;
    public int MaxDifficulty
    {
        get => Mathf.Min(maxDifficutly, PlayerPrefs.GetInt("MaxDifficulty", 0));
        set => maxDifficutly = value;
    }
    [SerializeField] private int selectedLevelId = default;
    public int SelectedLevelID { get => selectedLevelId; set => selectedLevelId = Mathf.Clamp(value, 0, MaxDifficulty); }
    public LevelAttribute CurrentLevel { get => Levels[SelectedLevelID]; }

}
