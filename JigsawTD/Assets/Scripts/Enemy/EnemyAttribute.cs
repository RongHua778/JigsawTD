using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attribute/EnemyAttribute", fileName = "EnemyAttribute")]
public class EnemyAttribute : ScriptableObject
{
    public float Health;
    public float Speed;
    public int Shell;

}
