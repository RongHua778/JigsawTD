using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] ContentAttribute contenAtt = default;
    [SerializeField] Image icon = default;
    [SerializeField] Text nameTxt = default;
    [SerializeField] Color normalColor = default;
    [SerializeField] Color lockColor = default;
    [SerializeField] GameObject lockIcon = default;
    bool isLock = false;
    //private void Start()
    //{
    //    SetContent();
    //}
    public void SetContent()
    {
        isLock = contenAtt.isLock;
        lockIcon.SetActive(isLock);
        icon.sprite = contenAtt.Icon;
        nameTxt.text = isLock ? "" : GameMultiLang.GetTraduction(contenAtt.Name);
        icon.color = isLock ? lockColor : Color.white;
        nameTxt.color = isLock ? Color.gray : normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLock)
            return;
        contenAtt.MenuShowTips(StaticData.RightTipsPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuUIManager.Instance.HideTips();
    }
}
