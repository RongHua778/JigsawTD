using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
    void ApplyDamage(float amount, out float realDamage, bool isCritical = false);

    public bool IsDie { get; set; }
    public bool IsEnemy { get; }
}
