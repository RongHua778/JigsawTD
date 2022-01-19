using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodTurret : ElementTurret
{
    private bool isPlayingAudio = false;


    public override bool GameUpdate()
    {
        if (Activated)
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
        }
        else
        {
            if (isPlayingAudio)
            {
                turretAnim.SetBool("Attacking", false);
                ShootEffect.Stop();
                isPlayingAudio = false;
                audioSource.Stop();
            }
        }
        return base.GameUpdate();

    }
    protected override void Shoot()
    {
        Bullet bullet = ObjectPool.Instance.Spawn(this.bulletPrefab).GetComponent<Bullet>();
        float offset = Random.Range(-0.02f, 0.02f);
        bullet.transform.position = (Vector2)(shootPoint.position + offset * shootPoint.right);
        Vector2 dir = bullet.transform.position - transform.position;
        Vector2 pos = (Vector2)shootPoint.position + dir.normalized * Strategy.FinalRange;
        bullet.Initialize(this, Target[0], pos);
    }



}
