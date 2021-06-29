using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Enemy
{
    WaveSystem ws;
    BoardSystem bs;
    int dividingCount;
    [SerializeField] int dividing;
    [SerializeField] int springs;
    [SerializeField] Transform size;
    float DivierIntensify = 1;
    [SerializeField] Sprite[] dividerSprites = default;
    [SerializeField] SpriteRenderer EnemySprite = default;



    public override EnemyType EnemyType => EnemyType.Divider;

    public int Dividing { get => dividing; set => dividing = value; }

    public override void Awake()
    {
        base.Awake();
        ws = GameManager.Instance.WaveSystem;
        bs = GameManager.Instance.BoardSystem;
    }
    public override bool GameUpdate()
    {
        if (IsDie)
        {
            GetSprings();
            StopAllCoroutines();
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = model.transform.position;
            GameEvents.Instance.EnemyDie(this);
            ObjectPool.Instance.UnSpawn(this);
            return false;
        }
        return base.GameUpdate();
    }

    private void GetSprings()
    {
        if (dividingCount < Dividing)
        {
            DivierIntensify *= 0.5f;
            for (int i = 0; i < springs; i++)
            {
                ws.EnemyRemain += 1;
                SpawnEnemy(bs);
            }
            dividingCount += 1;
        }
    }
    private void SpawnEnemy(BoardSystem board)
    {
        EnemyAttribute attribute = GameManager.Instance.EnemyFactory.Get(EnemyType.Divider);
        float intensify = ws.RunningSequence.Intensify;
        Divider enemy = ObjectPool.Instance.Spawn(attribute.Prefab) as Divider;
        HealthBar healthBar = ObjectPool.Instance.Spawn(ws.HealthBarPrefab) as HealthBar;
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify);
        enemy.Dividing = Dividing - 1;
        enemy.EnemySprite.sprite = dividerSprites[Dividing - 1];
        enemy.DivierIntensify = DivierIntensify;
        enemy.MaxHealth = MaxHealth * DivierIntensify;
        enemy.EnemySprite.GetComponent<CircleCollider2D>().radius = 0.4f - 0.1f * (3 - Dividing);
        enemy.SpawnOn(PointIndex, board.shortestPoints);
        GameManager.Instance.enemies.Add(enemy);
        enemy.progress = Mathf.Clamp((progress + Random.Range(-0.2f, 0.2f)), 0, 1);

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        Dividing = 2;
        dividingCount = 0;
    }
}
