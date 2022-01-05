using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armorer : Boss
{
    [SerializeField] float armorIntensify; 
    public float Armor { get; set; }

    public override void Initialize(int pathIndex,EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        Armor = intensify * armorIntensify;
        Armor[] armors = GetComponentsInChildren<Armor>();
        for (int i = 0; i < armors.Length; i++)
        {
            armors[i].DamageStrategy.MaxHealth=Armor;
            armors[i].DamageStrategy.IsDie = false;
            armors[i].ReArmor();
        }
    }
}
