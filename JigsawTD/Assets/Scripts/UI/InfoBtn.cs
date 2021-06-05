using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string infoKey = default;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.ShowTempTips(StaticData.TipsInfoDIC[infoKey](), transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HideTempTips();
    }


}
