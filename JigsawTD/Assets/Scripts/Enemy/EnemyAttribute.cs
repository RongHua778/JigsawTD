using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ScriptableObject
{
    public string EnemyName;
    public EnemyType EnemyType;
    public int InitCount;
    public float CountIncrease;
    public float Health;
    public float Speed;
    public float CoolDown;
    public Enemy Prefab;
    public int ReachDamage;
    public float extraHealth;
    public Sprite EnemyIcon;
    public Sprite EnemyEmptyIcon;
    public string Description;

}
