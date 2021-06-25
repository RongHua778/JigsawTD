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

            size = 0.8f + CurrentHealth / MaxHealth;
            model.transform.localScale = new Vector3(size,size,1);
        }
    }

    public override bool GameUpdate()
    {
        Speed = minSpeed + maxSpeed * ((float)PointIndex / (float)pathPoints.Count);
        progressFactor = Speed * adjust;
        return base.GameUpdate();
    }
}
