using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boomerrang : RefactorTurret
{
    float rotSpeed = -360;
    private float flyTime;
    public float FlyTime => 1.5f / (Strategy.FinalFireRate / 0.2f);
    float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    private SelfBullet cannonBullet;

    protected override void RotateTowards()
    {

    }

    protected override bool AngleCheck()//回旋镖必须360度确认
    {
        var angleCheck = Quaternion.Angle(rotTrans.rotation, look_Rotation);
        if (angleCheck < 360f)
        {
            return true;
        }
        return false;
    }

    public override bool GameUpdate()
    {
        base.GameUpdate();
        SelfRotateControl();
        return true;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        CannonSprite.transform.rotation = Quaternion.identity;
        cannonBullet = CannonSprite.GetComponent<SelfBullet>();
    }

    private void SelfRotateControl()
    {
        cannonBullet.transform.Rotate(Vector3.forward * RotSpeed * Time.deltaTime, Space.Self);
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootCor());
    }

    private IEnumerator ShootCor()
    {
        cannonBullet.Initialize(this);

        Vector2 dir;
        Vector2 pos;

        if (Strategy.RangeType == RangeType.Line)
        {
            pos = (Vector2)transform.position + (Vector2)transform.up * Strategy.FinalRange;
        }
        else
        {
            dir = Target[0].transform.position - transform.position;
            pos = (Vector2)transform.position + dir.normalized * Strategy.FinalRange;
        }

        DOTween.To(() => RotSpeed, x => RotSpeed = x, -720, FlyTime).SetEase(Ease.OutCubic);
        cannonBullet.transform.DOMove(pos, FlyTime).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(FlyTime);
        DOTween.To(() => RotSpeed, x => RotSpeed = x, -360, FlyTime).SetEase(Ease.InCubic);
        cannonBullet.transform.DOMove(transform.position, FlyTime).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(FlyTime);
    }

}
