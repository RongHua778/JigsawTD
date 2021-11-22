using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnlockSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ContentAttribute contentAtt;
    [SerializeField] Image icon = default;
    [SerializeField] Text nameTxt = default;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (contentAtt != null)
            contentAtt.GameShowTips(StaticData.LeftTipsPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.HideTips();
    }

    public void SetBonus(ContentAttribute att)
    {
        contentAtt = att;
        icon.sprite = att.Icon;
        nameTxt.text = GameMultiLang.GetTraduction(att.Name);
    }


}
