using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armorer : Enemy
{
    [SerializeField] float armorIntensify; 
    float armor;
    public float Armor { get => armor; set => armor = value; }
    public float ArmorCoolDown { get => armorCoolDown; set => armorCoolDown = value; }

    [SerializeField] float armorCoolDown;
    public override EnemyType EnemyType => EnemyType.Armorer;

    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        armor = intensify * armorIntensify;
        Armor[] armors = GetComponentsInChildren<Armor>();
        for (int i = 0; i < armors.Length; i++)
        {
            armors[i].MaxHealth = armor;
            armors[i].CurrentHealth = armor;
            armors[i].ReArmor();
        }
    }
}
