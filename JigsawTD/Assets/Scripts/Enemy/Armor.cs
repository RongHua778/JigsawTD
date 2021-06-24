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
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<CircleCollider2D>().radius = 0;
    }

    protected virtual void ReArmor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        GetComponent<CircleCollider2D>().radius = 0.6f;
        CurrentHealth = MaxHealth;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            bullet.hit = true;
            CurrentHealth -= bullet.Damage;
            if (bullet.SputteringEffect != null)
            {
                ParticalControl effect = ObjectPool.Instance.Spawn(bullet.SputteringEffect) as ParticalControl;
                effect.transform.position = transform.position;
                effect.transform.localScale = Mathf.Max(0.4f, bullet.SputteringRange * 2) * Vector3.one;
                effect.PlayEffect();
            }
            bullet.ReclaimBullet();
        }
    }
}
