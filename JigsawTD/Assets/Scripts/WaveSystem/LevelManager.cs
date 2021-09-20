using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public LevelAttribute[] Levels = default;
    public int SelectedLevelID = 0;
    public LevelAttribute CurrentLevel { get => Levels[SelectedLevelID]; }

}
