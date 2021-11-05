using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalToggle : MonoBehaviour
{
    [SerializeField] CanvasGroup m_canvasGroup = default;

    Toggle m_Toggle;
    private void Awake()
    {
        m_Toggle = this.GetComponent<Toggle>();
        m_Toggle.onValueChanged.AddListener(OnToggleChange);
    }

    private void Start()
    {
        OnToggleChange(m_Toggle.isOn);
    }

    private void OnToggleChange(bool value)
    {
        if (value)
        {
            m_canvasGroup.alpha = 1;
            m_canvasGroup.blocksRaycasts = true;
        }
        else
        {
            m_canvasGroup.alpha = 0;
            m_canvasGroup.blocksRaycasts = false;
        }
    }


}
