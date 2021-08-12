using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameSystem : MonoBehaviour
{
    public virtual void Initialize() { }
    public virtual void Release() { }
    public virtual void GameUpdate() { }
}
