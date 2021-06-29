using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Froster : Enemy
{
    Collider2D[] attachedResult = new Collider2D[20];
    [SerializeField] FrostEffect frostPrefab = default;
    [SerializeField] ParticalControl frostPartical = default;
    public float freezeRange;
    public float freezeTime;
    public override EnemyType EnemyType => EnemyType.Froster;

    public override void Awake()
    {
        anim = this.GetComponent<Animator>();
        explosionClip = Resources.Load<AudioClip>("Music/Effects/Sound_EnemyExplosionFrost");
    }

    public override bool GameUpdate()
    {
        if (IsDie)
        {
            FrostTurrets();
        }
        return base.GameUpdate();
    }

    private void FrostTurrets()
    {
        ReusableObject partical = ObjectPool.Instance.Spawn(frostPartical);
        partical.transform.position = transform.position;
        partical.transform.localScale = Vector3.one * freezeRange;
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position, freezeRange, attachedResult, LayerMask.GetMask(StaticData.TurretMask));
        for(int i = 0; i < hits; i++)
        {
            TurretContent turret = attachedResult[i].GetComponent<TurretContent>();
            FrostEffect frosteffect = ObjectPool.Instance.Spawn(frostPrefab) as FrostEffect;
            frosteffect.transform.position = attachedResult[i].transform.position;
            frosteffect.UnspawnAfterTime(freezeTime);
            turret.Frost(freezeTime);
        }
    }
}
