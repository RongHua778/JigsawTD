using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TileBase : ReusableObject
{
    Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get => _offsetCoord; set => _offsetCoord = value; }

    private bool isLanded = false;//�Ƿ��ڰ�ͼ״̬
    public virtual bool IsLanded { get => isLanded; set => isLanded = value; }


}
