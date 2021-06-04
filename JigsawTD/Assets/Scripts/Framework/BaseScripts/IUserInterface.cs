using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInterface : MonoBehaviour
{
    protected GameManager m_GameManager;
    [SerializeField] protected GameObject m_RootUI;
    protected bool m_Active = true;

    public virtual void Initialize(GameManager gameManager)
    {
        m_GameManager = gameManager;
        m_RootUI = transform.Find("Root").gameObject;
    }
    public bool IsVisible()
    {
        return m_Active;
    }
    public virtual void Show()
    {
        m_RootUI.SetActive(true);
        m_Active = true;
    }

    public virtual void Hide()
    {
        m_RootUI.SetActive(false);
        m_Active = false;
    }

    public virtual void Release() { }
    public virtual void Update() { }
}
