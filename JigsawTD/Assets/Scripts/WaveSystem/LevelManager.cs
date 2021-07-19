using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelAttribute[] Levels = default;
    public int SelectedLevelID = 1;
    public LevelAttribute CurrentLevel { get => Levels[SelectedLevelID - 1]; }
    public int LevelMaxTurn
    {
        get => PlayerPrefs.GetInt(SelectedLevelID + "LevelHigh", 0);
        set => PlayerPrefs.SetInt(SelectedLevelID + "LevelHigh", value);
    }

}
