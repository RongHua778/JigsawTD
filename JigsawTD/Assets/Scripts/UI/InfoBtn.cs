using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Camera mainCam;
    [TextArea(2, 3)]
    [SerializeField] string content = default;
    [SerializeField] Vector3 offset = default;

    private void Awake()
    {
        mainCam = Camera.main;
    }
    public void SetContent(string content)
    {
        this.content = content;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (content == "" || GameManager.Instance == null)
            return;
        GameManager.Instance.ShowTempTips(content, mainCam.WorldToScreenPoint(transform.position + offset));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.HideTempTips();
    }


}
