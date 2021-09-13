using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTrap : TrapContent
{
    [SerializeField] ParticalControl explisionPrefab = default;
    private float sputteringRange = 0.5f;
    public override void OnContentPass(Enemy enemy, GameTileContent content = null)
    {
        base.OnContentPass(enemy);
        float realDamage;
        float damage = TrapIntensify * enemy.EnemyTrapIntensify * 0.2f * (enemy.DamageStrategy.MaxHealth - enemy.DamageStrategy.CurrentHealth);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, sputteringRange, StaticData.EnemyLayerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            TargetPoint target = hits[i].GetComponent<TargetPoint>();
            if (target)
            {
                target.Enemy.DamageStrategy.ApplyDamage(damage, out realDamage, true);
                DamageAnalysis += (int)realDamage;
            }
        }
        ParticalControl effect = ObjectPool.Instance.Spawn(explisionPrefab) as ParticalControl;
        effect.transform.position = transform.position;
        effect.transform.localScale = Mathf.Max(0.3f, sputteringRange * 2) * Vector3.one;
        effect.PlayEffect();
        Sound.Instance.PlayEffect("Sound_ExplosionTrap");
    }


}
