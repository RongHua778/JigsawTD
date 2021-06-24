using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingArmor:Armor
{
    [SerializeField] Transform body;

    protected override void Update()
    {
        base.Update();
        transform.RotateAround(body.transform.position, Vector3.forward, 100 * Time.deltaTime);
    }
    protected override void DisArmor()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        GetComponent<CircleCollider2D>().radius = 0 ;
        Invoke("ReArmor",boss.ArmorCoolDown);
    }

}
