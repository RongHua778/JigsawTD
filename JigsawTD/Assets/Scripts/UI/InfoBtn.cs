using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tipsInfo = default;
    public void OnPointerEnter(PointerEventData eventData)
    {
        LevelUIManager.Instance.ShowTempTips(tipsInfo,transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LevelUIManager.Instance.HideTempTips();
    }


}
