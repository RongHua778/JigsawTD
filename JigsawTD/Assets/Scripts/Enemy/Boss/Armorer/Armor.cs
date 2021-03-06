using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour,IDamageable
{

    [SerializeField] ParticalControl explosionPrefab = default;
    [SerializeField] JumpDamage jumpDamagePrefab = default;
    protected AudioClip explosionClip;
    public bool IsEnemy { get => false; }

    private bool isDie;
    public bool IsDie { get => isDie; set => isDie = value; }
    float maxHealth;
    float currentHealth;
    public float CurrentHealth 
    { get => currentHealth;
        set 
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                ReusableObject explosion = ObjectPool.Instance.Spawn(explosionPrefab);
                Sound.Instance.PlayEffect(explosionClip);
                explosion.transform.position = transform.position;
                DisArmor();
            }
        } 
    }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }


    private void Awake()
    {
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosion");

    }


    protected virtual void DisArmor()
    {
        transform.localScale = Vector3.zero;

    }

    public virtual void ReArmor()
    {
        transform.localScale = Vector3.one;
        CurrentHealth = MaxHealth;

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();
        if (bullet!=null)
        {
            //if (bullet.hit)
            //    return;
            //bullet.hit = true;
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

    public void ApplyDamage(float amount, out float realDamage, bool isCritical = false)
    {
        realDamage = amount;
        CurrentHealth -= realDamage;
        GameEndUI.TotalDamage += (int)realDamage;

        if (isCritical)
        {
            JumpDamage obj = ObjectPool.Instance.Spawn(jumpDamagePrefab) as JumpDamage;
            obj.Jump((int)realDamage, transform.position);
        }

    }


}
