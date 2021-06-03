using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : ReusableObject
{
    Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    // Start is called before the first frame update

    private bool isActive = true;//是否处于激活状态
    public virtual bool IsActive { get => isActive; set => isActive = value; }


}
