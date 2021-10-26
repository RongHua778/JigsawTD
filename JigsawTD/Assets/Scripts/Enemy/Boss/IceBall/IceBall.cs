using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IceBall : Boss
{
    public override string ExplosionEffect => "BossExplosionBlue";

    public float freezeRange;
    public float freezeTime;
    private Tween tween;
    [SerializeField] Transform rotateObj = default;
    public override void Initialize(int pathIndex, EnemyAttribute attribute, float pathOffset, float intensify)
    {
        base.Initialize(pathIndex, attribute, pathOffset, intensify);
        Vector3 rot = new Vector3(0, 0, 360);
        tween = rotateObj.DORotate(rot, 4f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();
    }
    public override DirectionChange DirectionChange
    {
        get => base.DirectionChange;
        set
        {
            base.DirectionChange = value;
            if (value != DirectionChange.None)
            {
                StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);
            }
        }
    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        tween.Kill();
    }


}
