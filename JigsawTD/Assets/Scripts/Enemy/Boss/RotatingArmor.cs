using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmor:DestructableObject
{
    [SerializeField] Transform body;
    [SerializeField] RotatingArmorBoss boss;
    public override void Awake()
    {
        base.Awake();
        MaxHealth = boss.ArmorIntensify;
        CurrentHealth = MaxHealth;
        Type = ObjectType.Armor;
    }

    public void Update()
    {
        if (IsDie)
        {
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = transform.position;
            //ObjectPool.Instance.UnSpawn(this);
            DisArmor();
        }
        transform.RotateAround(body.transform.position, Vector3.forward, 100 * Time.deltaTime);
    }
    private void DisArmor()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = new Color(0, 0, 0, 0);
        GetComponentInChildren<CircleCollider2D>().radius = 0 ;
        IsDie = false;
        Invoke("ReArmor",boss.ArmorCoolDown);
    }

    private void ReArmor()
    {
        GetComponentInChildren<SpriteRenderer>().material.color = new Color(1, 1, 1, 1);
        GetComponentInChildren<CircleCollider2D>().radius = 0.6f;
        CurrentHealth = MaxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bullet>())
        {
            Bullet bullet = collision.GetComponent<Bullet>();
            CurrentHealth -= bullet.Damage;
            bullet.hit = true;
            bullet.ReclaimBullet();
        }
    }
}
