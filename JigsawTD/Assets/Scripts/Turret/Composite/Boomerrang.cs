using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boomerrang : CompositeTurret
{
    float rotSpeed = 60;
    float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    protected override void RotateTowards()
    {

    }

    public override bool GameUpdate()
    {
        base.GameUpdate();
        //SelfRotateControl();
        return true;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        rotTrans.rotation = Quaternion.identity;
        CheckAngle = 360f;
    }

    private void SelfRotateControl()
    {
        rotTrans.Rotate(Vector3.forward * RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(CannonSprite.transform.DOLocalMove(transform.up * 3f, 1.5f).SetRelative().SetEase(Ease.OutQuart));
        mySequence.Append(CannonSprite.transform.DOMove(transform.position, 1.5f).SetEase(Ease.InQuart));

    }

}
