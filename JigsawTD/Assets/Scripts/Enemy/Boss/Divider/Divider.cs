using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Divider : Boss
{
    [SerializeField] Sprite[] DividerSprites = default;
    public int dividing;
    public int springs;
    public override EnemyType EnemyType => EnemyType.Divider;

    public override void Initialize(int pathIndex,EnemyAttribute attribute, float pathOffset,float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset,intensify);
        dividing = 2;
    }

    protected override void OnDie()
    {
        base.OnDie();

        if (!isOutTroing)
            GetSprings();
    }

    private void GetSprings()
    {
        if (dividing > 0)
        {
            for (int i = 0; i < springs; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void LateUpdate()
    {
        enemySprite.sprite = DividerSprites[dividing];
    }
    private void SpawnEnemy()
    {
        Divider divider = GameManager.Instance.SpawnEnemy(EnemyType, PointIndex, Intensify / 2) as Divider;
        divider.dividing = dividing - 1;
        divider.Progress = Mathf.Clamp((Progress + Random.Range(-0.2f, 0.2f)), 0, 1);
        divider.enemyCol.radius = 0.4f - 0.1f * (3 - divider.dividing);
        divider.ReachDamage = divider.dividing + 1;

    }
}
