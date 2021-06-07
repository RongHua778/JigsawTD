using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : ReusableObject
{
    Vector2Int _offsetCoord;
    public Vector2Int OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    private bool isLanded = false;//ÊÇ·ñ´¦ÓÚ°æÍ¼×´Ì¬
    public virtual bool IsLanded { get => isLanded; set => isLanded = value; }


}
