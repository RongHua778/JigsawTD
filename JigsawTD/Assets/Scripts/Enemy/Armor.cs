using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : DestructableObject
{

    public override void Awake()
    {
        base.Awake();
        CurrentHealth = MaxHealth;
        Type = ObjectType.Armor;
    }
    protected virtual void DisArmor()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = new Color(0, 0, 0, 0);
        GetComponentInChildren<CircleCollider2D>().radius = 0;
    }

    protected virtual void ReArmor()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);
        GetComponentInChildren<CircleCollider2D>().radius = 0.6f;
        CurrentHealth = MaxHealth;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Debug.LogWarning("hit!");
            Bullet bullet = collision.GetComponent<Bullet>();
            CurrentHealth -= bullet.Damage;
            bullet.TriggerDamage();
            bullet.ReclaimBullet();
        }
    }
}
