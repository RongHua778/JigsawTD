using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/EnemyFactory", fileName = "EnemyFactory")]
public class EnemyFactory : GameObjectFactory
{
    [SerializeField]
    float pathOffset = 0.4f;

    [SerializeField] Enemy prefab = default;
    [SerializeField] HealthBar healthBarPrefab = default;

    public Enemy Get()
    {
        Enemy instance = CreateInstance(prefab.gameObject).GetComponent<Enemy>();
        HealthBar healthInstance = CreateInstance(healthBarPrefab.gameObject).GetComponent<HealthBar>();

        instance.OriginFactory = this;
        instance.Initialize(Random.Range(-pathOffset, pathOffset), healthInstance);
        return instance;
    }
    public void Reclaim(Enemy enemy)
    {
        Debug.Assert(enemy.OriginFactory == this, "Wrong factory reclaimed!");
        enemy.OriginFactory = null;
        ObjectPool.Instance.UnSpawn(enemy.gameObject);
    }
}
