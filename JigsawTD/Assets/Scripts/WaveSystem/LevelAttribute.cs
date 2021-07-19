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
    [TextArea(2, 3)]
    public string LevelInfo;
    public int bossForm;

}
