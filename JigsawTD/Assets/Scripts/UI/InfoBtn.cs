using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Camera mainCam;
    string content;

    private void Start()
    {
        mainCam = Camera.main;
    }
    public void SetContent(string content)
    {
        this.content = content;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.ShowTempTips(content, mainCam.WorldToScreenPoint(transform.position));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HideTempTips();
    }


}
