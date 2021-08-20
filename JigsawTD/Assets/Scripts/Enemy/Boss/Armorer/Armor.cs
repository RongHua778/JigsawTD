using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour,IDamageable
{

    public ParticalControl explosionPrefab = default;
    public AudioClip explosionClip;
    public DamageStrategy DamageStrategy { get; set; }
    public bool IsDie { get; set; }


    private void Awake()
    {
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");
        DamageStrategy = new ArmourStrategy(this.gameObject, this);
    }


    public virtual void DisArmor()
    {
        transform.localScale = Vector3.zero;

    }

    public virtual void ReArmor()
    {
        transform.localScale = Vector3.one;
        DamageStrategy.CurrentHealth = DamageStrategy.MaxHealth;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.TriggerPreHitEffect();
            bullet.DealRealDamage(this);
            GameManager.Instance.nonEnemies.Remove(bullet);
            bullet.ReclaimBullet();

            ParticalControl effect = ObjectPool.Instance.Spawn(bullet.SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = Vector3.one * 0.3f;
            effect.PlayEffect();
        }

    }


}
