using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBehavior : ReusableObject
{
    public virtual bool GameUpdate() => true;

}
