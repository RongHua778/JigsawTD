using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeToggle : MonoBehaviour
{
    Animator m_Anim;
    Toggle m_Toggle;

    private void Awake()
    {
        m_Toggle = this.GetComponent<Toggle>();
        m_Anim = this.GetComponent<Animator>();
        m_Toggle.onValueChanged.AddListener(OnValueChange);
    }

    private void OnEnable()
    {
        OnValueChange(m_Toggle.isOn);

    }

    public void OnValueChange(bool value)
    {
        m_Anim.SetBool("Select", !value);
    }

}
