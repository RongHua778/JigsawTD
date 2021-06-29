using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingArmor : Armor
{
    [SerializeField] Transform body;

    protected override void Update()
    {
        base.Update();
        //transform.RotateAround(body.transform.position, Vector3.forward, 30 * Time.deltaTime);
    }

    protected override void DisArmor()
    {
        base.DisArmor();
        Invoke("ReArmor", boss.ArmorCoolDown);
    }

}
