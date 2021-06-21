using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Enemy
{
    WaveSystem ws;
    BoardSystem bs;
    int dividingCount;
    [SerializeField]int dividing;
    [SerializeField]int springs;
    [SerializeField] Transform size;
    float scale = 1;



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
        if (StunTime >= 0)
        {
            StunTime -= Time.deltaTime;
            if (StunTime < 0)
                progressFactor = Speed * adjust;
        }
        progress += Time.deltaTime * progressFactor;

        if (!trapTriggered && progress >= 0.5f)
        {
            TriigerTrap();
        }

        while (progress >= 1f)
        {
            if (PointIndex == pathPoints.Count - 1)
            {
                StartCoroutine(ExitCor());
                return false;
            }
            trapTriggered = false;
            progress = 0;
            PrepareNextState();
        }
        if (DirectionChange == DirectionChange.None)
        {
            transform.localPosition = Vector3.LerpUnclamped(positionFrom, positionTo, progress);
        }
        else
        {
            float angle = Mathf.LerpUnclamped(directionAngleFrom, directionAngleTo, progress);
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
        }
        return true;
    }

    private void GetSprings()
    {
        if (dividingCount < Dividing)
        {
            scale *= 0.8f;
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
        enemy.Initialize(attribute, Random.Range(-0.3f, 0.3f), healthBar, intensify) ;
        enemy.Dividing = Dividing-1;
        enemy.SpawnOn(PointIndex, board.shortestPoints);
        enemy.scale = scale;
        enemy.MaxHealth = MaxHealth * scale;
        enemy.size.localScale = new Vector3(scale,scale,1);
        GameManager.Instance.enemies.Add(enemy);
    }
}
