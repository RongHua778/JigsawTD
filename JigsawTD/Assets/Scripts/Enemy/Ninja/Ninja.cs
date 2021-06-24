using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Enemy
{
    [SerializeField] float maxSpeed;
    [SerializeField] float minSpeed;
    public override EnemyType EnemyType => EnemyType.Ninja;

    float size;
    public override float CurrentHealth 
    { 
        get => base.CurrentHealth; 
        set
        {
            base.CurrentHealth = value;
            Speed = minSpeed + maxSpeed * (1 - CurrentHealth / MaxHealth);
            progressFactor = Speed * adjust;
            size = 0.8f + CurrentHealth / MaxHealth;
            model.transform.localScale = new Vector3(size,size,1);
        }
    }
}