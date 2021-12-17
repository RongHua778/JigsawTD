using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borner : Boss
{
    public override string ExplosionEffect => "BossExplosionPurple";


    [SerializeField] float[] bornCD=default;
    [SerializeField] int[] enemyOneBorn=default;
    int level;
    int form;
    float castleCounter;
    float bornCounter;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        level = 0;
        form = 0;
    }

    protected override void OnEnemyUpdate()
    {
        base.OnEnemyUpdate();
        bornCounter += Time.deltaTime;
        if (bornCounter > bornCD[level])
        {
            Born();
            bornCounter = 0;
        }
        Castle();
    }

    private void Born()
    {
        for (int i = 0; i < enemyOneBorn[level]; i++)
        {
            int typeInt = Random.Range(0, 6);
            GameManager.Instance.SpawnEnemy((EnemyType)typeInt, PointIndex, Intensify / 2);
        }
    }


    private void Castle()
    {
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 0.7f && form == 0)
        {
            DamageStrategy.BuffDamageIntensify -= 0.8f;
            level = 1;
            form = 1;
            DamageStrategy.StunTime += 7f;
            castleCounter = 7f;
            anim.SetBool("Transform", true);
            Sound.Instance.PlayEffect("Sound_BornerTransform");
        }
        if (DamageStrategy.CurrentHealth / DamageStrategy.MaxHealth <= 0.3f && form == 1)
        {
            DamageStrategy.BuffDamageIntensify -= 0.8f;
            level = 2;
            form = 2;
            DamageStrategy.StunTime += 7f;
            castleCounter = 7f;
            anim.SetBool("Transform", true);
            Sound.Instance.PlayEffect("Sound_BornerTransform");

        }
        if (castleCounter > 0)
        {
            castleCounter -= Time.deltaTime;
            if (castleCounter <= 0f)
            {
                level = 0;
                DamageStrategy.BuffDamageIntensify += 0.8f;
                anim.SetBool("Transform", false);
                Sound.Instance.PlayEffect("Sound_BornerTransform");
            }
        }

    }
}
