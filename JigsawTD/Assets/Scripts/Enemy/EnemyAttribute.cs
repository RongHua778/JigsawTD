using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ScriptableObject
{
    public EnemyType EnemyType;
    public int CountPerWave;
    public float Health;
    public float Speed;
    public int Shell;
    public float CoolDown;
    public Enemy Prefab;

}
