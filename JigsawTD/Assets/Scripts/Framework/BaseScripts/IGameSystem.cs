using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameSystem : MonoBehaviour
{
    protected GameManager m_GameManager = null;
    public virtual void Initialize(GameManager gameManager)
    {
        m_GameManager = gameManager;
    }
    public virtual void Release() { }
    public virtual void Update() { }
}
