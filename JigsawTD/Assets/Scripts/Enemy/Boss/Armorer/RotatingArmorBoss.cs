using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingArmorBoss : Armorer
{
    private Tween tween;

   // [SerializeField] Transform rotateObj = default;


    public override void Initialize(EnemyAttribute attribute, float pathOffset, HealthBar healthBar, float intensify, List<BasicTile> path)
    {
        base.Initialize(attribute, pathOffset, healthBar, intensify,path);
        //Vector3 rot = new Vector3(0, 0, 360);
        //tween = rotateObj.DORotate(rot, 12f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetRelative();

    }

    public override void OnUnSpawn()
    {
        base.OnUnSpawn();
        //tween.Kill();
    }



}
