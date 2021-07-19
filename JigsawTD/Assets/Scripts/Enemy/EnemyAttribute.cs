using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ScriptableObject
{
    public string EnemyName;
    public EnemyType EnemyType;
    public int InitCount;
    public int CountIncrease;
    public float Health;
    public float Speed;
    public int Shell;
    public float CoolDown;
    public Enemy Prefab;
    public int ReachDamage;
    public float extraHealth;
    public Sprite EnemyIcon;
    public Sprite EnemyEmptyIcon;
    [TextArea(2,3)]
    public string Description;
}
