using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall : Enemy
{
    public float freezeRange;
    public float freezeTime;
    public override DirectionChange DirectionChange
    {
        get => base.DirectionChange;
        set
        {
            base.DirectionChange = value;
            if (value != DirectionChange.None)
            {
                StaticData.Instance.FrostTurretEffect(model.position, freezeRange, freezeTime);
            }

        }
    }


}
