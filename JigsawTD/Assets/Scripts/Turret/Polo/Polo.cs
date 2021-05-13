using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polo : Turret
{
    public override int AttackRange => 1;
    float selfRotSpeed = 10f;

    public override bool GameUpdate()
    {
        rotTrans.Rotate(Vector3.forward * selfRotSpeed * Time.deltaTime, Space.Self);
        return true;
    }


}
