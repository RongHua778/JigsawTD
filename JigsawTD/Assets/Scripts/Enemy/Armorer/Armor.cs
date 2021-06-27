using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : DestructableObject
{
    [SerializeField]protected Armorer boss;
    float boxColliderX;
    float boxColliderY;

    public override void Awake()
    {
        base.Awake();
        Type = ObjectType.Armor;
        if (GetComponent<BoxCollider2D>())
        {
            boxColliderX = GetComponent<BoxCollider2D>().size.x;
            boxColliderY = GetComponent<BoxCollider2D>().size.y;
        }

    }

    protected virtual void Update()
    {
        if (IsDie)
        {
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = transform.position;
            //ObjectPool.Instance.UnSpawn(this);
            DisArmor();
            IsDie = false;
        }
    }
    protected virtual void DisArmor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        if (GetComponent<CircleCollider2D>()) GetComponent<CircleCollider2D>().radius = 0;
        if(GetComponent<BoxCollider2D>()) GetComponent<BoxCollider2D>().size=new Vector2(0,0);
    }

    protected virtual void ReArmor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        if (GetComponent<CircleCollider2D>()) GetComponent<CircleCollider2D>().radius = 0.6f;
        if (GetComponent<BoxCollider2D>()) GetComponent<BoxCollider2D>().size=new Vector2(boxColliderX,boxColliderY);
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
