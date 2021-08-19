using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatingArmor : Armor
{

    public override void DisArmor()
    {
        base.DisArmor();
        //Invoke("ReArmor", boss.ArmorCoolDown);
    }

}
