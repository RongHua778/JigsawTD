using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingArmorBoss : Armorer
{
    public override EnemyType EnemyType => EnemyType.SixArmor;
    private Tween tween;

    [SerializeField] Transform rotateObj = default;


    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify);
        Vector3 rot = new Vector3(0, 0, 360);
        tween = rotateObj.DORotate(rot, 6f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        tween.Kill();
    }



}
