using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polo : TurretContent
{
    public override int AttackRange => 1;

    public override GameTileContentType ContentType => throw new System.NotImplementedException();

    float selfRotSpeed = 10f;

    public override bool GameUpdate()
    {
        rotTrans.Rotate(Vector3.forward * selfRotSpeed * Time.deltaTime, Space.Self);
        return true;
    }


}
