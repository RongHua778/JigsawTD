using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IDamage
{
    public string ExplosionSound => "Sound_EnemyExplosion";
    [SerializeField] private ReusableObject ExplosionPrefab = default;

    public HealthBar HealthBar { get; set; }

    public DamageStrategy DamageStrategy { get; set; }



    private void Awake()
    {
        DamageStrategy = new ArmourStrategy(this);
        HealthBar = transform.Find("HealthBarSmall").GetComponent<HealthBar>();
    }


    public virtual void DisArmor()
    {
        transform.localScale = Vector3.zero;
        ReusableObject explosion = ObjectPool.Instance.Spawn(ExplosionPrefab);
        explosion.transform.position = transform.position;
        Sound.Instance.PlayEffect(ExplosionSound);
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
            bullet.DealRealDamage(this, transform.position, true);
            GameManager.Instance.nonEnemies.Remove(bullet);
            bullet.ReclaimBullet();

            ParticalControl effect = ObjectPool.Instance.Spawn(bullet.SputteringEffect) as ParticalControl;
            effect.transform.position = transform.position;
            effect.transform.localScale = Vector3.one * 0.3f;
            effect.PlayEffect();
        }

    }


}
