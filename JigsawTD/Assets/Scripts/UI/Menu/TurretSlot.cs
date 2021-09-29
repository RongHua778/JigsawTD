using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurretSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Vector2 tipsPos = default;
    SetHolder m_SetHolder;

    [SerializeField] private bool isLock;
    public bool IsLock
    {
        get => isLock;
        set
        {
            isLock = value;
            lockIcon.gameObject.SetActive(value);
        }
    }

    [SerializeField] Image lockIcon = default;
    [SerializeField] Image icon = default;
    [SerializeField] TurretAttribute TurretAtt;

    bool isSelect = false;
    public bool IsSelect
    {
        get => isSelect;
        set
        {
            isSelect = value;
            _Anim.SetBool("IsSelect", isSelect);
        }
    }

    Animator _Anim;

    public void Initialize(SetHolder setHolder, SelectInfo selectInfo)
    {
        this.m_SetHolder = setHolder;
        _Anim = this.GetComponent<Animator>();
        this.IsSelect = selectInfo.isSelect;
        this.IsLock = selectInfo.isLock;
        icon.sprite = TurretAtt.Icon;
    }

    public void OnSlotClick()
    {
        if (isSelect)
        {
            IsSelect = false;
            m_SetHolder.SelectedCount--;
        }
        else
        {
            if (IsLock)
            {
                MenuUIManager.Instance.ShowMessage(GameMultiLang.GetTraduction("ISLOCK"));
                return;
            }
            if (m_SetHolder.SelectedCount < m_SetHolder.MaxSelectCount)
            {
                IsSelect = true;
                m_SetHolder.SelectedCount++;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsLock)
            MenuUIManager.Instance.ShowTurretTips(TurretAtt, tipsPos);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuUIManager.Instance.HideTips();
    }
}
