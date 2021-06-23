using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTurret : ElementTurret
{
    private bool isPlayingAudio = false;

    //public override float SputteringRange => 0;

    public override void OnSpawn()
    {
        base.OnSpawn();
        _rotSpeed = 0f;
        CheckAngle = 45f;
    }

    public override bool GameUpdate()
    {
        if (targetList.Count == 0)
        {
            if (isPlayingAudio)
            {
                turretAnim.SetBool("Attacking", false);
                ShootEffect.Stop();
                isPlayingAudio = false;
                audioSource.Stop();
            }
        }
        else
        {
            if (!isPlayingAudio)
            {
                ShootEffect.Play();
                turretAnim.SetBool("Attacking", true);
                isPlayingAudio = true;
                PlayAudio(ShootClip, true);
            }
        }
        return base.GameUpdate();

    }
    protected override void Shoot()
    {
        foreach (var target in Target)
        {
            Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
            float offset = Random.Range(-0.02f, 0.02f);
            bullet.transform.position = shootPoint.position + offset * shootPoint.right;
            Vector2 dir = bullet.transform.position - transform.position;
            Vector2 pos = (Vector2)shootPoint.position + dir.normalized * Strategy.AttackRange;
            bullet.Initialize(this, target, pos);
        }
    }



}
