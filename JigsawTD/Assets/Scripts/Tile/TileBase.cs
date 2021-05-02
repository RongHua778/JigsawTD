using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : ReusableObject
{
    [SerializeField]
    Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }
    // Start is called before the first frame update

}
