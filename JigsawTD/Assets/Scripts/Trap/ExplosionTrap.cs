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
        Vector2 pos = content == null ? transform.position : content.transform.position;
        float realDamage;
        float damage = TrapIntensify * enemy.DamageStrategy.TrapIntensify * 0.05f * enemy.DamageStrategy.CurrentHealth;
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos, sputteringRange, StaticData.EnemyLayerMask);
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
        effect.transform.position = pos;
        effect.transform.localScale = Mathf.Max(0.3f, sputteringRange * 2) * Vector3.one;
        effect.PlayEffect();
        Sound.Instance.PlayEffect("Sound_ExplosionTrap");
        enemy.DamageStrategy.TrapIntensify = 1;
    }


}
