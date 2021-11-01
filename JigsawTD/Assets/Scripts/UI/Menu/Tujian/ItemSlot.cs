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
    [SerializeField] Color lockColor = default;
    [SerializeField] GameObject lockIcon = default;
    Vector2 tipsPos = default;
    bool isLock = false;
    private void Start()
    {
        SetContent();
        tipsPos = new Vector2(Screen.width - 400f, 540);
    }
    public void SetContent()
    {
        ItemLockInfo info = Game.Instance.SaveData.SaveItemDIC[contenAtt.Name];
        isLock = info.isLock;
        lockIcon.SetActive(info.isLock);
        icon.sprite = contenAtt.Icon;
        nameTxt.text = GameMultiLang.GetTraduction(contenAtt.Name);
        icon.color = info.isLock ? lockColor : Color.white;
        nameTxt.color = info.isLock ? Color.gray : Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLock)
            return;
        contenAtt.MenuShowTips(tipsPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuUIManager.Instance.HideTips();
    }
}
