using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventPermeater : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerClickHandler,IPointerEnterHandler
{
    // 事件穿透对象
    private GameObject m_Target;
    [SerializeField] GameObject eventPermeaterMask = default;

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            m_Target = target;
            eventPermeaterMask.SetActive(true);
        }
        else
        {
            eventPermeaterMask.SetActive(false);
        }
    }
    // 监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }

    // 监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        //PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }
    //监听进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerEnterHandler);
    }

    // 把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (m_Target == results[i].gameObject)
            {
                // 如果是目标物体，则把事件透传下去，然后break
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                break;
            }
        }
    }


}
