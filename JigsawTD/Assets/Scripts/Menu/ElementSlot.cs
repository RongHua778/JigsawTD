using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ElementSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] Image icon = default;
    UIBattleSet m_UIBattleSet;
    [HideInInspector] public TurretAttribute TurretAtt;
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

    public void Initialize(UIBattleSet battleSet,TurretAttribute att)
    {
        this.m_UIBattleSet = battleSet;
        _Anim = this.GetComponent<Animator>();
        TurretAtt = att;
        this.IsSelect = isSelect;
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
            if (m_UIBattleSet.SelectedElement < m_UIBattleSet.MaxSelectElement)
            {
                IsSelect = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuUIManager.Instance.ShowTurretTips(TurretAtt);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuUIManager.Instance.HideTips();
    }
}
