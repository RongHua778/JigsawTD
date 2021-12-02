using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dianji : MonoBehaviour, ICanvasRaycastFilter
{
    /// <summary>
    /// 要点击的按钮
    /// </summary>
    /// 
    [SerializeField] Canvas m_Canvas;
    public GameObject onClockButton;
    private Button btn;
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 localPoint;
        Debug.Log(screenPoint);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_Canvas.GetComponent<RectTransform>(), screenPoint, eventCamera, out localPoint);
        Debug.Log("Local="+localPoint);

        
        if (onClockButton.GetComponent<RectTransform>().rect.Contains(localPoint))
        {
            Debug.Log("False");
            return false;
        }
        else
        {
            Debug.Log("True");

            return true;
        }

    }
}
