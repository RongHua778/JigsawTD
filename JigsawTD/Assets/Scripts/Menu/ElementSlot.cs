using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private bool isLock;
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
    UIBattleSet m_UIBattleSet;
    [SerializeField] TurretAttribute TurretAtt;

    bool isSelect = false;
    public bool IsSelect
    {
        get => isSelect;
        set
        {
            isSelect = value;
            if (isSelect)
                m_UIBattleSet.SelectedElement++;
            else
                m_UIBattleSet.SelectedElement--;
            _Anim.SetBool("IsSelect", isSelect);
        }
    }

    Animator _Anim;

    public void Initialize(UIBattleSet battleSet, ElementSelect selectInfo)
    {
        this.m_UIBattleSet = battleSet;
        _Anim = this.GetComponent<Animator>();
        this.IsSelect = selectInfo.isSelect;
        this.IsLock = selectInfo.isLock;
        icon.sprite = TurretAtt.TurretLevels[4].TurretIcon;
    }

    public void OnSlotClick()
    {
        if (isSelect)
        {
            IsSelect = false;
        }
        else
        {
            if (!IsLock && m_UIBattleSet.SelectedElement < m_UIBattleSet.MaxSelectElement)
            {
                IsSelect = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!IsLock)
            MenuUIManager.Instance.ShowTurretTips(TurretAtt);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuUIManager.Instance.HideTips();
    }
}
