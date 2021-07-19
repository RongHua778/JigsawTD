using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/LevelAttribute", fileName = "LevelAttribute")]
public class LevelAttribute : ScriptableObject
{
    public int LevelID;
    public float LevelIntensify;
    public List<EnemyAttribute> NormalEnemies;
    public List<EnemyAttribute> Boss;
    public string LevelInfo;


}
