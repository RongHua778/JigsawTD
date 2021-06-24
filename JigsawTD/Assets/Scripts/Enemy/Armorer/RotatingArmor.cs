using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmor:Armor
{
    [SerializeField] Transform body;
    [SerializeField] RotatingArmorBoss boss;
    public override void Awake()
    {
        MaxHealth = boss.ArmorIntensify;
        base.Awake();
    }

    public void Update()
    {
        if (IsDie)
        {
            ReusableObject explosion = ObjectPool.Instance.Spawn(exlposionPrefab);
            Sound.Instance.PlayEffect(explosionClip, StaticData.Instance.EnvrionmentBaseVolume);
            explosion.transform.position = transform.position;
            //ObjectPool.Instance.UnSpawn(this);
            DisArmor();
        }
        transform.RotateAround(body.transform.position, Vector3.forward, 100 * Time.deltaTime);
    }
    protected override void DisArmor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<CircleCollider2D>().radius = 0 ;
        IsDie = false;
        Invoke("ReArmor",boss.ArmorCoolDown);
    }

}
