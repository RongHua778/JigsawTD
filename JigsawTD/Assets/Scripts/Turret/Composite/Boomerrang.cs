using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boomerrang : CompositeTurret
{
    float rotSpeed = -360;
    float RotSpeed { get => rotSpeed; set => rotSpeed = value; }
    private SelfBullet cannonBullet;
    protected override void RotateTowards()
    {

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
        CheckAngle = 360f;
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

        Vector2 dir = Target[0].transform.position - transform.position;
        Vector2 pos = (Vector2)transform.position + dir.normalized * Strategy.FinalRange;

        DOTween.To(() => RotSpeed, x => RotSpeed = x, -720, 1.5f).SetEase(Ease.OutCubic);
        cannonBullet.transform.DOMove(pos, 1.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(1.5f);
        DOTween.To(() => RotSpeed, x => RotSpeed = x, -360, 1.5f).SetEase(Ease.InCubic);
        cannonBullet.transform.DOMove(transform.position, 1.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(1.5f);
    }

}
